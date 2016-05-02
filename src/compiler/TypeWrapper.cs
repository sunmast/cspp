using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HappyCspp.Compiler
{
    public class TypeWrapper
    {
        private SemanticModel semantic;

        public TypeWrapper(SemanticModel semantic)
        {
            this.semantic = semantic;
        }

        public string Wrap(TypeSyntax typeSyntax, bool wrapSptr, IEnumerable<UsingDirectiveSyntax> usings = null)
        {
            if (typeSyntax == null)
                return null;

            TypeInfo typeInfo = this.semantic.GetTypeInfo(typeSyntax);
            return this.Wrap(typeInfo, wrapSptr, usings);
        }

        public string Wrap(TypeInfo typeInfo, bool wrapSptr, IEnumerable<UsingDirectiveSyntax> usings = null)
        {
            return this.Wrap(typeInfo.Type, wrapSptr, usings);
        }

        public string Wrap(ITypeSymbol typeSymbol, bool wrapSptr, IEnumerable<UsingDirectiveSyntax> usings = null)
        {
            // Wrap ref type with _<> if wrapSptr
            // Change type name to its alias

            if (typeSymbol is IArrayTypeSymbol)
            {
                return this.WrapArrayType(typeSymbol as IArrayTypeSymbol, wrapSptr, usings);
            }
            else
            {
                return this.WrapNamedType(typeSymbol as INamedTypeSymbol, wrapSptr, usings);
            }
        }

        private string WrapArrayType(IArrayTypeSymbol typeInfo, bool wrapSptr, IEnumerable<UsingDirectiveSyntax> usings = null)
        {
            string type;
            if (typeInfo.ElementType is IArrayTypeSymbol)
            {
                type = this.WrapArrayType(typeInfo.ElementType as IArrayTypeSymbol, wrapSptr, usings);
            }
            else
            {
                type = this.WrapNamedType(typeInfo.ElementType as INamedTypeSymbol, wrapSptr, usings);
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

        private string WrapNamedType(INamedTypeSymbol namedType, bool wrapSptr, IEnumerable<UsingDirectiveSyntax> usings = null)
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

            string containingType = null;

            if (!string.IsNullOrEmpty(namedType.ContainingSymbol.Name)) // global namespace as containing symbol is not counted
            {
                if (namedType.ContainingSymbol is INamedTypeSymbol)
                {
                    containingType = this.WrapNamedType(namedType.ContainingType, false, usings);
                }
                else
                {
                    containingType = namedType.ContainingSymbol.ToString().Replace(".", "::");
                }
            }

            string name = Util.GetSymbolAlias(namedType.GetAttributes()) ?? namedType.Name;

            // TODO: lookup usings and shorter name
            string ret = containingType == null ? name : (containingType + "::" + name);

            List<string> typeArgs = new List<string>();
            foreach (var typeArg in namedType.TypeArguments)
            {
                typeArgs.Add(this.Wrap(typeArg, true, usings));
            }

            if (typeArgs.Count > 0)
            {
                ret += string.Format("<{0}>", string.Join(", ", typeArgs));
            }

            if (wrapSptr && !namedType.IsValueType)
            {
                return "_<" + ret + ">";
            }
            else
            {
                return ret;
            }
        }
    }
}
