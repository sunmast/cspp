using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HappyCspp.Compiler
{
    abstract class TypeModel // e.g. class Foo<T>
    {
        private bool isAttributesProcessed, isImported;
        private NamespaceDeclarationSyntax nsDeclaration;

        private string nsName, fullName, alias, altAlias;
        private string[] nsNameParts;

        public SemanticModel SemanticModel{ get; private set; }

        public string Name { get; protected set; }

        public bool IsValueType { get; protected set; }

        public List<UsingDirectiveSyntax> UsingDirectives { get; private set; }

        public SyntaxList<AttributeListSyntax> Attributes { get; private set; }

        public TypeParameterListSyntax GenericArgs { get; private set; }

        public SyntaxList<TypeParameterConstraintClauseSyntax> GenericConstraints { get; set; }

        public ISymbol[] TypeArgs { get; set; }

        public MemberModel[] Members { get; protected set; }

        public Dictionary<string, TypeSyntax> AutoProperties { get; set; }

        public MemberIndexerModel Indexer { get; set; }

        public CsWalker Compiler { get; set; }

        public string NsName
        {
            get
            {
                if (this.nsName == null)
                {
                    this.nsName = this.nsDeclaration == null ? null : this.nsDeclaration.Name.ToString().Replace(".", "::");
                }

                return this.nsName;
            }
        }

        public string[] NsNameParts
        {
            get
            {
                if (this.NsName == null)
                {
                    return null;
                }

                if (this.nsNameParts == null)
                {
                    this.nsNameParts = this.NsName.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
                }

                return this.nsNameParts;
            }
        }

        public string Alias
        {
            get
            {
                if (!this.isAttributesProcessed)
                {
                    this.ProcessAttributes();
                }

                return this.alias;
            }
        }

        public string AltAlias
        {
            get
            {
                if (!this.isAttributesProcessed)
                {
                    this.ProcessAttributes();
                }

                return this.altAlias;
            }
        }

        public bool IsImported
        {
            get
            {
                if (!this.isAttributesProcessed)
                {
                    this.ProcessAttributes();
                }

                return this.isImported;
            }
        }

        public string FullName
        {
            get
            {
                if (this.fullName == null)
                {
                    this.fullName = this.NsName == null ? this.Name : this.NsName + "::" + this.Name;
                }

                return this.fullName;
            }
        }

        public TypeModel(SemanticModel semanticModel)
        {
            this.SemanticModel = semanticModel;

            this.UsingDirectives = new List<UsingDirectiveSyntax>();
            this.AutoProperties = new Dictionary<string, TypeSyntax>();
        }

        public virtual void Merge(TypeModel partialClassModel)
        {
            Debug.Assert(this.nsDeclaration.Name == partialClassModel.nsDeclaration.Name);

            this.UsingDirectives.AddRange(partialClassModel.UsingDirectives);
            //this.ClassDeclaration.TypeParameterList.AddParameters
        }

        public static TypeModel Create(NamespaceDeclarationSyntax ns, SyntaxNode node, List<UsingDirectiveSyntax> usings, SemanticModel semanticModel)
        {
            TypeModel model;

            if (node is ClassDeclarationSyntax)
            {
                var classDeclaraion = node as ClassDeclarationSyntax;

                model = new ClassModel(semanticModel)
                {
                    Name = classDeclaraion.Identifier.Text,
                    ClassDeclaration = classDeclaraion,
                    Attributes = classDeclaraion.AttributeLists,
                    GenericArgs = classDeclaraion.TypeParameterList,
                    GenericConstraints = classDeclaraion.ConstraintClauses,
                };

                model.Members = MemberModel.GetSortedMembers(model, classDeclaraion.Members);
            }
            else if (node is StructDeclarationSyntax)
            {
                var structDeclaration = node as StructDeclarationSyntax;

                model = new StructModel(semanticModel)
                {
                    Name = structDeclaration.Identifier.Text,
                    StructDeclaration = structDeclaration,
                    Attributes = structDeclaration.AttributeLists,
                    GenericArgs = structDeclaration.TypeParameterList,
                    GenericConstraints = structDeclaration.ConstraintClauses,
                };

                model.Members = MemberModel.GetSortedMembers(model, structDeclaration.Members);
            }
            else if (node is InterfaceDeclarationSyntax)
            {
                var interfaceDeclaration = node as InterfaceDeclarationSyntax;

                model = new InterfaceModel(semanticModel)
                {
                    Name = interfaceDeclaration.Identifier.Text,
                    InterfaceDeclaration = interfaceDeclaration,
                    Attributes = interfaceDeclaration.AttributeLists,
                    GenericArgs = interfaceDeclaration.TypeParameterList,
                    GenericConstraints = interfaceDeclaration.ConstraintClauses,
                };

                model.Members = MemberModel.GetSortedMembers(model, interfaceDeclaration.Members);
            }
            else if (node is DelegateDeclarationSyntax)
            {
                var delegateDeclaration = node as DelegateDeclarationSyntax;

                model = new DelegateModel(semanticModel)
                {
                    Name = delegateDeclaration.Identifier.Text,
                    DelegateDeclaration = delegateDeclaration,
                    Attributes = delegateDeclaration.AttributeLists,
                    GenericArgs = delegateDeclaration.TypeParameterList,
                    GenericConstraints = delegateDeclaration.ConstraintClauses
                };
            }
            else if (node is EnumDeclarationSyntax)
            {
                var enumDeclaration = node as EnumDeclarationSyntax;

                model = new EnumModel(semanticModel)
                {
                    Name = enumDeclaration.Identifier.Text,
                    EnumDeclaration = enumDeclaration,
                    Attributes = enumDeclaration.AttributeLists,
                };
            }
            else
            {
                throw Util.NewSyntaxNotSupportedException(node);
            }

            model.UsingDirectives = usings;
            model.nsDeclaration = ns;

            return model;
        }

        private void ProcessAttributes()
        {
            if (this.Attributes.Count > 0)
            {
                foreach (var attrList in this.Attributes)
                {
                    foreach (var attr in attrList.Attributes)
                    {
                        string name = attr.Name.ToString();
                        if (name == "Imported" || name == "ImportedAttribute")
                        {
                            this.isImported = true;
                        }
                        else if (name == "Alias" || name == "AliasAttribute")
                        {
                            Util.GetAliases(attr.ArgumentList.Arguments, out this.alias, out this.altAlias);
                        }
                    }
                }
            }

            this.isAttributesProcessed = true;
        }
    }

    [DebuggerDisplay("class {Name}")]
    class ClassModel : TypeModel
    {
        public ClassDeclarationSyntax ClassDeclaration { get; set; }

        //public override void Merge(TypeModel partialTypeModel, string sourceFile)
        //{
        //    base.Merge(partialTypeModel, sourceFile);

        //    var partialClassModel = partialTypeModel as ClassModel;

        //    this.ClassDeclaration.Modifiers.AddRange(partialClassModel.ClassDeclaration.Modifiers);
        //    this.ClassDeclaration.AddBaseListTypes(partialClassModel.ClassDeclaration.BaseList.Types.ToArray());
        //}

        public ClassModel(SemanticModel semanticModel)
            : base(semanticModel)
        {
            this.IsValueType = false;
        }
    }

    [DebuggerDisplay("struct {Name}")]
    class StructModel : TypeModel
    {
        public StructDeclarationSyntax StructDeclaration { get; set; }

        public StructModel(SemanticModel semanticModel)
            : base(semanticModel)
        {
            this.IsValueType = true;
        }
    }

    [DebuggerDisplay("interface {Name}")]
    class InterfaceModel : TypeModel
    {
        public InterfaceDeclarationSyntax InterfaceDeclaration { get; set; }

        public InterfaceModel(SemanticModel semanticModel)
            : base(semanticModel)
        {
            this.IsValueType = false;
        }
    }

    [DebuggerDisplay("enum {Name}")]
    class EnumModel : TypeModel
    {
        public EnumDeclarationSyntax EnumDeclaration { get; set; }

        public EnumModel(SemanticModel semanticModel)
            : base(semanticModel)
        {
            this.IsValueType = true;
        }
    }

    [DebuggerDisplay("delegate {Name}")]
    class DelegateModel : TypeModel
    {
        public DelegateDeclarationSyntax DelegateDeclaration { get; set; }

        public DelegateModel(SemanticModel semanticModel)
            : base(semanticModel)
        {
            this.IsValueType = false;
        }
    }
}
