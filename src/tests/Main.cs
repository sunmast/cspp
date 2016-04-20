namespace HappyCspp.Tests
{
    using System;

    class MainClass
    {
        static int Main(string[] args)
        {
            Util.WriteLine("CS++ Test Suite v0.1");

            ITestCase[] tests = {
                new SyntaxTests()
            };

            foreach (var test in tests)
            {
                std.IO.PrintF("=== test started: %s ===\n", test.Name.CStr);
                test.Run();
                std.IO.PrintF("=== test completed: %s ===\n\n", test.Name.CStr);
            }

            return 0;
        }
    }
}
