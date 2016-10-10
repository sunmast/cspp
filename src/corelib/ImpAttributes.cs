using System;

[Imported, AttributeUsage(AttributeTargets.All)]
public class AliasAttribute : Attribute
{
    public readonly string Alias;

    public readonly string AltAlias;

    public AliasAttribute(string alias, string altAlias = null)
    {
        this.Alias = alias;
        this.AltAlias = altAlias;
    }
}

[Imported, AttributeUsage(AttributeTargets.All)]
public class ImportedAttribute : Attribute
{
}

[Imported, AttributeUsage(AttributeTargets.All)]
public class HeaderAttribute : Attribute
{
    public readonly string Header;

    public readonly string AltHeader;

    public HeaderAttribute(string header, string altHeader = null)
    {
        this.Header = header;
        this.AltHeader = altHeader;
    }
}

[Imported, AttributeUsage(AttributeTargets.Field)]
public sealed class WeakRefAttribute : Attribute
{

}

[Imported, AttributeUsage(AttributeTargets.Method)]
public sealed class ZerosAttribute : Attribute
{
    public readonly int Count;

    public ZerosAttribute(int count)
    {
        this.Count = count;
    }
}

[Imported]
public enum RtType
{
    AsciiStr,
    Utf8Str,
    WideStr,
    Ptr,
    Ref
}

[Imported, AttributeUsage(AttributeTargets.Parameter)]
public class ParamAttribute : Attribute
{
    public readonly RtType RtType;

    public ParamAttribute(RtType rtType)
    {
        this.RtType = rtType;
    }
}

[Imported, AttributeUsage(AttributeTargets.Method)]
public class ReturnAttribute : ParamAttribute
{
    public ReturnAttribute(RtType rtType) : base(rtType)
    { }
}

[Imported, AttributeUsage(AttributeTargets.Parameter)]
public sealed class InAttribute : Attribute
{
}

[Imported, AttributeUsage(AttributeTargets.Parameter)]
public sealed class OutAttribute : Attribute
{
}
