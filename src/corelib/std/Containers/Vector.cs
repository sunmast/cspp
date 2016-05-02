using System;

namespace std
{
    [Imported, Alias("vector")]
    public class Vector<T>
    {
        public Vector() { }

        public Vector(xint count) { }

        public Vector(xint count, T val) { }

        // Iterators:

        [Alias("begin")]
        public extern Iterator Begin { get; }

        [Alias("end")]
        public extern Iterator End { get; }

        [Alias("rbegin")]
        public extern ReverseIterator RBegin { get; }

        [Alias("rend")]
        public extern ReverseIterator REnd { get; }

        // Capacity:

        [Alias("size")]
        public extern xint Size { get; }

        [Alias("max_size")]
        public extern xint MaxSize { get; }

        [Alias("capacity")]
        public extern xint Capacity { get; }

        [Alias("empty")]
        public extern bool IsEmpty { get; }

        [Alias("resize")]
        public extern void Resize(xint n);

        [Alias("resize")]
        public extern void Resize(xint n, T val);

        [Alias("reserve")]
        public extern void Reserve(xint n);

        [Alias("shrink_to_fit")]
        public extern void ShrinkToFit();

        // Element access:

        public extern T this[xint n] { get; set; }

        [Alias("front")]
        public extern T Front { get; set; }

        [Alias("back")]
        public extern T Back { get; set; }

        // Modifiers:

        [Alias("assign")]
        public extern void Assign(xint n, T val);

        [Alias("push_back")]
        public extern void PushBack(T val);

        [Alias("pop_back")]
        public extern void PopBack();

        [Alias("insert")]
        public extern Iterator Insert(Iterator position, T val);

        [Alias("insert")]
        public extern Iterator Insert(Iterator position, xint n, T val);

        [Alias("erase")]
        public extern Iterator Erase(Iterator position);

        [Alias("erase")]
        public extern Iterator Erase(Iterator first, Iterator last);

        [Alias("swap")]
        public extern void Swap(Vector<T> x);

        [Alias("clear")]
        public extern void Clear();

        [Imported, Alias("iterator")]
        public enum Iterator : int
        {
        }

        [Imported, Alias("reverse_iterator")]
        public enum ReverseIterator : int
        {
        }
    }
}
