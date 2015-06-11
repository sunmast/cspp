using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyCspp.Compiler
{
    static class Consts
    {
        public static SortedDictionary<string, string> PrimitiveDatatypes = new SortedDictionary<string, string>()
        {
            {"bool", "System.Boolean"},
            {"byte", "System.Byte"},
            {"sbyte", "System.SByte"},
            {"char", "System.Char"},
            {"decimal", "System.Decimal"},
            {"double", "System.Double"},
            {"float", "System.Single"},
            {"int", "System.Int32"},
            {"uint", "System.UInt32"},
            {"long", "System.Int64"},
            {"ulong", "System.UInt64"},
            {"object", "System.Object"},
            {"short", "System.Int16"},
            {"ushort", "System.UInt16"},
            {"string", "System.String"},
            {"void", "System.Void"},
        };

        public static string SharedPtr = "_";

        public static string WeakPtr = "sys::wptr";

        public static string FuncPtr = "sys::fptr";

        public static string Array = "sys::array";

        public static string GetterPrefix = "get_";

        public static string SetterPrefix = "set_";

        public static string IndexOf = "IndexOf";

        public static string PropStoragePrefix = "prop_";
    }
}
