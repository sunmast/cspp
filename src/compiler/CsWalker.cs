using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HappyCspp.Compiler
{
    partial class CsWalker
    {
        private CodeWriter hWriter, cppWriter, tempWriter;
        private StringBuilder tempStringBuilder = new StringBuilder(2048);
        private CodeWriter.Depth depth;
        private TypeModel typeModel;
        private CompilerConfig config;
        private MemberModel memberModel;

        private int memberAccessDepth = 0;

        private SemanticModel semantic;
        private TypeWrapper typeWrapper;

        private MemberMethodModel mainMethod;

        public CsWalker(TypeModel typeModel, CompilerConfig config)
        {
            this.typeModel = typeModel;
            this.semantic = typeModel.SemanticModel;
            this.typeWrapper = new TypeWrapper(typeModel.SemanticModel, config);

            this.config = config;
        }

        public void Compile(CodeWriter hWriter, CodeWriter cppWriter, CodeWriter.Depth depth)
        {
            this.hWriter = hWriter;
            this.cppWriter = cppWriter;
            this.tempWriter = new CodeWriter(new StringWriter(this.tempStringBuilder), depth);
            this.depth = depth;

            this.cppWriter.NewLine();

            foreach (var nsImport in typeModel.UsingDirectives)
            {
                // All type identifies in h files will be expanded to avoid namespace problems
                // Refer to: http://stackoverflow.com/questions/223021/whats-the-scope-of-the-using-declaration-in-c

                string cppUsing = this.SyntaxName(nsImport.Name);

                // std and sys namespaces are always expanded in code
                if (cppUsing == "std" || cppUsing == "sys")
                    continue;

                // Only declare usings in cpp files to maintain code readability
                if (nsImport.Alias == null)
                {
                    this.cppWriter.WriteLine("using namespace {0};", cppUsing);
                }
                else
                {
                    this.cppWriter.WriteLine("/* using {0} = {1}; */ #define {0} {1}", nsImport.Alias.Name, cppUsing);
                }
            }

            if (typeModel.UsingDirectives.Count > 0)
            {
                this.cppWriter.NewLine();
            }
                
            if (this.typeModel.NsNameParts != null)
            {
                foreach (string n in this.typeModel.NsNameParts)
                {
                    this.cppWriter.Write("namespace {0} {{ ", n);
                }

                this.cppWriter.NewLine();
            }

            if (typeModel is ClassModel)
            {
                this.CompileTypeDeclaration((typeModel as ClassModel).ClassDeclaration);
            }
            else if (typeModel is StructModel)
            {
                this.CompileTypeDeclaration((typeModel as StructModel).StructDeclaration);
            }
            else if (typeModel is InterfaceModel)
            {
                this.CompileInterfaceDeclaration((typeModel as InterfaceModel).InterfaceDeclaration);
            }
            else if (typeModel is EnumModel)
            {
                this.CompileEnumDeclaration((typeModel as EnumModel).EnumDeclaration);
            }
            else if (typeModel is DelegateModel)
            {
                this.CompileDelegateDeclaration((typeModel as DelegateModel).DelegateDeclaration);
            }

            if (this.typeModel.NsNameParts != null)
            {
                this.cppWriter.WriteLine(new string('}', this.typeModel.NsNameParts.Length * 2));
            }

            if (this.mainMethod != null)
            {
                // Hard coded main definition
                string w = config.PreferWideChar ? "w" : null;

                this.cppWriter.NewLine();
                this.cppWriter.WriteLine("int {0}main(int argc, {0}cstring argv[]) {{", w);
                this.depth++;
                this.cppWriter.WriteLine("_array<{0}cstring> args = new_<array<{0}cstring>>(argc - 1);", w);
                this.cppWriter.WriteLine("for (int i = 1; i < argc; i++) {{");
                this.depth++;
                this.cppWriter.WriteLine("args.IndexOf<{0}cstring>(i - 1) = argv[i];", w);
                this.depth--;
                this.cppWriter.WriteLine("}}");
                this.cppWriter.WriteLine("return {0}::Run(args);", this.typeModel.FullName);
                this.depth--;
                this.cppWriter.WriteLine("}}");
            }
        }

        internal void CompileTypeDeclaration(TypeDeclarationSyntax typeDeclaration)
        {
            bool isValueType = typeDeclaration is StructDeclarationSyntax;

            if (isValueType && typeDeclaration.BaseList != null)
            {
                // Structs aren't allowed to have a base type, even interfaces (different from C#)
                throw Util.NewSyntaxNotSupportedException(typeDeclaration.BaseList);
            }

            string bases = this.SyntaxBaseList(typeDeclaration.BaseList, typeDeclaration.TypeParameterList);

            this.hWriter.WriteLine(this.SyntaxTemplateTypeDeclaration(typeDeclaration.TypeParameterList));
            this.hWriter.WriteLine("{0} {1}{2} {{", isValueType ? "struct" : "class", typeDeclaration.Identifier, bases);

            this.depth++;

            string lastScopeModifier = null;

            foreach (var member in this.typeModel.Members)
            {
                if (member.ScopeModifier != lastScopeModifier)
                {
                    this.hWriter.NewLine();

                    this.depth--;

                    lastScopeModifier = member.ScopeModifier;
                    this.hWriter.WriteLine(lastScopeModifier + ":");

                    this.depth++;
                }

                this.hWriter.NewLine();
                this.cppWriter.NewLine();

                this.memberModel = member;
                member.Compile(this);
                this.memberModel = null;
            }

            if (isValueType)
            {
                this.hWriter.NewLine();

                this.depth--;
                this.hWriter.WriteLine("public: // auto default constructor");
                this.depth++;

                this.hWriter.NewLine();

                this.hWriter.WriteLine(typeDeclaration.Identifier + "() {{ std::memset(this, 0, sizeof(*this)); }}");
            }

            if (typeModel.AutoProperties.Count > 0)
            {
                this.hWriter.NewLine();

                this.depth--;
                this.hWriter.WriteLine("private: // auto properties storage");
                this.depth++;

                this.hWriter.NewLine();

                foreach (var kvp in typeModel.AutoProperties)
                {
                    this.hWriter.WriteLine("{0} {1};", this.SyntaxType(kvp.Value), Consts.PropStoragePrefix + kvp.Key);
                }
            }

            this.depth--;
            this.hWriter.WriteLine("}};");
        }

        internal void CompileInterfaceDeclaration(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            string bases = this.SyntaxBaseList(interfaceDeclaration.BaseList, interfaceDeclaration.TypeParameterList);

            this.hWriter.WriteLine(this.SyntaxTemplateTypeDeclaration(interfaceDeclaration.TypeParameterList));
            this.hWriter.WriteLine("class {0}{1} {{", interfaceDeclaration.Identifier, bases);

            this.hWriter.NewLine();
            this.hWriter.WriteLine("public:"); // members in C# interface are all public
            this.hWriter.NewLine();

            this.depth++;

            this.hWriter.WriteLine("virtual ~{0}() {{}}", interfaceDeclaration.Identifier);

            foreach (var member in this.typeModel.Members)
            {
                this.hWriter.NewLine();

                this.memberModel = member;
                member.Compile(this);
            }

            this.depth--;
            this.hWriter.WriteLine("}};");
        }

        internal void CompileDelegateDeclaration(DelegateDeclarationSyntax delegateDeclaration)
        {
            this.hWriter.WriteLine(this.SyntaxTemplateTypeDeclaration(delegateDeclaration.TypeParameterList));

            this.hWriter.WriteLine(
                "{0} (*{1}){2};",
                typeWrapper.Wrap(this.semantic.GetTypeInfo(delegateDeclaration.ReturnType), true),
                delegateDeclaration.Identifier.Text,
                this.SyntaxParameterList(delegateDeclaration.ParameterList, true));
        }

        internal void CompileEnumDeclaration(EnumDeclarationSyntax enumDeclaration)
        {
            string enumName = enumDeclaration.Identifier.Text;

            this.hWriter.WriteLine("struct {0} {{", enumName);
            this.depth++;

            foreach (var member in enumDeclaration.Members)
            {
                this.hWriter.Write("{0}_{1}", enumName, member.Identifier.Text);

                if (member.EqualsValue != null)
                {
                    this.hWriter.Append(" = " + member.EqualsValue.Value);
                }

                this.hWriter.AppendLine(",");
            }

            this.depth--;
            this.hWriter.WriteLine("}};");
        }
    }
}
