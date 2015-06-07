namespace HappyCspp.Tests
{
    using std;

    class TestMain
    {
        static int Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                StdIO.PrintF(args[i] += " ");
            }

            return 0;
        }
    }
}
