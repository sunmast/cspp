cs2cpp
======

Code C++ with C\# *productivity*! Run C\# with C++ *performance*!

Status Update (June 7, 2015)
----------------------------

-   Initial source code published: compiler, corelib, includes, and helloworld
    sample

-   helloworld running on Windows (VC++/g++ compiler with -std=c++11)

Next Update
-----------

-   Unit tests

-   Port to Linux & Mac OS

-   Version 0.1 release

Short Term Goals
----------------

-   Stabilization

-   Import C/C++ standard libraries

-   Import Boost libraries

-   Version 1.0 release

Secondary Goals
---------------

-   Support advanced C\# features like yield, await, LINQ, etc.

-   Compile Roslyn source code

-   Compile MonoDevelop source code

-   TBD

Memory Management (to be continued)
-----------------------------------

In short, its based on an improved reference counter algorithm in both compile
time and runtime.

Hello World
-----------

C\# code:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
namespace HappyCspp.Tests
{
    class TestMain
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

        [Alias("getchar")]
        public static extern int GetChar();
    }
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Generated C++ code:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
#include "tests.h"

using namespace std;

namespace HappyCspp { namespace Tests {

    int32_t TestMain::Main(_array<_string> args) {
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
    return HappyCspp::Tests::TestMain::Main(args);
}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Hello World!

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
tests.exe Hello World!
Hello World! 
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
