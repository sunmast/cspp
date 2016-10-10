namespace System
{
    /// <summary>
    /// A mutable string
    /// </summary>
    [Imported]
    public class String
    {
        public String(string str) { }

        [Alias("c_str")]
        public extern cstring CStr { get; }

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

/// <summary>
/// A constant string
/// </summary>
[Imported, Alias("cstring", "wcstring")]
public struct cstring
{
    public static extern implicit operator string(cstring value);
    public static extern implicit operator xstring(cstring value);
    public static extern implicit operator cstring(string value);
}

/// <summary>
/// A mutable or constant string
/// This should only be used as a parameter type to allow passing different types of strings to a function
/// </summary>
[Imported, Alias("xstring", "wxstring")]
public struct xstring
{
    public static extern implicit operator xstring(cstring value);
    public static extern implicit operator xstring(string value);

    [Alias("c_str")]
    public extern cstring CStr { get; }
}