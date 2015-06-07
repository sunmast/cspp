namespace System
{
    using System.Reflection;

    [Imported, BuiltInType("void")]
    public struct Void
    {

    }

    [Imported, BuiltInType("object")]
    public class Object
    {

    }

    [Imported, BuiltInType("uint8_t")]
    public struct Byte
    {

    }

    [Imported, BuiltInType("int16_t")]
    public struct Int16
    {

    }

    [Imported, BuiltInType("int32_t")]
    public struct Int32
    {

    }

    [Imported, BuiltInType("int64_t")]
    public struct Int64
    {

    }

    [Imported, BuiltInType("float")]
    public struct Single
    {

    }

    [Imported, BuiltInType("double")]
    public struct Double
    {

    }

    [Imported, BuiltInType("char", "wchar_t")]
    public struct Char
    {

    }

    [Imported, BuiltInType("bool")]
    public struct Boolean
    {

    }

    [Imported, BuiltInType("int8_t")]
    public struct SByte
    {

    }

    [Imported, BuiltInType("uint16_t")]
    public struct UInt16
    {

    }

    [Imported, BuiltInType("uint32_t")]
    public struct UInt32
    {

    }

    [Imported, BuiltInType("uint64_t")]
    public struct UInt64
    {

    }

    [Imported, BuiltInType("decimal")]
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