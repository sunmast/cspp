# cs2cpp
A source to source compiler compiles C# to C++.

It enables you to code C++ with C# *productivity*, and run C# with C++ *performance*!

## Status Update (May 22, 2015)
 - Handled most common C# declarations, statements, expressions
 - Type model recognized almost everywhere
 - HelloWorld running on Windows (VC++ compiler)

## Next Update
 - Unit tests
 - Publish code in GitHub

## Short Term Goals
 - Debug
 - Import C/C++ standard libraries
 - Import Boost library
 - Port to Linux & Mac OS

## Secondary Goals
 - Support advanced C# features like yield, await, LINQ, etc.
 - Compile Roslyn source code
 - Compile MonoDevelop source code
 - TBD

## Memory Management (to be continued)
In short, its based on an improved reference counter algorithm in both compile time and runtime.

## Hello World
C# code:
```
namespace Tests
{
    class Program
    {
        static int Main(string[] args)
        {
            C.PrintF("Hello %s", "World!");
            return 0;
        }
    }
}
```
Import C Standard Library:
```
using Std;

/// <summary>
/// Imported C Standard Library.
/// </summary>
/// <remarks>
/// No namespace.
/// </remarks>
[Imported, Alias("")]
public static struct C
{
    [Alias("printf"), DefinedIn("cstdio")]
    public static extern void PrintF(string format, params object[] args);
}
```
Generated C++ code:
```
#include "Tests.Program.h"


namespace Tests { 

	int Program::Main(_sptr_<_array_<_string_>> args) {
		printf("Hello %s", "World!");
		return 0;
	}
}

int main(int argc, _char_** argv) {
	return Tests::Program::Main(new _array_<_string_>(argc, argv));
}
```
