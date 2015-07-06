using System;

namespace HappyCspp.Tests
{
    public static class Test
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
    }
}

