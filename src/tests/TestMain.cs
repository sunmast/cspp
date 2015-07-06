namespace HappyCspp.Tests
{
    using System;

    class TestMain
    {
        static int Main(string[] args)
        {
            Util.WriteLine("CS++ Test Suite v0.1");

            Core.Tests.Run();

            return 0;
        }
    }
}
