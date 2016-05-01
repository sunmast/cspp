using System;

namespace HappyCspp.Tests
{
    public interface ITestCase
    {
        cstring Name { get; }
        cstring Description { get; }
        int Priority { get; }
        bool Run();
    }
}

