namespace HappyCspp.Tests
{
    using System;

    class Program
    {
        delegate string TestDelegate(int input);

        static TestDelegate Test;

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

            Test = DelegateTest;
            Program.Test(23);

            C.Debug.Assert(true);

            fmt.Print(C.Clock.GetString(C.Clock.GetTime()));
            return 0;
        }

        static string DelegateTest(int input)
        {
            fmt.Print("Input: {0}\n", input);
            return null;
        }
    }
}
