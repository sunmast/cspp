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
            if (type == null)
                return null;

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
            if (!string.IsNullOrEmpty(typeParameter.VarianceKeyword.Text))
            {
                throw Util.NewSyntaxNotSupportedException(typeParameter);
            }

            return typeParameter.Identifier.Text;
        }

        private string SyntaxArrayType(ArrayTypeSyntax arrayType)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < arrayType.RankSpecifiers.Count; i++)
            {
                sb.AppendFormat(i == 0 ? "{0}<" : "_{0}<", Consts.Array);
                if (i == arrayType.RankSpecifiers.Count - 1)
                {
                    sb.Append(typeWrapper.Wrap(arrayType.ElementType, true));
                }
            }

            for (int i = arrayType.RankSpecifiers.Count - 1; i >= 0; i--)
            {
                var rank = arrayType.RankSpecifiers[i];
                sb.Append(rank.Sizes.Count == 1 ? ">" : string.Format(", {0}>", rank.Sizes.Count));
            }

            return sb.ToString();
        }

        private string SyntaxTypeParameterList(TypeParameterListSyntax typeParameterList)
        {
            if (typeParameterList == null)
                return null;

            List<string> list = new List<string>(typeParameterList.Parameters.Count);

            foreach (var type in typeParameterList.Parameters)
            {
                list.Add(this.SyntaxTypeParameter(type));
            }

            return string.Format("<{0}>", string.Join(", ", list));
        }

        private string SyntaxBracketedParameterList(BracketedParameterListSyntax parameterList, bool forHeader)
        {
            if (parameterList == null)
                return null;

            List<string> list = new List<string>(parameterList.Parameters.Count);

            foreach (var type in parameterList.Parameters)
            {
                list.Add(this.SyntaxParameter(type, forHeader));
            }

            return string.Join(", ", list);
        }

        private string SyntaxTypeArgumentList(TypeArgumentListSyntax typeArgumentList)
        {
            if (typeArgumentList == null)
                return null;

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

            if (parameter.Modifiers.Count > 0)
            {
                foreach (var m in parameter.Modifiers)
                {
                    if (m.Text == "params" && forHeader && defaultClause == null)
                    {
                        // give params x[] a default value null
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

            TypeInfo paramType = this.semantic.GetTypeInfo(parameter.Type);
            if (paramType.Type.IsReferenceType)
            {
                byRef = "&";
            }

            if (!forHeader)
            {
                this.memberModel.Variables.Push(new VariableModel(paramType, name, this.memberModel.ScopeDepth + 1));
            }

            string type = typeWrapper.Wrap(paramType, true);
            return type + byRef + " " + name + defaultClause;
        }

        private string SyntaxTemplateTypeDeclaration(TypeParameterListSyntax typeArgs)
        {
            if (typeArgs == null)
                return null;

            List<string> list = new List<string>();
            foreach (var typeArg in typeArgs.Parameters)
            {
                list.Add(typeArg.Identifier.Text);
            }

            return string.Format("template<typename {0}>", string.Join(", ", list));
        }

        private string SyntaxBaseList(BaseListSyntax baseList, TypeParameterListSyntax typeArgs)
        {
            if (baseList == null)
                return null;

            List<string> list = new List<string>();
            foreach (var baseType in baseList.Types)
            {
                string baseName = typeWrapper.Wrap(baseType.Type, false);
                list.Add("public " + baseName);
            }

            return " : " + string.Join(", ", list);
        }

        private string SyntaxSingleLineVariableDeclaration(VariableDeclarationSyntax variable, TypeInfo t)
        {
            // Only used in for loop variables declaration which must be in single line
            // And thus its for impl, not for header

            List<string> list = new List<string>();

            foreach (var variableDeclarator in variable.Variables)
            {
                list.Add(this.SyntaxVariableDeclarator(variableDeclarator, false, false, false));
            }

            return string.Format("{0} {1}", typeWrapper.Wrap(t, true), string.Join(", ", list));
        }

        private string SyntaxVariableDeclaration(VariableDeclarationSyntax variableDeclaration, VariableDeclaratorSyntax variableDeclarator, bool forMember, bool forHeader, bool ignoreInitializer)
        {
            return typeWrapper.Wrap(variableDeclaration.Type, true) + " " + this.SyntaxVariableDeclarator(variableDeclarator, forMember, forHeader, ignoreInitializer);
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
            if (equalsValueClause == null)
                return null;

            string right = this.ExprSyntax(equalsValueClause.Value);

            return " = " + right;
        }

        private string SyntaxModifiers(SyntaxTokenList modifiers)
        {
            if (modifiers.Count == 0)
                return null;

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
            if (separatedSyntaxList.Count == 0)
                return null;

            List<string> list = new List<string>();
            foreach (var syntax in separatedSyntaxList)
            {
                list.Add(func(syntax));
            }

            return string.Join(", ", list);
        }

        private string SyntaxRtArgumentList(ArgumentListSyntax argumentList, List<TypeInfo> typeParameterList)
        {
            if (argumentList == null || argumentList.Arguments.Count == 0)
                return null;

            var typeParameterEnum = typeParameterList.GetEnumerator();

            List<string> list = new List<string>();
            foreach (var arg in argumentList.Arguments)
            {
                typeParameterEnum.MoveNext();
                list.Add(this.SyntaxRtArgument(arg,typeParameterEnum.Current));
            }

            return list.Count > 0 ? string.Join(", ", list) : null;
        }

        private string SyntaxRtArgument(ArgumentSyntax arg, TypeInfo typeParameter)
        {
            if (arg.NameColon != null)
                throw Util.NewSyntaxNotSupportedException(arg.NameColon);

            string modifier = null;

            if (!string.IsNullOrEmpty(arg.RefOrOutKeyword.Text))
            {
                // Passing parameter by reference
                modifier = "&";
            }

            if (typeParameter.Type != null)
            {
                foreach (var attr in typeParameter.Type.GetAttributes())
                {
                    string name = attr.AttributeClass.Name;
                    if (name == "Param" || name == "ParamAttribute")
                    {
                        var paramType = attr.ConstructorArguments.First().Value;
                    }
                }
            }

            return modifier + this.ExprSyntax(arg.Expression);
        }
    }
}
