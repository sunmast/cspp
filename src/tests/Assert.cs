using System;

namespace HappyCspp.Tests
{
    public static class Assert
    {
        static std.Vector<string> stack = new std.Vector<string>();

        public static string ImCalled(xstring place)
        {
            string ret = place.CStr;
            stack.PushBack(ret);

            return ret;
        }

        public static void Reset()
        {
            stack.Clear();
        }

        public static void IsTrue(bool condition, xstring message)
        {
            if (!condition)
            {
                Util.WriteLine(message);
            }
        }

        public static void IsFalse(bool condition, xstring message)
        {
            if (condition)
            {
                Util.WriteLine(message);
            }
        }

        public static void Equals(xint expected, xint actual, xstring message)
        {
            if (expected != actual)
            {
                Util.WriteLine(message);
            }
        }
    }
}
