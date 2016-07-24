CS++
======

C\# *productivity* with C++ *performance!*


Hello World
-----------

C\# code:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
namespace HappyCspp.HelloWorld
{
    class MainClass
    {
        static int Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                std.IO.PrintF(args[i] += " ");
            }

            return 0;
        }
    }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Import some standard libraries:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
using System;

namespace std
{
    /// <summary>
    /// C library to perform Input/Output operations.
    /// </summary>
    /// <remarks>
    /// http://www.cplusplus.com/reference/cstdio/
    /// </remarks>
    [Imported, Alias("")]
    public struct IO
    {
        [Alias("printf", "wprintf")]
        public static extern int PrintF(string format, params object[] args);
    }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Generated C++ code:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
#include "tests.h"

namespace HappyCspp { namespace HelloWorld {

    int32_t MainClass::Main(_array<_string> args) {
        for (int32_t i = 0; i < args->get_Length(); i++) {
            std::printf(args.IndexOf<_string>(i) += " ");
        }
        return 0;
    }
}}

int main(int argc, char* argv[]) {
    _array<_string> args = new_<array<_string>>(argc - 1);
    for (int i = 1; i < argc; i++) {
        args.IndexOf<_string>(i - 1) = argv[i];
    }
    return HappyCspp::HelloWorld::MainClass::Main(args);
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Hello World!

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
tests.exe Hello World!
Hello World! 
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
