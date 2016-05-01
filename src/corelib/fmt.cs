using System;

[Imported]
public static class fmt
{
    [Alias("format")]
    public static extern string Format(xstring format, params object[] args);

    [Alias("print")]
    public static extern void Print(xstring format, params object[] args);
}