using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;

namespace HappyCspp.Compiler
{
    partial class CsWalker
    {
        private string ExprInvocationSyntax(InvocationExpressionSyntax invocationExpression, out TypeInfo exprType)
        {
            // E.g. X(y, z)
            Debug.Assert(this.currentParameterListTypes.Count == 0);
            Debug.Assert(this.currentParameterListTypesDeclaration.Count == 0);

            foreach (var arg in invocationExpression.ArgumentList.Arguments)
            {
                // Named argument is not supported
                if (arg.NameColon != null)
                {
                    throw Util.NewSyntaxNotSupportedException(arg);
                }

                this.currentParameterListTypes.Add(this.semantic.GetTypeInfo(arg));
            }

            // this.currentParameterListTypesDeclaration should be populated in ExprSyntax
            string left = this.ExprSyntax(invocationExpression.Expression);
            string right = this.SyntaxRtArgumentList(invocationExpression.ArgumentList, this.currentParameterListTypesDeclaration);

            // Get the returning type of the invocation
            //exprType = null;

            this.currentParameterListTypes.Clear();
            this.currentParameterListTypesDeclaration.Clear();

            return string.Format("{0}({1})",
                left,
                right);
        }

        private string ExprMemberAccessSyntax(MemberAccessExpressionSyntax memberAccessExpression, bool isLeftValue, out TypeInfo exprType)
        {
            // E.g. this.foo(); variableA.memberB.memberC; classA.staticMemberB; enumType.enumValue
            if (memberAccessExpression.Expression is LiteralExpressionSyntax)
            {
                // E.g. "foo".X ; 5.ToString
                throw Util.NewSyntaxNotSupportedException(memberAccessExpression);
            }

            ExpressionSyntax leftExpr = memberAccessExpression.Expression;
            TypeInfo leftType;
            string left = this.ExprSyntax(leftExpr, out leftType);
            string op;

            if (left == "this")
            {
                op = "->";
            }
            else if (leftType.Type == null)
            {
                op = "::";
            }
            else
            {
                op = leftType.Type.IsValueType || leftType.Type.Name == "String" ? "." : "->";
            }

            string right = memberAccessExpression.Name.Identifier.Text;
            ImmutableArray<ISymbol> members = ImmutableArray<ISymbol>.Empty;

            if (leftType.Type == null)
            {
                // Left is NS, right must be NS or type
                exprType = this.semantic.GetTypeInfo(memberAccessExpression);
                if (exprType.Type != null)
                {
                    // It's a type
                    return typeWrapper.Wrap(exprType, false);
                }
                else
                {
                    // It's a NS
                    return left + "::" + right;
                }
            }
            else if (leftType.Type is IArrayTypeSymbol)
            {
                members = App.ArrayType.GetMembers(right);
            }
            else if (leftType.Type is INamespaceOrTypeSymbol)
            {
                members = (leftType.Type as INamespaceOrTypeSymbol).GetMembers(right);
            }
            else
            {
                Util.NewSyntaxNotSupportedException(memberAccessExpression);
            }

            bool isStatic;
            string alias;

            ITypeSymbol typeSymbol;
            IFieldSymbol fieldSymbol;
            IPropertySymbol propertySymbol;
            IMethodSymbol methodSymbol;

            this.ResolveMember(
                right, members, this.currentParameterListTypes,
                out isStatic, out alias,
                out typeSymbol, out fieldSymbol, out propertySymbol, out methodSymbol);

            if (isStatic)
            {
                op = "::";
            }

            // Properties: compile to get_X / set_X
            // Members with aliases: compiled to alias
            // Properties with alias: compiled to alias as-is. No get_/set_ prefixes
            if(typeSymbol != null)
            {
                right = typeWrapper.Wrap(typeSymbol, false);
            }
            else if(fieldSymbol != null)
            {
                right = alias ?? fieldSymbol.Name;
            }
            else if(propertySymbol != null)
            {
                if (!isLeftValue || this.memberAccessDepth > 1)
                {
                    right = (alias ?? Consts.GetterPrefix + propertySymbol.Name) + "()";
                }
                else
                {
                    right = (alias ?? Consts.SetterPrefix + propertySymbol.Name) + "("; // x.Prop = Y; will be compiled to x.set_Prop(Y);
                }
            }
            else if(methodSymbol != null)
            {
                right = alias ?? methodSymbol.Name;
            }
            else
            {
                throw Util.NewSyntaxNotSupportedException(memberAccessExpression);
            }

            exprType = this.semantic.GetTypeInfo(memberAccessExpression.Name);
            return string.IsNullOrEmpty(left) ? right : (left + op + right);
        }

        internal void ResolveMember(
            string name, IEnumerable<ISymbol> members, IList<TypeInfo> parameterTypes,
            out bool isStatic, out string alias,
            out ITypeSymbol typeSymbol, out IFieldSymbol fieldSymbol, out IPropertySymbol propertySymbol, out IMethodSymbol methodSymbol)
        {
            isStatic = false;
            alias = null;
            typeSymbol = null;
            fieldSymbol = null;
            propertySymbol = null;
            methodSymbol = null;

            foreach (var member in members.Where(x => x.Name == name))
            {
                alias = Util.GetSymbolAlias(config.PreferWideChar, member.GetAttributes());
                isStatic = member.IsStatic;

                // Properties: compile to get_X / set_X
                // Members with aliases: compiled to alias
                // Properties with alias: compiled to alias as-is. No get_/set_ prefixes
                if (member is ITypeSymbol)
                {
                    typeSymbol = member as ITypeSymbol;
                }
                else if (member is IFieldSymbol)
                {
                    fieldSymbol = member as IFieldSymbol;
                }
                else if (member is IPropertySymbol)
                {
                    propertySymbol = member as IPropertySymbol;
                }
                else if (member is IMethodSymbol)
                {
                    methodSymbol = member as IMethodSymbol;

                    if (this.AreParametersMatch(parameterTypes, methodSymbol.Parameters))
                    {
                        return;
                    }
                }
            }
        }

        private bool AreParametersMatch(IList<TypeInfo> parameterTypes, IList<IParameterSymbol> parameterDeclations)
        {
            if (parameterDeclations.Count < parameterTypes.Count)
            {
                return false;
            }

            int i = 0;

            for (; i < parameterTypes.Count; i++)
            {
                ITypeSymbol parameterType = parameterTypes[i].Type;
                ITypeSymbol parameterDeclationType = parameterDeclations[i].Type;
                bool isMatch = false;

                while (parameterType != null)
                {
                    if (parameterType.Equals(parameterDeclationType))
                    {
                        isMatch = true;
                        break;
                    }
                    else
                    {
                        parameterType = parameterType.BaseType;
                    }
                }

                if (!isMatch)
                {
                    return false;
                }
            }

            for (; i < parameterDeclations.Count; i++)
            {
                IParameterSymbol parameterDeclation = parameterDeclations[i];
                if (!parameterDeclation.HasExplicitDefaultValue)
                {
                    return false;
                }
            }

            return true;
        }
    }
}