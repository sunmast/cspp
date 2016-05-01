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
        private StringBuilder tempStringBuilder = new StringBuilder();
        private CodeWriter.Depth depth;
        private TypeModel typeModel;
        private MemberModel memberModel;

        private int memberAccessDepth = 0;

        private SemanticModel semantic;

        private MemberMethodModel mainMethod;

        public CsWalker(TypeModel typeModel)
        {
            this.typeModel = typeModel;
            this.semantic = typeModel.SemanticModel;
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
                string w = App.PreferWideChar ? "w" : null;

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
                this.WrapTypeName(this.semantic.GetTypeInfo(delegateDeclaration.ReturnType), true),
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

        private string WrapTypeName(TypeSyntax typeSyntax, bool wrapSptr, IEnumerable<UsingDirectiveSyntax> usings = null)
        {
            if (typeSyntax == null)
                return null;
			
            TypeInfo typeInfo = this.semantic.GetTypeInfo(typeSyntax);
            return this.WrapTypeName(typeInfo, wrapSptr, usings);
        }

        private string WrapTypeName(TypeInfo typeInfo, bool wrapSptr, IEnumerable<UsingDirectiveSyntax> usings = null)
        {
            // Wrap ref type with _<> if wrapSptr
            // Change type name to its alias

            if (typeInfo.Type is IArrayTypeSymbol)
            {
                return this.WrapArrayTypeName(typeInfo.Type as IArrayTypeSymbol, wrapSptr, usings);
            }

            INamedTypeSymbol namedType = typeInfo.Type as INamedTypeSymbol;
            if (namedType == null)
            {
                return null;
            }
            else
            {
                return this.WrapNamedTypeSymbol(namedType, wrapSptr, usings);
            }
        }

        private string WrapArrayTypeName(IArrayTypeSymbol typeInfo, bool wrapSptr, IEnumerable<UsingDirectiveSyntax> usings = null)
        {
            string type;
            if (typeInfo.ElementType is IArrayTypeSymbol)
            {
                type = this.WrapArrayTypeName(typeInfo.ElementType as IArrayTypeSymbol, wrapSptr, usings);
            }
            else
            {
                type = this.WrapNamedTypeSymbol(typeInfo.ElementType as INamedTypeSymbol, wrapSptr, usings);
            }

            string rank = typeInfo.Rank == 1 ? null : ", " + typeInfo.Rank.ToString();
            if (wrapSptr)
            {
                return string.Format("_{0}<{1}{2}>", Consts.Array, type, rank);
            }
            else
            {
                return string.Format("{0}<{1}{2}>", Consts.Array, type, rank);
            }
        }

        private string WrapNamedTypeSymbol(INamedTypeSymbol namedType, bool wrapSptr, IEnumerable<UsingDirectiveSyntax> usings = null)
        {
            // Special types
            switch (namedType.SpecialType)
            {
                case SpecialType.System_String:
                    return App.PreferWideChar ? "wstring" : "string"; // always considered as value type
                case SpecialType.System_Char:
                    return App.PreferWideChar ? "wchar_t" : "char";
                case SpecialType.System_Void:
                    return "void";
                case SpecialType.System_Object:
                    return wrapSptr ? "_<object>" : "object";
                case SpecialType.System_Byte:
                    return "uint8_t";
                case SpecialType.System_Int16:
                    return "int16_t";
                case SpecialType.System_Int32:
                    return "int32_t";
                case SpecialType.System_Int64:
                    return "int64_t";
                case SpecialType.System_Single:
                    return "float";
                case SpecialType.System_Double:
                    return "double";
                case SpecialType.System_Decimal:
                    return "sys::decimal";
                case SpecialType.System_Boolean:
                    return "bool";
                case SpecialType.System_SByte:
                    return "int8_t";
                case SpecialType.System_UInt16:
                    return "uint16_t";
                case SpecialType.System_UInt32:
                    return "uint32_t";
                case SpecialType.System_UInt64:
                    return "uint64_t";
            }

            // Named non-generic types
            if (!namedType.IsGenericType)
            {
                string name = this.GetPrintName(namedType, usings);
                if (namedType.IsValueType || !wrapSptr)
                {
                    return name;
                }
                else
                {
                    return string.Format("_<{0}>", name);
                }
            }

            // Named generic types
            string fullName = namedType.ToString();
            return this.WrapGenericNamedTypeSymbol(fullName, wrapSptr, usings);
        }

        private string WrapGenericNamedTypeSymbol(string fullName, bool wrapSptr, IEnumerable<UsingDirectiveSyntax> usings = null)
        {
            INamedTypeSymbol namedType;
            int index = fullName.IndexOf('<');
            if (index < 0)
            {
                namedType = App.Compilation.GetTypeByMetadataName(fullName);
                return this.WrapNamedTypeSymbol(namedType, wrapSptr, usings);
            }

            List<string> list = new List<string>();
            string typeNames = fullName.Substring(index, fullName.Length - index);

            int depth = 0;

            for (int i = 0, j = 0; i < typeNames.Length; i++)
            {
                char c = typeNames[i];
                if (c == '<')
                {
                    depth++;

                    if (depth == 1)
                    {
                        j = i + 1;
                    }

                    continue;
                }

                if ((c == ',' || c == '>') && depth == 1)
                {
                    string typeArg = typeNames.Substring(j, i - j).Trim();

                    if (Consts.PrimitiveDatatypes.ContainsKey(typeArg))
                    {
                        typeArg = Consts.PrimitiveDatatypes[typeArg];
                    }

                    string wrappedTypeArg = this.WrapGenericNamedTypeSymbol(typeArg, wrapSptr, usings);
                    list.Add(wrappedTypeArg);

                    j = i + 1;
                }

                if (c == '>')
                    depth--;
            }

            string typeKey = fullName.Substring(0, index) + "`" + list.Count;
            namedType = App.Compilation.GetTypeByMetadataName(typeKey);

            string name = this.GetPrintName(namedType, usings);

            if (namedType.IsValueType || !wrapSptr)
            {
                return string.Format("{0}<{1}>", name, string.Join(", ", list));
            }
            else
            {
                return string.Format("_<{0}<{1}>>", name, string.Join(", ", list));
            }
        }

        private string GetPrintName(INamedTypeSymbol namedType, IEnumerable<UsingDirectiveSyntax> usings)
        {
            string name = Util.GetSymbolAlias(namedType.GetAttributes()) ?? namedType.Name;
            string ns = string.IsNullOrEmpty(namedType.ContainingSymbol.Name) ? null : namedType.ContainingSymbol.ToString().Replace(".", "::");

            if (string.IsNullOrEmpty(name))
                return ns;
            if (string.IsNullOrEmpty(ns))
                return name;

            // TODO: lookup usings and shorter name
            return ns + "::" + name;
        }
    }
}
