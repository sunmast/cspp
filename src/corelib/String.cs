namespace System
{
    [Imported, BuiltInType("sys::string", "sys::wstring")]
    public class String
    {
        public String(string str) { }

        [Alias("c_str")]
        public extern IntPtr CStr { get; }

        public extern char this [xint index] { get; set;}

        //public static extern string Concat(params string[] values);

        //public static extern string Concat(string str0, string str1, string str2, string str3);

        //public static extern string Concat(string str0, string str1, string str2);

        public static extern string Concat(string str0, string str1);

        //public static extern string Concat(params object[] args);

        //public static extern string Concat(object arg0, object arg1, object arg2);

        //public static extern string Concat(object arg0, object arg1);

        //public static extern string Concat(object arg0);

        public static extern bool operator ==(string x, string y);

        public static extern bool operator !=(string x, string y);
    }
}
