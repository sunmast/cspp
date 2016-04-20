using System;

namespace HappyCspp.Tests
{
    public static class Assert
    {
        static std.Vector<string> stack = new std.Vector<string>();

        public static string ImCalled(string place)
        {
            stack.PushBack(place);
            return place;
        }

        public static void Reset()
        {
            stack.Clear();
        }

        public static void IsTrue(bool condition, string message)
        {
            if (!condition)
            {
                Util.WriteLine(message);
            }
        }

        public static void IsFalse(bool condition, string message)
        {
            if (condition)
            {
                Util.WriteLine(message);
            }
        }

        public static void Equals(xint expected, xint actual, string message)
        {
            if (expected != actual)
            {
                Util.WriteLine(message);
            }
        }
    }
}

