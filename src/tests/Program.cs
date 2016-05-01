namespace HappyCspp.Tests
{
    using System;

    class Program
    {
        static void Main()
        {
            // Need to tune C# compiler to support Main function with CString[] instead of string[]
        }

        public static int Run(cstring[] args)
        {
            fmt.Print("CS++ Test Suite v0.1\n");

            ITestCase[] tests = {
                new SyntaxTests()
            };

            foreach (var test in tests)
            {
                std.Vector<int> v = new std.Vector<int>();
                fmt.Print("=== test started: {0} ===\n", test.Name);
                test.Run();
                fmt.Print("=== test completed: {0} ===\n\n", test.Name);
            }

            return 0;
        }
    }
}
