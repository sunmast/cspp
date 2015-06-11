using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace HappyCspp.Compiler
{
	static class Util
	{
		internal static Exception NewSyntaxNotSupportedException<T> (SeparatedSyntaxList<T> syntaxList)
            where T : SyntaxNode
		{
			return new NotSupportedException ();
		}

		internal static Exception NewSyntaxNotSupportedException (SyntaxToken syntaxToken)
		{
			return new NotSupportedException ();
		}

		internal static Exception NewSyntaxNotSupportedException (SyntaxNode syntaxNode)
		{
			return new NotSupportedException ();
		}

		internal static Exception NewTypeNotFoundException (string typeName)
		{
			return new System.IO.FileNotFoundException ();
		}

		internal static Exception NewTokenNotFoundException (string tokenName)
		{
			return new System.IO.FileNotFoundException ();
		}

		internal static string RemoveLastChar (this string str)
		{
			return str.Substring (0, str.Length - 1);
		}

		internal static T GetAttributeValue<T> (SeparatedSyntaxList<AttributeArgumentSyntax> attrArgs, int index)
		{
			var expr = attrArgs [index].Expression;

			if (expr is LiteralExpressionSyntax) {
				var lt = attrArgs [index].Expression as LiteralExpressionSyntax;
				return (T)lt.Token.Value;
			} else if (expr is TypeOfExpressionSyntax) {
				var t = expr as TypeOfExpressionSyntax;
				return (T)(object)t.Type;
			} else {
				// Only literals are supported here
				throw Util.NewSyntaxNotSupportedException (attrArgs [index]);
			}

		}

		internal static string GetAliasOrBuiltInType (SeparatedSyntaxList<AttributeArgumentSyntax> attrArgs)
		{
			if (App.PreferWideChar && attrArgs.Count > 1) {
				return GetAttributeValue<string> (attrArgs, 1);
			} else {
				return GetAttributeValue<string> (attrArgs, 0);
			}
		}

		internal static string GetSymbolAlias(ImmutableArray<AttributeData> attributes)
		{
			foreach (var attr in attributes)
			{
				if (attr.AttributeClass.Name != "AliasAttribute")
					continue;

				var aliases = attr.ConstructorArguments.ToArray();
				if (App.PreferWideChar && aliases[1].Value != null)
				{
					return (string)aliases[1].Value;
				}
				else
				{
					return (string)aliases[0].Value;
				}
			}

			return null;
		}


        internal static bool IsAttributeDefined(IEnumerable<AttributeData> attributes, string attributeName)
        {
            foreach (var attr in attributes)
            {
                if (attr.AttributeClass.Name == attributeName)
                    return true;
            }

            return false;
        }

        internal static bool IsAttributeDefined(IEnumerable<AttributeListSyntax> attributes, string attributeShortName)
        {
            foreach (var attrList in attributes)
            {
                foreach (var attr in attrList.Attributes)
                {
                    string name = attr.Name.ToString();
                    if (name == attributeShortName || name == attributeShortName + "Attribute")
                    {
                        return true;
                    }
                }
            }

            return false;
        }
	}
}
