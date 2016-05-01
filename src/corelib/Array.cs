using System;

namespace System
{   
    [Imported]
    public class Array // For C# compilation
    {
        //public extern T this[uint index] { get; set; } // this indexer will be assumed by C# compiler by default

        public extern xint Length { get; }

        [Alias("init")]
        public extern void Init<T>(T[] values);
    }

    [Imported, BuiltInType]
    public class GenericArray<T> // For cs2cpp compilation
    {
        public extern T this[xint index] { get; set; }

        public extern xint Length { get; }

        [Alias("init")]
        public extern void Init(T[] values);
    }
}
