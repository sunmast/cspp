using System;

/// <summary>
/// C Language Library
/// </summary>
/// <remarks>http://www.cplusplus.com/reference/clibrary/ </remarks>
[Imported, Alias("")]
public static class C
{
    [Imported, Alias("")]
    public static class Debug
    {
        #region cassert

        /// <summary>
        /// If the argument expression of this macro with functional form compares equal to zero (i.e., the expression is false), a message is written to the standard error device and abort is called, terminating the program execution.
        /// </summary>
        /// <param name="expression">Expression to be evaluated. If this expression evaluates to 0, this causes an assertion failure that terminates the program.</param>
        /// <remarks>
        /// http://www.cplusplus.com/reference/cassert/assert/
        /// </remarks>
        [Header("cassert"), Alias("assert")]
        public static extern void Assert(bool expression);

        #endregion // cassert

        #region cerrno

        /// <summary>
        /// The last error number, it can be both read and modified by a program.
        /// </summary>
        /// <remarks>http://www.cplusplus.com/reference/cerrno/errno/ </remarks>
        [Header("cerrno"), Alias("errno")]
        public static int LastError;

        #endregion // cerrno
    }

    #region cctype

    /// <summary>
    /// This class declares a set of functions to classify and transform individual characters.
    /// </summary>
    /// <remarks>http://www.cplusplus.com/reference/cctype/ </remarks>
    [Imported, Alias("")]
    public static class Char
    {
        /// <summary>
        /// Checks whether c is either a decimal digit or an uppercase or lowercase letter.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is either a digit or a letter. Zero (i.e., false) otherwise.<returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/isalnum/ </remarks>
        [Header("cctype"), Alias("isalnum")]
        public static extern bool IsAlphaOrNumeric(char c);

        /// <summary>
        /// Checks whether c is an alphabetic letter.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is an alphabetic letter. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/isalnum/ </remarks>
        [Header("cctype"), Alias("isalpha")]
        public static extern bool IsAlpha(char c);

        /// <summary>
        /// Checks whether c is a blank character.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is a blank character. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/isblank/ </remarks>
        [Header("cctype"), Alias("isblank")]
        public static extern bool IsBlank(char c);

        /// <summary>
        /// Checks whether c is a control character.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is a control character. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/iscntrl/ </remarks>
        [Header("cctype"), Alias("iscntrl")]
        public static extern bool IsControl(char c);

        /// <summary>
        /// Checks whether c is a decimal digit character.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is a decimal digit. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/isdigit/ </remarks>
        [Header("cctype"), Alias("isdigit")]
        public static extern bool IsDigit(char c);

        /// <summary>
        /// Checks whether c is a character with graphical representation.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c has a graphical representation as character. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/isgraph/ </remarks>
        [Header("cctype"), Alias("isgraph")]
        public static extern bool IsGraph(char c);

        /// <summary>
        /// Checks whether c is a lowercase letter.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is a lowercase alphabetic letter. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/islower/ </remarks>
        [Header("cctype"), Alias("islower")]
        public static extern bool IsLower(char c);

        /// <summary>
        /// Checks whether c is a printable character.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is a printable character. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/isprint/ </remarks>
        [Header("cctype"), Alias("isprint")]
        public static extern bool IsPrintable(char c);

        /// <summary>
        /// Checks whether c is a punctuation character.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is a punctuation character. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/ispunct/ </remarks>
        [Header("cctype"), Alias("ispunct")]
        public static extern bool IsPunctuation(char c);

        /// <summary>
        /// Checks whether c is a white-space character.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is a white-space character. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/isspace/ </remarks>
        [Header("cctype"), Alias("isspace")]
        public static extern bool IsSpace(char c);

        /// <summary>
        /// Checks if parameter c is an uppercase alphabetic letter.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is an uppercase alphabetic letter. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/isupper/ </remarks>
        [Header("cctype"), Alias("isupper")]
        public static extern bool IsUpper(char c);

        /// <summary>
        /// Checks whether c is a hexdecimal digit character.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is a hexadecimal digit. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/isxdigit/ </remarks>
        [Header("cctype"), Alias("isxdigit")]
        public static extern bool IsHexDigit(char c);

        /// <summary>
        /// Converts c to its lowercase equivalent if c is an uppercase letter and has a lowercase equivalent. If no such conversion is possible, the value returned is c unchanged.
        /// </summary>
        /// <returns>The lowercase equivalent to c, if such value exists, or c (unchanged) otherwise. The value is returned as an int value that can be implicitly casted to char.</returns>
        /// <param name="c">Character to be converted.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/tolower/ </remarks>
        [Header("cctype"), Alias("tolower")]
        public static extern int ToLower(char c);

        /// <summary>
        /// Converts c to its uppercase equivalent if c is a lowercase letter and has an uppercase equivalent. If no such conversion is possible, the value returned is c unchanged.
        /// </summary>
        /// <returns>The uppercase equivalent to c, if such value exists, or c (unchanged) otherwise. The value is returned as an int value that can be implicitly casted to char.</returns>
        /// <param name="c">Character to be converted.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/toupper/ </remarks>
        [Header("cctype"), Alias("toupper")]
        public static extern int ToUpper(char c);
    }

    #endregion // cctype

    // TODO: cfenv

    #region cfloat

    /// <summary>
    /// This class describes the characteristics of floating types for the specific system and compiler implementation used.
    /// </summary>
    /// <remarks>http://www.cplusplus.com/reference/cfloat/ <remarks>
    [Imported, Alias("")]
    public static class Float
    {
        /// <summary>
        /// Base for all floating-point types (float, double and long double).
        /// </summary>
        [Header("cfloat"), Alias("FLT_RADIX")]
        public static readonly ushort Radix;

        /// <summary>
        /// Precision of significand, i.e. the number of digits that conform the significand.
        /// </summary>
        [Header("cfloat"), Alias("FLT_MANT_DIG")]
        public static readonly ushort FloatMantDig;

        /// <summary>
        /// Precision of significand, i.e. the number of digits that conform the significand.
        /// </summary>
        [Header("cfloat"), Alias("DBL_MANT_DIG")]
        public static readonly ushort DoubleMantDig;

        /// <summary>
        /// Precision of significand, i.e. the number of digits that conform the significand.
        /// </summary>
        [Header("cfloat"), Alias("LDBL_MANT_DIG")]
        public static readonly ushort LongDoubleMantDig;

        /// <summary>
        /// Number of decimal digits that can be rounded into a floating-point and back without change in the number of decimal digits.
        /// </summary>
        [Header("cfloat"), Alias("FLT_DIG")]
        public static readonly ushort FloatDig;

        /// <summary>
        /// Number of decimal digits that can be rounded into a floating-point and back without change in the number of decimal digits.
        /// </summary>
        [Header("cfloat"), Alias("DBL_DIG")]
        public static readonly ushort DoubleDig;

        /// <summary>
        /// Number of decimal digits that can be rounded into a floating-point and back without change in the number of decimal digits.
        /// </summary>
        [Header("cfloat"), Alias("LDBL_DIG")]
        public static readonly ushort LongDoubleDig;

        /// <summary>
        /// Minimum negative integer value for the exponent that generates a normalized floating-point number.
        /// </summary>
        [Header("cfloat"), Alias("FLT_MIN_EXP")]
        public static readonly float MinFloatExp;

        /// <summary>
        /// Minimum negative integer value for the exponent that generates a normalized floating-point number.
        /// </summary>
        [Header("cfloat"), Alias("DBL_MIN_EXP")]
        public static readonly double MinDoubleExp;

        /// <summary>
        /// Minimum negative integer value for the exponent that generates a normalized floating-point number.
        /// </summary>
        [Header("cfloat"), Alias("LDBL_MIN_EXP")]
        public static readonly ldouble MinLongDoubleExp;

        /// <summary>
        /// Minimum negative integer value for the exponent of a base-10 expression that would generate a normalized floating-point number.
        /// </summary>
        [Header("cfloat"), Alias("FLT_MIN_10_EXP")]
        public static readonly short MinFloatBase10Exp;

        /// <summary>
        /// Minimum negative integer value for the exponent of a base-10 expression that would generate a normalized floating-point number.
        /// </summary>
        [Header("cfloat"), Alias("DBL_MIN_10_EXP")]
        public static readonly short MinDoubleBase10Exp;

        /// <summary>
        /// Minimum negative integer value for the exponent of a base-10 expression that would generate a normalized floating-point number.
        /// </summary>
        [Header("cfloat"), Alias("LDBL_MIN_10_EXP")]
        public static readonly short MinLongDoubleBase10Exp;

        /// <summary>
        /// Maximum integer value for the exponent that generates a normalized floating-point number.
        /// </summary>
        [Header("cfloat"), Alias("FLT_MAX_EXP")]
        public static readonly float MaxFloatExp;

        /// <summary>
        /// Maximum integer value for the exponent that generates a normalized floating-point number.
        /// </summary>
        [Header("cfloat"), Alias("DBL_MAX_EXP")]
        public static readonly double MaxDoubleExp;

        /// <summary>
        /// Maximum integer value for the exponent that generates a normalized floating-point number.
        /// </summary>
        [Header("cfloat"), Alias("LDBL_MAX_EXP")]
        public static readonly ldouble MaxLongDoubleExp;

        /// <summary>
        /// Maximum integer value for the exponent that generates a normalized floating-point number.
        /// </summary>
        [Header("cfloat"), Alias("FLT_MAX_10_EXP")]
        public static readonly short MaxFloatBase10Exp;

        /// <summary>
        /// Maximum integer value for the exponent that generates a normalized floating-point number.
        /// </summary>
        [Header("cfloat"), Alias("DBL_MAX_10_EXP")]
        public static readonly short MaxDoubleBase10Exp;

        /// <summary>
        /// Maximum integer value for the exponent that generates a normalized floating-point number.
        /// </summary>
        [Header("cfloat"), Alias("LDBL_MAX_10_EXP")]
        public static readonly short MaxLongDoubleBase10Exp;

        /// <summary>
        /// Maximum finite representable floating-point number.
        /// </summary>
        [Header("cfloat"), Alias("FLT_MAX")]
        public static readonly float MaxFloat;

        /// <summary>
        /// Maximum finite representable floating-point number.
        /// </summary>
        [Header("cfloat"), Alias("DBL_MAX")]
        public static readonly double MaxDouble;

        /// <summary>
        /// Maximum finite representable floating-point number.
        /// </summary>
        [Header("cfloat"), Alias("LDBL_MAX")]
        public static readonly ldouble MaxLongDouble;

        /// <summary>
        /// Difference between 1 and the least value greater than 1 that is representable.
        /// </summary>
        [Header("cfloat"), Alias("FLT_EPSILON")]
        public static readonly float FloatEpsilon;

        /// <summary>
        /// Difference between 1 and the least value greater than 1 that is representable.
        /// </summary>
        [Header("cfloat"), Alias("DBL_EPSILON")]
        public static readonly double DoubleEpsilon;

        /// <summary>
        /// Difference between 1 and the least value greater than 1 that is representable.
        /// </summary>
        [Header("cfloat"), Alias("LDBL_EPSILON")]
        public static readonly ldouble LongDoubleEpsilon;

        /// <summary>
        /// Minimum representable floating-point number.
        /// </summary>
        [Header("cfloat"), Alias("FLT_MIN")]
        public static readonly float MinFloat;

        /// <summary>
        /// Minimum representable floating-point number.
        /// </summary>
        [Header("cfloat"), Alias("DBL_MIN")]
        public static readonly double MinDouble;

        /// <summary>
        /// Minimum representable floating-point number.
        /// </summary>
        [Header("cfloat"), Alias("LDBL_MIN")]
        public static readonly ldouble MinLongDouble;

        /// <summary>
        /// Rounding behavior. Possible values:
        /// -1 undetermined
        /// 0 toward zero
        /// 1 to nearest
        /// 2 toward positive infinity
        /// 3 toward negative infinity
        /// </summary>
        [Header("cfloat"), Alias("FLT_ROUNDS")]
        public static readonly short RoundingBehavior;

        /// <summary>
        /// Properties of the evaluation format. Possible values:
        /// -1 undetermined
        /// 0 evaluate just to the range and precision of the type
        /// 1 evaluate float and double as double, and long double as long double.
        /// 2 evaluate all as long double Other negative values indicate an implementation-defined behavior.
        /// </summary>
        [Header("cfloat"), Alias("FLT_EVAL_METHOD")]
        public static readonly short EvalMethod;

        /// <summary>
        /// Number of decimal digits that can be rounded into a floating-point type and back again to the same decimal digits, without loss in precision.
        /// </summary>
        [Header("cfloat"), Alias("DECIMAL_DIG")]
        public static readonly ushort DecimalDig;
    }

    #endregion // cfloat

    // TODO: cinttypes, ciso646

    #region climits

    /// <summary>
    /// This class defines constants with the limits of fundamental integral types for the specific system and compiler implementation used.
    /// </summary>
    /// <remarks>http://www.cplusplus.com/reference/climits/ <remarks>
    [Imported, Alias("")]
    public static class Limits
    {
        /// <summary>
        /// Number of bits in a char object (byte)  
        /// </summary>
        [Header("climits"), Alias("CHAR_BIT")]
        public static readonly ushort CharBit;

        /// <summary>
        /// Minimum value for an object of type signed char 
        /// </summary>
        [Header("climits"), Alias("SCHAR_MIN")]
        public static readonly short SignedCharMin;

        /// <summary>
        /// Maximum value for an object of type signed char 
        /// </summary>
        [Header("climits"), Alias("SCHAR_MAX")]
        public static readonly short SignedCharMax;

        /// <summary>
        /// Maximum value for an object of type unsigned char   
        /// </summary>
        [Header("climits"), Alias("UCHAR_MAX")]
        public static readonly ushort UnsignedCharMax;

        /// <summary>
        /// Minimum value for an object of type char    
        /// </summary>
        [Header("climits"), Alias("CHAR_MIN")]
        public static readonly ushort CharMin;

        /// <summary>
        /// Maximum value for an object of type char    
        /// </summary>
        [Header("climits"), Alias("CHAR_MAX")]
        public static readonly ushort CharMax;

        /// <summary>
        /// Maximum number of bytes in a multibyte character, for any locale    
        /// </summary>
        [Header("climits"), Alias("MB_LEN_MAX")]
        public static readonly ushort MultiByteCharLenMax;

        /// <summary>
        /// Minimum value for an object of type short int   
        /// </summary>
        [Header("climits"), Alias("SHRT_MIN")]
        public static readonly short ShortMin;

        /// <summary>
        /// Maximum value for an object of type short int   
        /// </summary>
        [Header("climits"), Alias("SHRT_MAX")]
        public static readonly short ShortMax;

        /// <summary>
        /// Maximum value for an object of type unsigned short int  
        /// </summary>
        [Header("climits"), Alias("USHRT_MAX")]
        public static readonly ushort UnsignedShortMax;

        /// <summary>
        /// Minimum value for an object of type int 
        /// </summary>
        [Header("climits"), Alias("INT_MIN")]
        public static readonly int IntMin;

        /// <summary>
        /// Maximum value for an object of type int 
        /// </summary>
        [Header("climits"), Alias("INT_MAX")]
        public static readonly int IntMax;

        /// <summary>
        /// Maximum value for an object of type unsigned int    
        /// </summary>
        [Header("climits"), Alias("UINT_MAX")]
        public static readonly uint UnsignedIntMax;

        /// <summary>
        /// Minimum value for an object of type long int    
        /// </summary>
        [Header("climits"), Alias("LONG_MIN")]
        public static readonly long LongMin;

        /// <summary>
        /// Maximum value for an object of type long int    
        /// </summary>
        [Header("climits"), Alias("LONG_MAX")]
        public static readonly long LongMax;

        /// <summary>
        /// Maximum value for an object of type unsigned long int   
        /// </summary>
        [Header("climits"), Alias("ULONG_MAX")]
        public static readonly ulong UnsignedLongMax;

        /// <summary>
        /// Minimum value for an object of type long long int   
        /// </summary>
        [Header("climits"), Alias("LLONG_MIN")]
        public static readonly long LongLongMin;

        /// <summary>
        /// Maximum value for an object of type long long int   
        /// </summary>
        [Header("climits"), Alias("LLONG_MAX")]
        public static readonly long LongLongMax;

        /// <summary>
        /// Maximum value for an object of type unsigned long long int  
        /// </summary>
        [Header("climits"), Alias("ULLONG_MAX")]
        public static readonly ulong UnsignedLongLongMax;
    }

    #endregion // climits
}