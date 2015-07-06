using System;

namespace HappyCspp.Tests
{
    public static class Util
    {
        public static void WriteLine(string str)
        {
            std.IO.PrintF(str);
            std.IO.PrintF(Environment.NewLine);
        }
    }
}

