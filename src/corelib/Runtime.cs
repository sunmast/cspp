namespace System
{
    [Imported]
    public struct Void
    {

    }

    [Imported]
    public class Object
    {

    }

    [Imported]
    public struct Byte
    {

    }

    [Imported]
    public struct Int16
    {

    }

    [Imported]
    public struct Int32
    {

    }

    [Imported]
    public struct Int64
    {

    }

    [Imported]
    public struct Single
    {

    }

    [Imported]
    public struct Double
    {

    }

    [Imported]
    public struct Char
    {

    }

    [Imported]
    public struct Boolean
    {

    }

    [Imported]
    public struct SByte
    {

    }

    [Imported]
    public struct UInt16
    {

    }

    [Imported]
    public struct UInt32
    {

    }

    [Imported]
    public struct UInt64
    {

    }

    [Imported]
    public struct Decimal
    {

    }

    [Imported]
    public struct IntPtr
    {

    }

    [Imported]
    public struct UIntPtr
    {

    }

    [Imported]
    public class Delegate
    {

    }

    [Imported]
    public class MulticastDelegate
    {

    }

    [Imported]
    public class Exception
    {

    }

    [Imported]
    public class Type
    {

    }

    [Imported]
    public abstract class ValueType
    {

    }

    [Imported]
    public abstract class Enum : ValueType
    {

    }

    [Imported]
    public class Attribute
    {

    }

    [Imported]
    public class ParamArrayAttribute
    {

    }

    [Imported]
    public struct RuntimeTypeHandle
    {

    }

    [Imported]
    public struct RuntimeFieldHandle
    {

    }

    /// <summary>
    /// Provides a mechanism for releasing unmanaged resources.
    /// </summary>
    public interface IDisposable
    {
        /// <summary>
        /// Releases all resource used by the <see cref="System.IDisposable"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="System.IDisposable"/>. The
        /// <see cref="Dispose"/> method leaves the <see cref="System.IDisposable"/> in an unusable state. After calling
        /// <see cref="Dispose"/>, you must release all references to the <see cref="System.IDisposable"/> so the
        /// garbage collector can reclaim the memory that the <see cref="System.IDisposable"/> was occupying.</remarks>
        void Dispose();
    }
}

namespace System.Runtime.Versioning
{
    [Imported]
    public class TargetFrameworkAttribute : Attribute
    {
        public TargetFrameworkAttribute(string targetFrameworkVersion)
        {
            
        }
    }
}

namespace System.Runtime.InteropServices
{
    [Imported]
    public class OutAttribute : Attribute
    {

    }
}

namespace System.Runtime.CompilerServices
{
    [Imported]
    public class RuntimeHelpers
    {
        public static void InitializeArray(Array array, RuntimeFieldHandle fldHandle)
        {
        }
    }
}

namespace System.Collections
{
    [Imported]
    public interface IEnumerable
    {
        IEnumerator GetEnumerator();
    }

    [Imported]
    public interface IEnumerator
    {
        object Current{get;}

        bool MoveNext();
    }
}