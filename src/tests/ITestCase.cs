using System;

namespace HappyCspp.Tests
{
    public interface ITestCase
    {
        string Name { get; }
        string Description { get; }
        int Priority { get; }
        bool Run();
    }
}

