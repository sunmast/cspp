using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HappyCspp.Compiler
{
    partial class CsWalker
    {
        private string SyntaxName(NameSyntax name)
        {
            if (name is AliasQualifiedNameSyntax)
            {
                throw Util.NewSyntaxNotSupportedException(name);
            }
            else if (name is QualifiedNameSyntax)
            {
                return this.SyntaxQualifiedName(name as QualifiedNameSyntax);
            }
            else if (name is GenericNameSyntax)
            {
                return this.SyntaxGenericName(name as GenericNameSyntax);
            }
            else if (name is IdentifierNameSyntax)
            {
                return this.SyntaxIdentifierName(name as IdentifierNameSyntax);
            }
            else
            {
                throw Util.NewSyntaxNotSupportedException(name);
            }
        }

        private string SyntaxQualifiedName(QualifiedNameSyntax qualifiedName)
        {
            return this.SyntaxName(qualifiedName.Left) + "::" + this.SyntaxSimpleName(qualifiedName.Right);
        }

        private string SyntaxSimpleName(SimpleNameSyntax simpleName)
        {
            if (simpleName is GenericNameSyntax)
            {
                return this.SyntaxGenericName(simpleName as GenericNameSyntax);
            }
            else if (simpleName is IdentifierNameSyntax)
            {
                return this.SyntaxIdentifierName(simpleName as IdentifierNameSyntax);
            }
            else
            {
                throw Util.NewSyntaxNotSupportedException(simpleName);
            }
        }

        private string SyntaxGenericName(GenericNameSyntax genericName)
        {
            if (genericName.IsUnboundGenericName)
            {
                throw Util.NewSyntaxNotSupportedException(genericName);
            }

            return genericName.Identifier.Text + this.SyntaxTypeArgumentList(genericName.TypeArgumentList);
        }

        private string SyntaxIdentifierName(IdentifierNameSyntax identifierName)
        {
            return identifierName.Identifier.Text;
        }

        private string SyntaxType(TypeSyntax type)
        {
            if (type == null) return null;

            if (type.IsVar)
            {
                return "auto";
            }
            else if (type is NameSyntax)
            {
                return this.SyntaxName(type as NameSyntax);
            }
            else if (type is PredefinedTypeSyntax)
            {
				return type.ToString();
            }
            else if (type is ArrayTypeSyntax)
            {
                return this.SyntaxArrayType(type as ArrayTypeSyntax);
            }
            else if (type is PointerTypeSyntax)
            {
                throw Util.NewSyntaxNotSupportedException(type);
            }
            else if (type is NullableTypeSyntax)
            {
                throw Util.NewSyntaxNotSupportedException(type);
            }
            else if (type is OmittedTypeArgumentSyntax)
            {
                throw Util.NewSyntaxNotSupportedException(type);
            }
            else
            {
                throw Util.NewSyntaxNotSupportedException(type);
            }
        }

        private string SyntaxTypeParameter(TypeParameterSyntax typeParameter)
        {
            if (typeParameter.VarianceKeyword != null && !string.IsNullOrEmpty(typeParameter.VarianceKeyword.Text))
            {
                throw Util.NewSyntaxNotSupportedException(typeParameter);
            }

            return typeParameter.Identifier.Text;
        }

        private string SyntaxArrayType(ArrayTypeSyntax arrayType, int omittedSize = -1)
        {
            StringBuilder sb = new StringBuilder();
            int ranks = arrayType.RankSpecifiers.Count;
            List<string> sizes = new List<string>();

            foreach (var rank in arrayType.RankSpecifiers)
            {
                List<string> list = new List<string>();
                foreach(var sizeExpr in rank.Sizes)
                {
                    string size = this.ExprSyntax(sizeExpr, out dummyType);

                    if (size != null)
                    {
                        list.Add(size);
                    }
                }

                sizes.Add(list.Count == 0 ? null : string.Join(" * ", list));

                sb.AppendFormat("{0}<", Consts.Array);
            }

            sb.Append(this.SyntaxType(arrayType.ElementType));

            for (int i = sizes.Count - 1; i >= 0; i--)
            {
                string size = sizes[i];
                if (size == null)
                {
                    sb.Append('>');
                }
                else
                {
                    sb.AppendFormat(">", size);
                }
            }

            if (omittedSize >= 0)
            {
                sb.Length--;
                sb.AppendFormat(", {0}>", omittedSize);
            }

            return sb.ToString();
        }

        private string SyntaxTypeParameterList(TypeParameterListSyntax typeParameterList)
        {
            if (typeParameterList == null) return null;

            List<string> list = new List<string>(typeParameterList.Parameters.Count);

            foreach (var type in typeParameterList.Parameters)
            {
                list.Add(this.SyntaxTypeParameter(type));
            }

            return string.Format("<{0}>", string.Join(", ", list));
        }

        private string SyntaxBracketedParameterList(BracketedParameterListSyntax parameterList, bool forHeader)
        {
            if (parameterList == null) return null;

            List<string> list = new List<string>(parameterList.Parameters.Count);

            foreach (var type in parameterList.Parameters)
            {
                list.Add(this.SyntaxParameter(type, forHeader));
            }

            return string.Join(", ", list);
        }

        private string SyntaxTypeArgumentList(TypeArgumentListSyntax typeArgumentList)
        {
            if (typeArgumentList == null) return null;

            List<string> list = new List<string>(typeArgumentList.Arguments.Count);

            foreach (var type in typeArgumentList.Arguments)
            {
                list.Add(this.SyntaxType(type));
            }

            return string.Format("<{0}>", string.Join(", ", list));
        }

        private string SyntaxParameterList(ParameterListSyntax parameterList, bool forHeader)
        {
            if (parameterList == null)
                return null;
            
            List<string> list = new List<string>(parameterList.Parameters.Count);

            foreach (var param in parameterList.Parameters)
            {
                list.Add(this.SyntaxParameter(param, forHeader));
            }

            return string.Format("({0})", string.Join(", ", list));
        }

        private string SyntaxParameter(ParameterSyntax parameter, bool forHeader)
        {
            string name = parameter.Identifier.Text;
            string defaultClause = forHeader ? this.SyntaxEqualsValueClause(parameter.Default) : null;
            string byRef = null;

            if (parameter.Modifiers != null)
            {
                // give params x[] a default value null
                foreach (var m in parameter.Modifiers)
                {
                    if (m.Text == "params" && forHeader && defaultClause == null)
                    {
                        defaultClause = " = nullptr";
                        break;
                    }
                    else if (m.Text == "ref" || m.Text == "out")
                    {
                        byRef = "&";
                        break;
                    }
                }
            }

            TypeInfo paramType = parameter.Type == null ? new TypeInfo() : this.semantic.GetTypeInfo(parameter.Type);// ?? this.semantic.GetTypeInfo(parameter);

            if (!forHeader)
            {
                this.memberModel.Variables.Push(new VariableModel(paramType, name, this.memberModel.ScopeDepth + 1));
            }

            string type = paramType.Type == null ? "auto" : this.WrapTypeName(paramType);
            return type + byRef + " " + name + defaultClause;
        }

        private string SyntaxTemplateTypeDeclaration(TypeParameterListSyntax typeArgs)
        {
            if (typeArgs == null) return null;

            List<string> list = new List<string>();
            foreach (var typeArg in typeArgs.Parameters)
            {
                list.Add(typeArg.Identifier.Text);
            }

            return string.Format("template<typename {0}>", string.Join(", ", list));
        }

        private string SyntaxBaseList(BaseListSyntax baseList, TypeParameterListSyntax typeArgs)
        {
            if (baseList == null) return null;

            List<string> list = new List<string>();
            foreach (var baseType in baseList.Types)
            {
                string baseName = this.WrapTypeName(baseType.Type);

                // The wrapped name may contain ptr wrapper _<> for reference base types, but its not needed here
                baseName = baseName.StartsWith("_<") ? baseName.Substring(2, baseName.Length - 3) : baseName;

                list.Add("public " + baseName);
            }

            return " : " + string.Join(", ", list);
        }

        private string SyntaxSingleLineVariableDeclaration(VariableDeclarationSyntax variable)
        {
            // Only used in for loop variables declaration which must be in single line
            // And thus its for impl, not for header

            List<string> list = new List<string>();

            foreach (var variableDeclarator in variable.Variables)
            {
                list.Add(this.SyntaxVariableDeclarator(variableDeclarator, false, false, false));
            }

            return string.Format("{0} {1}", this.WrapTypeName(variable.Type), string.Join(", ", list));
        }

        private string SyntaxVariableDeclaration(VariableDeclarationSyntax variableDeclaration, VariableDeclaratorSyntax variableDeclarator, bool forMember, bool forHeader, bool ignoreInitializer)
        {
            return this.WrapTypeName(variableDeclaration.Type) + " " + this.SyntaxVariableDeclarator(variableDeclarator, forMember, forHeader, ignoreInitializer);
        }

        private string SyntaxVariableDeclarator(VariableDeclaratorSyntax variable, bool forMember, bool forHeader, bool ignoreInitializer)
        {
            if (variable.ArgumentList != null)
            {
                throw Util.NewSyntaxNotSupportedException(variable);
            }

            string initializer = ignoreInitializer ? null : this.SyntaxEqualsValueClause(variable.Initializer);

            if (forHeader)
            {
                return variable.Identifier.Text + initializer;
            }
            else if (forMember)
            {
                return this.typeModel.Name + "::" + variable.Identifier.Text + initializer;
            }
            else
            {
                return variable.Identifier.Text + initializer;
            }
        }

        private string SyntaxEqualsValueClause(EqualsValueClauseSyntax equalsValueClause)
        {
            if (equalsValueClause == null) return null;

            string right = this.ExprSyntax(equalsValueClause.Value, out dummyType);
			if (equalsValueClause.Value is LiteralExpressionSyntax && !dummyType.Type.IsValueType)
            {
                right = string.Format("new{0}({1})",
                    this.WrapTypeName(dummyType),
                    right);
            }

            return " = " + right;
        }

        private string SyntaxModifiers(SyntaxTokenList modifiers)
        {
            if (modifiers == null) return null;

            StringBuilder sb = new StringBuilder();
            foreach (var modifier in modifiers)
            {
                sb.Append(modifier.Text);
                sb.Append(' ');
            }

            return sb.ToString();
        }

        private string SyntaxSeparatedSyntaxList<T>(SeparatedSyntaxList<T> separatedSyntaxList, Func<T, string> func)
            where T : SyntaxNode
        {
            if (separatedSyntaxList == null) return null;

            List<string> list = new List<string>();
            foreach (var syntax in separatedSyntaxList)
            {
                list.Add(func(syntax));
            }

            return string.Join(", ", list);
        }

        private string SyntaxArgumentList(ArgumentListSyntax argumentList)
        {
            if (argumentList == null || argumentList.Arguments == null) return null;

            List<string> list = new List<string>();
            foreach (var arg in argumentList.Arguments)
            {

                list.Add(this.SyntaxArgument(arg));
            }

            return list.Count > 0 ? string.Join(", ", list) : null;
        }

        private string SyntaxArgument(ArgumentSyntax arg)
        {
            if (arg.NameColon != null) throw Util.NewSyntaxNotSupportedException(arg.NameColon);

            if (arg.RefOrOutKeyword != null && arg.RefOrOutKeyword.Text != "")
            {
                // Passing parameter by reference, but C++ compiler will take care of it by looking parameter definition in the function
            }

            return this.ExprSyntax(arg.Expression, out dummyType);
        }
    }
}
