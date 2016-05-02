using System;

namespace System
{   
    [Imported]
    public class Array // For C# compilation
    {
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
