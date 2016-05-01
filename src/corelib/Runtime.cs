namespace System
{
    using System.Reflection;

    [Imported, BuiltInType]
    public struct Void
    {

    }

    [Imported, BuiltInType]
    public class Object
    {

    }

    [Imported, BuiltInType]
    public struct Byte
    {

    }

    [Imported, BuiltInType]
    public struct Int16
    {

    }

    [Imported, BuiltInType]
    public struct Int32
    {

    }

    [Imported, BuiltInType]
    public struct Int64
    {

    }

    [Imported, BuiltInType]
    public struct Single
    {

    }

    [Imported, BuiltInType]
    public struct Double
    {

    }

    [Imported, BuiltInType]
    public struct Char
    {

    }

    [Imported, BuiltInType]
    public struct Boolean
    {

    }

    [Imported, BuiltInType]
    public struct SByte
    {

    }

    [Imported, BuiltInType]
    public struct UInt16
    {

    }

    [Imported, BuiltInType]
    public struct UInt32
    {

    }

    [Imported, BuiltInType]
    public struct UInt64
    {

    }

    [Imported, BuiltInType]
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

    }
}