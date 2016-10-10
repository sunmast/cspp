using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HappyCspp.Compiler
{
    abstract class MemberModel
    {
        private string alias, altAlias;
        private bool isAttributesProcessed;

		public TypeModel TypeModel { get; private set; }

        public SyntaxTokenList Modifiers { get; protected set; }

        public string ScopeModifier { get; protected set; }

        public bool IsPublic { get; protected set; }
        public bool IsInternal { get; protected set; }
        public bool IsProtected { get; protected set; }
        public bool IsPrivate { get; protected set; }
        public bool IsConst { get; protected set; }
        public bool IsStatic { get; protected set; }
        public bool IsExtern { get; protected set; }
        public bool IsAbstract { get; protected set; }
        public bool IsVirtual { get; protected set; }
        public bool IsOverride { get; protected set; }

        public int Rank1 { get; protected set; }
        public abstract int Rank2 { get; }
        public int Rank3 { get; protected set; }

		public TypeInfo Type { get; set; }

        public SyntaxList<AttributeListSyntax> Attributes { get; protected set; }

		public Stack<VariableModel> Variables { get; private set; }

        public int ScopeDepth { get; private set; }

        public string Name { get; protected set; }

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

		public MemberModel(TypeModel tm)
        {
			this.TypeModel = tm;
			this.Variables = new Stack<VariableModel>();
        }

        public void PushScope()
        {
            this.ScopeDepth++;
        }

        public void PopScope()
        {
            if (this.Variables.Count > 0)
            {
                var variable = this.Variables.Peek();
                while (variable.ScopeDepth == this.ScopeDepth)
                {
                    this.Variables.Pop();
                    if (this.Variables.Count == 0)
                        break;

                    variable = this.Variables.Peek();
                }
            }

            this.ScopeDepth--;
        }

        public string GetModifiers()
        {
            StringBuilder sb = new StringBuilder();

            if (this.IsConst) sb.Append("const ");
            if (this.IsStatic) sb.Append("static ");
            if (this.IsExtern) sb.Append("extern ");
            if (this.IsVirtual) sb.Append("virtual ");
            if (this.IsOverride) sb.Append("virtual ");

            return sb.Length == 0 ? null : sb.ToString();
        }

        public abstract void Compile(CsWalker cc);

        private void ProcessAttributes()
        {
            if (this.Attributes.Count > 0)
            {
                foreach (var attrList in this.Attributes)
                {
                    foreach (var attr in attrList.Attributes)
                    {
                        string name = attr.Name.ToString();
                        if (name == "Alias" || name == "AliasAttribute")
                        {
                            Util.GetAliases(attr.ArgumentList.Arguments, out this.alias, out this.altAlias);
                        }
                    }
                }
            }

            this.isAttributesProcessed = true;
        }

		public static MemberModel[] GetSortedMembers(TypeModel tm, SyntaxList<MemberDeclarationSyntax> memberDeclarations)
        {
            // public > internal > protected > private
            // descructor > constructor > field > indexer > event > property > method
            // const > static > non static

            List<MemberModel> list = new List<MemberModel>();

            foreach (var d in memberDeclarations)
            {
                if (d is BaseFieldDeclarationSyntax)
                {
                    if (d is FieldDeclarationSyntax)
                    {
                        var fieldDeclaration = d as FieldDeclarationSyntax;

                        foreach(var variable in fieldDeclaration.Declaration.Variables)
                        {
							list.Add(new MemberFieldModel(tm, fieldDeclaration, variable));
                        }
                    }
                    else if (d is EventFieldDeclarationSyntax)
                    {
                        var fieldDeclaration = d as EventFieldDeclarationSyntax;

                        foreach(var variable in fieldDeclaration.Declaration.Variables)
                        {
							list.Add(new MemberEventFieldModel(tm, fieldDeclaration, variable));
                        }
                    }
                    else
                    {
                        throw Util.NewSyntaxNotSupportedException(d);
                    }
                }
                else if (d is BasePropertyDeclarationSyntax)
                {
                    if (d is PropertyDeclarationSyntax)
                    {
                        list.Add(new MemberPropertyModel(tm, d as PropertyDeclarationSyntax));
                    }
                    else if (d is IndexerDeclarationSyntax)
                    {
						list.Add(new MemberIndexerModel(tm, d as IndexerDeclarationSyntax));
                    }
                    else if (d is EventDeclarationSyntax)
                    {
						list.Add(new MemberEventPropertyModel(tm, d as EventDeclarationSyntax));
                    }
                    else
                    {
                        throw Util.NewSyntaxNotSupportedException(d);
                    }
                }
                else if (d is BaseMethodDeclarationSyntax)
                {
                    if (d is MethodDeclarationSyntax)
                    {
						list.Add(new MemberMethodModel(tm, d as MethodDeclarationSyntax));
                    }
                    else if (d is OperatorDeclarationSyntax)
                    {
						list.Add(new MemberOperatorModel(tm, d as OperatorDeclarationSyntax));
                    }
                    else if (d is ConversionOperatorDeclarationSyntax)
                    {
						list.Add(new MemberConversionOperatorModel(tm, d as ConversionOperatorDeclarationSyntax));
                    }
                    else if (d is ConstructorDeclarationSyntax)
                    {
						list.Add(new MemberConstructorModel(tm, d as ConstructorDeclarationSyntax));
                    }
                    else if (d is DestructorDeclarationSyntax)
                    {
						list.Add(new MemberDestructorModel(tm, d as DestructorDeclarationSyntax));
                    }
                    else
                    {
                        throw Util.NewSyntaxNotSupportedException(d);
                    }
                }
                else if (d is DelegateDeclarationSyntax)
                {
					list.Add(new MemberDelegateModel(tm, d as DelegateDeclarationSyntax));
                }
                else if (d is BaseTypeDeclarationSyntax)
                {
                    if (d is StructDeclarationSyntax)
                    {
						list.Add(new MemberStructModel(tm, d as StructDeclarationSyntax));
                    }
                    else if (d is ClassDeclarationSyntax)
                    {
						list.Add(new MemberClassModel(tm, d as ClassDeclarationSyntax));
                    }
                    else if (d is EnumDeclarationSyntax)
                    {

						list.Add(new MemberEnumModel(tm, d as EnumDeclarationSyntax));
                    }
                    else if (d is InterfaceDeclarationSyntax)
                    {

						list.Add(new MemberInterfaceModel(tm, d as InterfaceDeclarationSyntax));
                    }
                    else
                    {
                        throw Util.NewSyntaxNotSupportedException(d);
                    }
                }
                else
                {
                    throw Util.NewSyntaxNotSupportedException(d);
                }
            }

            list.Sort((x, y) =>
            {
                if (x.Rank1 != y.Rank1)
                {
                    return x.Rank1 - y.Rank1;
                }

                if (x.Rank2 != y.Rank2)
                {
                    return x.Rank2 - y.Rank2;
                }

                if (x.Rank3 != y.Rank3)
                {
                    return x.Rank3 - y.Rank3;
                }

                return 0;
            });

            return list.ToArray();
        }
    }
    abstract class MemberModel<T> : MemberModel where T : MemberDeclarationSyntax
    {
        public T Declaration { get; private set; }

		public MemberModel(TypeModel tm, T declaration, SyntaxTokenList modifiers) : base(tm)
        {
            this.Declaration = declaration;

            this.Modifiers = modifiers;

            this.Rank1 = this.Rank3 = 1000;

            // default to private member
            this.IsPrivate = true;
            this.ScopeModifier = "private";

            foreach (var modifier in modifiers)
            {
                switch (modifier.Text)
                {
                    case "public":
                        this.ScopeModifier = "public";
                        this.IsPublic = true;
                        this.Rank1 = 1;
                        break;
                    case "internal":
                        this.ScopeModifier = "public"; // for now internal is treated as public
                        this.IsInternal = true;
                        this.Rank1 = 2;
                        break;
                    case "protected":
                        this.ScopeModifier = "protected";
                        this.IsProtected = true;
                        this.Rank1 = 3;
                        break;
                    case "private":
                        this.ScopeModifier = "private";
                        this.IsPrivate = true;
                        this.Rank1 = 4;
                        break;
                    case "const":
                        this.IsConst = true;
                        this.Rank3 = 1;
                        break;
                    case "static":
                        this.IsStatic = true;
                        this.Rank3 = 2;
                        break;
                    case "extern":
                        this.IsExtern = true;
                        this.Rank3 = 3; // static extern is lower than static
                        break;
                    case "abstract":
                        this.IsAbstract = true;
                        this.Rank3 = 4;
                        break;
                    case "virtual":
                        this.IsVirtual = true;
                        this.Rank3 = 5;
                        break;
                    case "override":
                        this.IsOverride = true;
                        this.Rank3 = 5; // same rank as virtual
                        break;
                }
            } 
        }
    }

    #region fields

    abstract class MemberBaseFieldModel : MemberModel<BaseFieldDeclarationSyntax>
    {
        public VariableDeclaratorSyntax Variable { get; private set; }

		public MemberBaseFieldModel(TypeModel tm, BaseFieldDeclarationSyntax declaration, VariableDeclaratorSyntax variable)
            : base(tm, declaration, declaration.Modifiers)
        {
			this.Type = tm.SemanticModel.GetTypeInfo(declaration.Declaration.Type);
            this.Variable = variable;
            this.Name = variable.Identifier.Text;
            this.Attributes = declaration.AttributeLists;
        }
    }

    class MemberFieldModel : MemberBaseFieldModel
    {
        public override int Rank2
        {
            get { return 1; }
        }

		public MemberFieldModel(TypeModel tm, FieldDeclarationSyntax declaration, VariableDeclaratorSyntax variable)
            : base(tm, declaration, variable)
        {
            //if (variable.Initializer != null && this.IsStatic)
            //{
            //    this.IsStatic = false;
            //    this.IsConst = true; // C++ requires static field with initializer be const
            //}
        }

        public override void Compile(CsWalker cc)
        {
            cc.CompileMemberField(this);
        }
    }

    class MemberEventFieldModel : MemberBaseFieldModel
    {
        public override int Rank2
        {
            get { return 2; }
        }

		public MemberEventFieldModel(TypeModel tm, EventFieldDeclarationSyntax declaration, VariableDeclaratorSyntax variable)
            : base(tm, declaration, variable)
        {
        }

        public override void Compile(CsWalker cc)
        {
            cc.CompileMemberEventField(this);
        }
    }

    #endregion

    #region property

    abstract class MemberBasePropertyModel<T> : MemberModel<BasePropertyDeclarationSyntax> where T : BasePropertyDeclarationSyntax
    {
        public new T Declaration { get { return base.Declaration as T; } }

		public MemberBasePropertyModel(TypeModel tm, BasePropertyDeclarationSyntax declaration)
            : base(tm, declaration, declaration.Modifiers)
        {
			this.Type = tm.SemanticModel.GetTypeInfo(declaration.Type);
            this.Attributes = declaration.AttributeLists;
        }
    }

    class MemberIndexerModel : MemberBasePropertyModel<IndexerDeclarationSyntax>
    {
        public override int Rank2
        {
            get { return 21; }
        }

        public MemberIndexerModel(TypeModel tm, IndexerDeclarationSyntax declaration)
            : base(tm, declaration)
        {
			this.Type = tm.SemanticModel.GetTypeInfo(declaration.Type);
        }

        public override void Compile(CsWalker cc)
        {
            cc.CompileMemberIndexer(this);
        }
    }

    class MemberEventPropertyModel : MemberBasePropertyModel<EventDeclarationSyntax>
    {
        public override int Rank2
        {
            get { return 22; }
        }

		public MemberEventPropertyModel(TypeModel tm, EventDeclarationSyntax declaration)
            : base(tm, declaration)
        {
            this.Name = this.Declaration.Identifier.Text;
        }

        public override void Compile(CsWalker cc)
        {
            cc.CompileMemberEventProperty(this);
        }
    }

    class MemberPropertyModel : MemberBasePropertyModel<PropertyDeclarationSyntax>
    {
        public override int Rank2
        {
            get { return 23; }
        }

		public MemberPropertyModel(TypeModel tm, PropertyDeclarationSyntax declaration)
            : base(tm, declaration)
        {
            this.Name = this.Declaration.Identifier.Text;
        }

        public override void Compile(CsWalker cc)
        {
            cc.CompileMemberProperty(this);
        }
    }

    #endregion

    #region method

    abstract class MemberBaseMethodModel<T> : MemberModel<BaseMethodDeclarationSyntax> where T : BaseMethodDeclarationSyntax
    {
        public new T Declaration { get { return base.Declaration as T; } }

        public abstract TypeParameterListSyntax TypeParameterList { get; }

        public abstract TypeSyntax ReturnType { get; }

        public MemberBaseMethodModel(TypeModel tm, BaseMethodDeclarationSyntax declaration)
            : base(tm, declaration, declaration.Modifiers)
        {
            this.Attributes = declaration.AttributeLists;
        }
    }

    class MemberConstructorModel : MemberBaseMethodModel<ConstructorDeclarationSyntax>
    {
        public override int Rank2
        {
            get { return -10; }
        }

        public override TypeParameterListSyntax TypeParameterList { get { return null; } }

        public override TypeSyntax ReturnType { get { return null; } }

        public MemberConstructorModel(TypeModel tm, ConstructorDeclarationSyntax declaration)
            : base(tm, declaration)
        {
            this.Name = this.Declaration.Identifier.Text;
        }

        public override void Compile(CsWalker cc)
        {
            cc.CompileMemberConstructor(this);
        }
    }

    class MemberDestructorModel : MemberBaseMethodModel<DestructorDeclarationSyntax>
    {
        public override int Rank2
        {
            get { return -20; }
        }

        public override TypeParameterListSyntax TypeParameterList { get { return null; } }

        public override TypeSyntax ReturnType { get { return null; } }

		public MemberDestructorModel(TypeModel tm, DestructorDeclarationSyntax declaration)
            : base(tm, declaration)
        {
            this.ScopeModifier = "public";
            this.Name = "~" + this.Declaration.Identifier.Text;
            this.Rank1 = 1;
            this.IsPublic = true; // destructor is always public
            this.IsVirtual = true; // destructor is always virtual
        }

        public override void Compile(CsWalker cc)
        {
            cc.CompileMemberDestructor(this);
        }
    }

    class MemberMethodModel : MemberBaseMethodModel<MethodDeclarationSyntax>
    {
        public override int Rank2
        {
            get { return 33; }
        }

        public override TypeParameterListSyntax TypeParameterList { get { return this.Declaration.TypeParameterList; } }

        public override TypeSyntax ReturnType { get { return this.Declaration.ReturnType; } }

		public ISymbol[] TypeArgs { get; set; }

		public MemberMethodModel(TypeModel tm, MethodDeclarationSyntax declaration)
            : base(tm, declaration)
        {
			this.Type = tm.SemanticModel.GetTypeInfo(declaration.ReturnType);
            this.Name = this.Declaration.Identifier.Text;

            if (this.Name == "Main")
            {
                this.IsPublic = true;
                this.ScopeModifier = "public";
                this.Rank1 = 0;
            }
        }

        public override void Compile(CsWalker cc)
        {
            cc.CompileMemberMethod(this);
        }
    }

    class MemberOperatorModel : MemberBaseMethodModel<OperatorDeclarationSyntax>
    {
        public override int Rank2
        {
            get { return 34; }
        }

        public override TypeParameterListSyntax TypeParameterList { get { return null; } }

        public override TypeSyntax ReturnType { get { return this.Declaration.ReturnType; } }

		public MemberOperatorModel(TypeModel tm, OperatorDeclarationSyntax declaration)
            : base(tm, declaration)
        {
        }

        public override void Compile(CsWalker cc)
        {
            cc.CompileMemberOperator(this);
        }
    }

    class MemberConversionOperatorModel : MemberBaseMethodModel<ConversionOperatorDeclarationSyntax>
    {
        public override int Rank2
        {
            get { return 35; }
        }

        public override TypeParameterListSyntax TypeParameterList { get { return null; } }

        public override TypeSyntax ReturnType { get { return null; } }

		public MemberConversionOperatorModel(TypeModel tm, ConversionOperatorDeclarationSyntax declaration)
            : base(tm, declaration)
        {
        }

        public override void Compile(CsWalker cc)
        {
            cc.CompileMemberConversionOperator(this);
        }
    }

    #endregion

    #region type

    class MemberDelegateModel : MemberModel<DelegateDeclarationSyntax>
    {
        public override int Rank2
        {
            get { return 41; }
        }

        public DelegateModel DelegateModel { get; private set; }

		public MemberDelegateModel(TypeModel tm, DelegateDeclarationSyntax declaration)
            : base(tm, declaration, declaration.Modifiers)
        {
			this.DelegateModel = (DelegateModel)TypeModel.Create(null, declaration, null, this.TypeModel.SemanticModel);
            //this.Type = declaration.ReturnType;
            this.Name = declaration.Identifier.Text;
            this.Attributes = declaration.AttributeLists;
        }

        public override void Compile(CsWalker cc)
        {
            cc.CompileDelegateDeclaration(this.Declaration as DelegateDeclarationSyntax);
        }
    }

    class MemberEnumModel : MemberModel<EnumDeclarationSyntax>
    {
        public override int Rank2
        {
            get { return 51; }
        }

        public EnumModel EnumModel { get; private set; }

        public MemberEnumModel(TypeModel tm, EnumDeclarationSyntax declaration)
            : base(tm, declaration, declaration.Modifiers)
        {
			this.EnumModel = (EnumModel)TypeModel.Create(null, declaration, null, this.TypeModel.SemanticModel);
            this.Name = declaration.Identifier.Text;
            this.Attributes = declaration.AttributeLists;
        }

        public override void Compile(CsWalker cc)
        {
            cc.CompileEnumDeclaration(this.Declaration as EnumDeclarationSyntax);
        }
    }

    class MemberStructModel : MemberModel<StructDeclarationSyntax>
    {
        public override int Rank2
        {
            get { return 52; }
        }

        public StructModel StructModel { get; private set; }

		public MemberStructModel(TypeModel tm, StructDeclarationSyntax declaration)
            : base(tm, declaration, declaration.Modifiers)
        {
			this.StructModel = (StructModel)TypeModel.Create(null, declaration, null, this.TypeModel.SemanticModel);
            this.Name = declaration.Identifier.Text;
            this.Attributes = declaration.AttributeLists;
        }

        public override void Compile(CsWalker cc)
        {
            cc.CompileTypeDeclaration(this.Declaration as StructDeclarationSyntax);
        }
    }

    class MemberClassModel : MemberModel<ClassDeclarationSyntax>
    {
        public override int Rank2
        {
            get { return 53; }
        }

        public ClassModel ClassModel { get; private set; }

		public MemberClassModel(TypeModel tm, ClassDeclarationSyntax declaration)
            : base(tm, declaration, declaration.Modifiers)
        {
			this.ClassModel = (ClassModel)TypeModel.Create(null, declaration, null, this.TypeModel.SemanticModel);
            this.Name = declaration.Identifier.Text;
            this.Attributes = declaration.AttributeLists;
        }

        public override void Compile(CsWalker cc)
        {
            cc.CompileTypeDeclaration(this.Declaration as ClassDeclarationSyntax);
        }
    }

    class MemberInterfaceModel : MemberModel<InterfaceDeclarationSyntax>
    {
        public override int Rank2
        {
            get { return 54; }
        }

        public InterfaceModel InterfaceModel { get; private set; }

		public MemberInterfaceModel(TypeModel tm, InterfaceDeclarationSyntax declaration)
            : base(tm, declaration, declaration.Modifiers)
        {
			this.InterfaceModel = (InterfaceModel)TypeModel.Create(null, declaration, null, this.TypeModel.SemanticModel);
            this.Name = declaration.Identifier.Text;
            this.Attributes = declaration.AttributeLists;
        }

        public override void Compile(CsWalker cc)
        {
            cc.CompileInterfaceDeclaration(this.Declaration as InterfaceDeclarationSyntax);
        }
    }

    #endregion
}
