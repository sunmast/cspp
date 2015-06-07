using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyCspp.Compiler
{
    static class Logger
    {
        public static void LogWarning(string format, params object[] args)
        {
            Console.Write("[WARNING] ");
            Console.WriteLine(format, args);
        }
    }
}
