/// <summary>
/// The integer type defined as "size_t". The type is usually uint for 32bit programs and ulong for 64bit programs.
/// It's commonly used for array indexing and loop counting.
/// </summary>
[Imported, Alias("size_t")]
public struct xint
{
    public static extern implicit operator xint(byte i);

    public static extern implicit operator byte(xint i);

    public static extern implicit operator xint(sbyte i);

    public static extern implicit operator sbyte(xint i);

    public static extern implicit operator xint(short i);

    public static extern implicit operator short(xint i);

    public static extern implicit operator xint(ushort i);

    public static extern implicit operator ushort(xint i);

    public static extern implicit operator xint(int i);

    public static extern implicit operator int(xint i);

    public static extern implicit operator xint(uint i);

    public static extern implicit operator uint(xint i);

    public static extern implicit operator xint(long i);

    public static extern implicit operator long(xint i);

    public static extern implicit operator xint(ulong i);

    public static extern implicit operator ulong(xint i);

    public static extern xint operator +(xint x);

    public static extern xint operator -(xint x);

    public static extern xint operator !(xint x);

    public static extern xint operator ~(xint x);

    public static extern xint operator ++(xint x);

    public static extern xint operator --(xint x);

    public static extern bool operator true(xint x);

    public static extern bool operator false(xint x);

    public static extern xint operator +(xint x, xint y);

    public static extern xint operator -(xint x, xint y);

    public static extern xint operator *(xint x, xint y);

    public static extern xint operator /(xint x, xint y);

    public static extern xint operator %(xint x, xint y);

    public static extern xint operator &(xint x, xint y);

    public static extern xint operator |(xint x, xint y);

    public static extern xint operator ^(xint x, xint y);

    public static extern xint operator <<(xint x, int y);

    public static extern xint operator >>(xint x, int y);

    public static extern bool operator ==(xint x, xint y);

    public static extern bool operator !=(xint x, xint y);

    public static extern bool operator <(xint x, xint y);

    public static extern bool operator >(xint x, xint y);

    public static extern bool operator <=(xint x, xint y);

    public static extern bool operator >=(xint x, xint y);
}
