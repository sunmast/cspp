namespace HappyCspp.Tests
{
    using System;

    class Program
    {
        static void Main()
        {
            // Need to tune C# compiler to support Main function with cstring[] instead of string[]
        }

        public static int Run(cstring[] args)
        {
            fmt.Print("CS++ Test Suite v0.1\n");

            ITestCase[] tests = {
                new SyntaxTests()
            };

            foreach (var test in tests)
            {
                fmt.Print("=== test started: {0} ===\n", test.Name);
                test.Run();
                fmt.Print("=== test completed: {0} ===\n\n", test.Name);
            }

            C.Debug.Assert(true);
            return 0;
        }
    }
}
