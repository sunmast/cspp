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
        [Header("cctype", "cwctype"), Alias("isalnum", "iswalnum")]
        public static extern bool IsAlphaOrNumeric(char c);

        /// <summary>
        /// Checks whether c is an alphabetic letter.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is an alphabetic letter. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/isalnum/ </remarks>
        [Header("cctype", "cwctype"), Alias("isalpha", "iswalpha")]
        public static extern bool IsAlpha(char c);

        /// <summary>
        /// Checks whether c is a blank character.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is a blank character. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/isblank/ </remarks>
        [Header("cctype", "cwctype"), Alias("isblank", "iswblank")]
        public static extern bool IsBlank(char c);

        /// <summary>
        /// Checks whether c is a control character.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is a control character. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/iscntrl/ </remarks>
        [Header("cctype", "cwctype"), Alias("iscntrl", "iswcntrl")]
        public static extern bool IsControl(char c);

        /// <summary>
        /// Checks whether c is a decimal digit character.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is a decimal digit. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/isdigit/ </remarks>
        [Header("cctype", "cwctype"), Alias("isdigit", "iswdigit")]
        public static extern bool IsDigit(char c);

        /// <summary>
        /// Checks whether c is a character with graphical representation.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c has a graphical representation as character. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/isgraph/ </remarks>
        [Header("cctype", "cwctype"), Alias("isgraph", "iswgraph")]
        public static extern bool IsGraph(char c);

        /// <summary>
        /// Checks whether c is a lowercase letter.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is a lowercase alphabetic letter. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/islower/ </remarks>
        [Header("cctype", "cwctype"), Alias("islower", "iswlower")]
        public static extern bool IsLower(char c);

        /// <summary>
        /// Checks whether c is a printable character.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is a printable character. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/isprint/ </remarks>
        [Header("cctype", "cwctype"), Alias("isprint", "iswprint")]
        public static extern bool IsPrintable(char c);

        /// <summary>
        /// Checks whether c is a punctuation character.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is a punctuation character. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/ispunct/ </remarks>
        [Header("cctype", "cwctype"), Alias("ispunct", "iswpunct")]
        public static extern bool IsPunctuation(char c);

        /// <summary>
        /// Checks whether c is a white-space character.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is a white-space character. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/isspace/ </remarks>
        [Header("cctype", "cwctype"), Alias("isspace", "iswspace")]
        public static extern bool IsSpace(char c);

        /// <summary>
        /// Checks if parameter c is an uppercase alphabetic letter.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is an uppercase alphabetic letter. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/isupper/ </remarks>
        [Header("cctype", "cwctype"), Alias("isupper", "iswupper")]
        public static extern bool IsUpper(char c);

        /// <summary>
        /// Checks whether c is a hexdecimal digit character.
        /// </summary>
        /// <returns>A value different from zero (i.e., true) if indeed c is a hexadecimal digit. Zero (i.e., false) otherwise.</returns>
        /// <param name="c">Character to be checked.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/isxdigit/ </remarks>
        [Header("cctype", "cwctype"), Alias("isxdigit", "iswxdigit")]
        public static extern bool IsHexDigit(char c);

        /// <summary>
        /// Converts c to its lowercase equivalent if c is an uppercase letter and has a lowercase equivalent. If no such conversion is possible, the value returned is c unchanged.
        /// </summary>
        /// <returns>The lowercase equivalent to c, if such value exists, or c (unchanged) otherwise. The value is returned as an int value that can be implicitly casted to char.</returns>
        /// <param name="c">Character to be converted.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/tolower/ </remarks>
        [Header("cctype", "cwctype"), Alias("tolower", "towlower")]
        public static extern int ToLower(char c);

        /// <summary>
        /// Converts c to its uppercase equivalent if c is a lowercase letter and has an uppercase equivalent. If no such conversion is possible, the value returned is c unchanged.
        /// </summary>
        /// <returns>The uppercase equivalent to c, if such value exists, or c (unchanged) otherwise. The value is returned as an int value that can be implicitly casted to char.</returns>
        /// <param name="c">Character to be converted.</param>
        /// <remarks>http://www.cplusplus.com/reference/cctype/toupper/ </remarks>
        [Header("cctype", "cwctype"), Alias("toupper", "towupper")]
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

    #region clocale

    /// <summary>
    /// The C language supports localization specific settings, such as culture-specific date formats or country-specific currency symbols.
    /// </summary>
    /// <remarks>http://www.cplusplus.com/reference/clocale/ </remarks>
    [Imported, Alias("")]
    public static class Locale
    {
        /// <summary>
        /// The entire locale.
        /// </summary>
        [Header("clocale"), Alias("LC_ALL")]
        public static readonly int All;

        /// <summary>
        /// Affects the behavior of strcoll and strxfrm.
        /// </summary>
        [Header("clocale"), Alias("LC_COLLATE")]
        public static readonly int Collate;

        /// <summary>
        /// Affects character handling functions (all functions of <cctype>, except isdigit and isxdigit), and the multibyte and wide character functions.
        /// </summary>
        [Header("clocale"), Alias("LC_CTYPE")]
        public static readonly int CType;

        /// <summary>
        /// Affects monetary formatting information returned by localeconv.
        /// </summary>
        [Header("clocale"), Alias("LC_MONETARY")]
        public static readonly int Monetary;

        /// <summary>
        /// Affects the decimal-point character in formatted input/output operations and string formatting functions, as well as non-monetary information returned by localeconv.
        /// </summary>
        [Header("clocale"), Alias("LC_NUMERIC")]
        public static readonly int Numeric;

        /// <summary>
        /// Affects the behavior of strftime.
        /// </summary>
        [Header("clocale"), Alias("LC_TIME")]
        public static readonly int Time;

        /// <summary>
        /// Sets locale information to be used by the current program, either changing the entire locale or portions of it. The function can also be used to retrieve the current locale's name by passing NULL as the value for argument locale.
        /// </summary>
        /// <returns>
        /// On success, A pointer to a C string identifying the locale currently set for the category. If category is LC_ALL and different portions of the locale are set to different values, the string returned gives this information in a format which may vary between library implementations.
        /// If the function failed to set a new locale, this is not modified and a null pointer is returned.</returns>
        /// <param name="category">Portion of the locale affected.</param>
        /// <param name="locale">C string containing the name of a C locale.</param>
        [Header("clocale"), Alias("setlocale"), AsciiString]
        public static extern string SetLocale(int category, [AsciiString]string locale);

        // TODO:
        // struct lconv* localeconv (void);
    }

    #endregion // clocale

    #region cmath

    /// <summary>
    /// This class declares a set of functions to compute common mathematical operations and transformations.
    /// </summary>
    /// <remarks>http://www.cplusplus.com/reference/cmath/ </remarks>
    [Imported, Alias("")]
    public static class Math
    {
        //Trigonometric functions

        [Header("cmath")]
        public static extern float cos(float x);

        [Header("cmath")]
        public static extern double cos(double x);

        [Header("cmath")]
        public static extern ldouble cos(ldouble x);

        [Header("cmath")]
        public static extern float sin(float x);

        [Header("cmath")]
        public static extern double sin(double x);

        [Header("cmath")]
        public static extern ldouble sin(ldouble x);

        [Header("cmath")]
        public static extern float tan(float x);

        [Header("cmath")]
        public static extern double tan(double x);

        [Header("cmath")]
        public static extern ldouble tan(ldouble x);

        [Header("cmath")]
        public static extern float acos(float x);

        [Header("cmath")]
        public static extern double acos(double x);

        [Header("cmath")]
        public static extern ldouble acos(ldouble x);

        [Header("cmath")]
        public static extern float asin(float x);

        [Header("cmath")]
        public static extern double asin(double x);

        [Header("cmath")]
        public static extern ldouble asin(ldouble x);

        [Header("cmath")]
        public static extern float atan(float x);

        [Header("cmath")]
        public static extern double atan(double x);

        [Header("cmath")]
        public static extern ldouble atan(ldouble x);

        [Header("cmath")]
        public static extern float atan2(float y, float x);

        [Header("cmath")]
        public static extern double atan2(double y, double x);

        [Header("cmath")]
        public static extern ldouble atan2(ldouble y, ldouble x);

        // Hyperbolic functions

        [Header("cmath")]
        public static extern float cosh(float x);

        [Header("cmath")]
        public static extern double cosh(double x);

        [Header("cmath")]
        public static extern ldouble cosh(ldouble x);

        [Header("cmath")]
        public static extern float sinh(float x);

        [Header("cmath")]
        public static extern double sinh(double x);

        [Header("cmath")]
        public static extern ldouble sinh(ldouble x);

        [Header("cmath")]
        public static extern float tanh(float x);

        [Header("cmath")]
        public static extern double tanh(double x);

        [Header("cmath")]
        public static extern ldouble tanh(ldouble x);

        [Header("cmath")]
        public static extern float acosh(float x);

        [Header("cmath")]
        public static extern double acosh(double x);

        [Header("cmath")]
        public static extern ldouble acosh(ldouble x);

        [Header("cmath")]
        public static extern float asinh(float x);

        [Header("cmath")]
        public static extern double asinh(double x);

        [Header("cmath")]
        public static extern ldouble asinh(ldouble x);

        [Header("cmath")]
        public static extern float atanh(float x);

        [Header("cmath")]
        public static extern double atanh(double x);

        [Header("cmath")]
        public static extern ldouble atanh(ldouble x);

        // Exponential and logarithmic functions

        [Header("cmath")]
        public static extern float exp(float x);

        [Header("cmath")]
        public static extern double exp(double x);

        [Header("cmath")]
        public static extern ldouble exp(ldouble x);

        [Header("cmath")]
        public static extern float frexp(float x, out int exp);

        [Header("cmath")]
        public static extern double frexp(double x, out int exp);

        [Header("cmath")]
        public static extern ldouble frexp(ldouble x, out int exp);

        [Header("cmath")]
        public static extern float ldexp(float x, int exp);

        [Header("cmath")]
        public static extern double ldexp(double x, int exp);

        [Header("cmath")]
        public static extern ldouble ldexp(ldouble x, int exp);

        [Header("cmath")]
        public static extern float log(float x);

        [Header("cmath")]
        public static extern double log(double x);

        [Header("cmath")]
        public static extern ldouble log(ldouble x);

        [Header("cmath")]
        public static extern float log10(float x);

        [Header("cmath")]
        public static extern double log10(double x);

        [Header("cmath")]
        public static extern ldouble log10(ldouble x);

        [Header("cmath")]
        public static extern float modf(float x, out float intpart);

        [Header("cmath")]
        public static extern double modf(double x, out double intpart);

        [Header("cmath")]
        public static extern ldouble modf(ldouble x, out ldouble intpart);

        [Header("cmath")]
        public static extern float exp2(float x);

        [Header("cmath")]
        public static extern double exp2(double x);

        [Header("cmath")]
        public static extern ldouble exp2(ldouble x);

        [Header("cmath")]
        public static extern float expm1(float x);

        [Header("cmath")]
        public static extern double expm1(double x);

        [Header("cmath")]
        public static extern ldouble expm1(ldouble x);

        [Header("cmath")]
        public static extern float ilogb(float x);

        [Header("cmath")]
        public static extern double ilogb(double x);

        [Header("cmath")]
        public static extern ldouble ilogb(ldouble x);

        [Header("cmath")]
        public static extern float log1p(float x);

        [Header("cmath")]
        public static extern double log1p(double x);

        [Header("cmath")]
        public static extern ldouble log1p(ldouble x);

        [Header("cmath")]
        public static extern float log2(float x);

        [Header("cmath")]
        public static extern double log2(double x);

        [Header("cmath")]
        public static extern ldouble log2(ldouble x);

        [Header("cmath")]
        public static extern float logb(float x);

        [Header("cmath")]
        public static extern double logb(double x);

        [Header("cmath")]
        public static extern ldouble logb(ldouble x);

        [Header("cmath")]
        public static extern float scalbn(float x, int n);

        [Header("cmath")]
        public static extern double scalbn(double x, int n);

        [Header("cmath")]
        public static extern ldouble scalbn(ldouble x, int n);

        [Header("cmath")]
        public static extern float scalbln(float x, long n);

        [Header("cmath")]
        public static extern double scalbln(double x, long n);

        [Header("cmath")]
        public static extern ldouble scalbln(ldouble x, long n);

        // Power functions

        [Header("cmath")]
        public static extern float pow(float x, float exponent);

        [Header("cmath")]
        public static extern double pow(double x, double exponent);

        [Header("cmath")]
        public static extern ldouble pow(ldouble x, ldouble exponent);

        [Header("cmath")]
        public static extern float sqrt(float x);

        [Header("cmath")]
        public static extern double sqrt(double x);

        [Header("cmath")]
        public static extern ldouble sqrt(ldouble x);

        [Header("cmath")]
        public static extern float cbrt(float x);

        [Header("cmath")]
        public static extern double cbrt(double x);

        [Header("cmath")]
        public static extern ldouble cbrt(ldouble x);

        [Header("cmath")]
        public static extern float hypot(float x, float y);

        [Header("cmath")]
        public static extern double hypot(double x, double y);

        [Header("cmath")]
        public static extern ldouble hypot(ldouble x, ldouble y);

        // Error and gamma functions

        [Header("cmath")]
        public static extern float erf(float x);

        [Header("cmath")]
        public static extern double erf(double x);

        [Header("cmath")]
        public static extern ldouble erf(ldouble x);

        [Header("cmath")]
        public static extern float erfc(float x);

        [Header("cmath")]
        public static extern double erfc(double x);

        [Header("cmath")]
        public static extern ldouble erfc(ldouble x);

        [Header("cmath")]
        public static extern float tgamma(float x);

        [Header("cmath")]
        public static extern double tgamma(double x);

        [Header("cmath")]
        public static extern ldouble tgamma(ldouble x);

        [Header("cmath")]
        public static extern float lgamma(float x);

        [Header("cmath")]
        public static extern double lgamma(double x);

        [Header("cmath")]
        public static extern ldouble lgamma(ldouble x);

        // Rounding and remainder functions

        [Header("cmath")]
        public static extern float ceil(float x);

        [Header("cmath")]
        public static extern double ceil(double x);

        [Header("cmath")]
        public static extern ldouble ceil(ldouble x);

        [Header("cmath")]
        public static extern float floor(float x);

        [Header("cmath")]
        public static extern double floor(double x);

        [Header("cmath")]
        public static extern ldouble floor(ldouble x);

        [Header("cmath")]
        public static extern float fmod(float numer, float denom);

        [Header("cmath")]
        public static extern double fmod(double numer, double denom);

        [Header("cmath")]
        public static extern ldouble fmod(ldouble numer, ldouble denom);

        [Header("cmath")]
        public static extern float trunc(float x);

        [Header("cmath")]
        public static extern double trunc(double x);

        [Header("cmath")]
        public static extern ldouble trunc(ldouble x);

        [Header("cmath")]
        public static extern float round(float x);

        [Header("cmath")]
        public static extern double round(double x);

        [Header("cmath")]
        public static extern ldouble round(ldouble x);

        [Header("cmath")]
        public static extern int lround(float x);

        [Header("cmath")]
        public static extern int lround(double x);

        [Header("cmath")]
        public static extern int lround(ldouble x);

        [Header("cmath")]
        public static extern long llround(float x);

        [Header("cmath")]
        public static extern long llround(double x);

        [Header("cmath")]
        public static extern long llround(ldouble x);

        [Header("cmath")]
        public static extern float rint(float x);

        [Header("cmath")]
        public static extern double rint(double x);

        [Header("cmath")]
        public static extern ldouble rint(ldouble x);

        [Header("cmath")]
        public static extern int lrint(float x);

        [Header("cmath")]
        public static extern int lrint(double x);

        [Header("cmath")]
        public static extern int lrint(ldouble x);

        [Header("cmath")]
        public static extern long llrint(float x);

        [Header("cmath")]
        public static extern long llrint(double x);

        [Header("cmath")]
        public static extern long llrint(ldouble x);

        [Header("cmath")]
        public static extern float nearbyint(float x);

        [Header("cmath")]
        public static extern double nearbyint(double x);

        [Header("cmath")]
        public static extern ldouble nearbyint(ldouble x);

        [Header("cmath")]
        public static extern float remainder(float numer, float denom);

        [Header("cmath")]
        public static extern double remainder(double numer, double denom);

        [Header("cmath")]
        public static extern ldouble remainder(ldouble numer, ldouble denom);

        [Header("cmath")]
        public static extern float remquo(float numer, float denom, out int quot);

        [Header("cmath")]
        public static extern double remquo(double numer, double denom, out int quot);

        [Header("cmath")]
        public static extern ldouble remquo(ldouble numer, ldouble denom, out int quot);

        // Floating-point manipulation functions

        [Header("cmath")]
        public static extern float copysign(float x, float y);

        [Header("cmath")]
        public static extern double copysign(double x, double y);

        [Header("cmath")]
        public static extern ldouble copysign(ldouble x, ldouble y);

        [Header("cmath")]
        public static extern double nan([AsciiString]string tagp);

        [Header("cmath")]
        public static extern float nextafter(float x, float y);

        [Header("cmath")]
        public static extern double nextafter(double x, double y);

        [Header("cmath")]
        public static extern ldouble nextafter(ldouble x, ldouble y);

        [Header("cmath")]
        public static extern float nexttoward(float x, ldouble y);

        [Header("cmath")]
        public static extern double nexttoward(double x, ldouble y);

        [Header("cmath")]
        public static extern ldouble nexttoward(ldouble x, ldouble y);

        // Minimum, maximum, difference functions

        [Header("cmath")]
        public static extern float fdim(float x, float y);

        [Header("cmath")]
        public static extern double fdim(double x, double y);

        [Header("cmath")]
        public static extern ldouble fdim(ldouble x, ldouble y);

        [Header("cmath")]
        public static extern float fmax(float x, float y);

        [Header("cmath")]
        public static extern double fmax(double x, double y);

        [Header("cmath")]
        public static extern ldouble fmax(ldouble x, ldouble y);

        [Header("cmath")]
        public static extern float fmin(float x, float y);

        [Header("cmath")]
        public static extern double fmin(double x, double y);

        [Header("cmath")]
        public static extern ldouble fmin(ldouble x, ldouble y);

        [Header("cmath")]
        public static extern float fabs(float x);

        [Header("cmath")]
        public static extern double fabs(double x);

        [Header("cmath")]
        public static extern ldouble fabs(ldouble x);

        [Header("cmath")]
        public static extern float abs(float x);

        [Header("cmath")]
        public static extern double abs(double x);

        [Header("cmath")]
        public static extern ldouble abs(ldouble x);

        [Header("cmath")]
        public static extern float fma(float x, float y, float z);

        [Header("cmath")]
        public static extern double fma(double x, double y, double z);

        [Header("cmath")]
        public static extern ldouble fma(ldouble x, ldouble y, ldouble z);

        // Other functions

        // Classification

        [Header("cmath")]
        public static extern int fpclassify(float x);

        [Header("cmath")]
        public static extern int fpclassify(double x);

        [Header("cmath")]
        public static extern int fpclassify(ldouble x);

        [Header("cmath")]
        public static extern bool isfinite(float x);

        [Header("cmath")]
        public static extern bool isfinite(double x);

        [Header("cmath")]
        public static extern bool isfinite(ldouble x);

        [Header("cmath")]
        public static extern bool isinf(float x);

        [Header("cmath")]
        public static extern bool isinf(double x);

        [Header("cmath")]
        public static extern bool isinf(ldouble x);

        [Header("cmath")]
        public static extern bool isnan(float x);

        [Header("cmath")]
        public static extern bool isnan(double x);

        [Header("cmath")]
        public static extern bool isnan(ldouble x);

        [Header("cmath")]
        public static extern bool isnormal(float x);

        [Header("cmath")]
        public static extern bool isnormal(double x);

        [Header("cmath")]
        public static extern bool isnormal(ldouble x);

        [Header("cmath")]
        public static extern bool signbit(float x);

        [Header("cmath")]
        public static extern bool signbit(double x);

        [Header("cmath")]
        public static extern bool signbit(ldouble x);

        // cstdlb

        [Header("cstdlib")]
        public static extern int abs(int x);

        [Header("cstdlib")]
        public static extern long abs(long x);

        [Header("cstdlb")]
        public static extern DivResult div(int numer, int denom);

        [Header("cstdlb")]
        public static extern LongDivResult div(long numer, long denom);
    }

    [Imported, Alias("div_t")]
    public struct DivResult
    {
        [Alias("quot")]
        public int Quotient;

        [Alias("rem")]
        public int Remainder;
    }

    [Imported, Alias("lldiv_t")]
    public struct LongDivResult
    {
        [Alias("quot")]
        public long Quotient;

        [Alias("rem")]
        public long Remainder;
    }

    #endregion // cmath

    #region csignal

    /// <summary>
    /// This class is to handle signals.
    /// </summary>
    /// <remarks>http://www.cplusplus.com/reference/csignal/ </remarks>
    [Imported, Alias("")]
    public static class Signal
    { 
        public delegate void SignalHandler(int param);

        /// <summary>
        /// Abnormal termination, such as is initiated by the abort function.
        /// </summary>
        [Header("csignal"), Alias("SIGABRT")]
        public static int Abort;

        /// <summary>
        /// Erroneous arithmetic operation, such as zero divide or an operation resulting in overflow (not necessarily with a floating-point operation).
        /// </summary>
        [Header("csignal"), Alias("SIGFPE")]
        public static int OverflowException;

        /// <summary>
        /// Invalid function image, such as an illegal instruction. This is generally due to a corruption in the code or to an attempt to execute data.
        /// </summary>
        [Header("csignal"), Alias("SIGILL")]
        public static int IllegalInstruction;

        /// <summary>
        /// Interactive attention signal. Generally generated by the application user.
        /// </summary>
        [Header("csignal"), Alias("SIGINT")]
        public static int Interrupt;

        /// <summary>
        /// Invalid access to storage: When a program tries to read or write outside the memory it is allocated for it.
        /// </summary>
        [Header("csignal"), Alias("SIGSEGV")]
        public static int SegmentationViolation;

        /// <summary>
        /// Termination request sent to program.
        /// </summary>
        [Header("csignal"), Alias("SIGTERM")]
        public static int Terminate;

        /// <summary>
        /// The signal is handled by the default action for that particular signal.
        /// </summary>
        [Header("csignal"), Alias("SIG_DFL")]
        public static SignalHandler DefaultSignalHandler;

        /// <summary>
        /// The signal is ignored.
        /// </summary>
        [Header("csignal"), Alias("SIG_IGN")]
        public static SignalHandler IgnoreSignalHandler;

        /// <summary>
        /// Specifies a way to handle the signals with the signal number specified by sig.
        /// </summary>
        /// <returns>The signal handler.</returns>
        /// <param name="sig">Sig.</param>
        /// <param name="handler">Handler.</param>
        [Header("csignal"), Alias("signal")]
        public static extern SignalHandler RegsiterSignalHandler(int sig, SignalHandler handler);

        /// <summary>
        /// Sends signal sig to the current executing program.
        /// </summary>
        /// <param name="sig">The signal value to raise.</param>
        /// <returns>Returns zero if successful, and a value different from zero otherwise.<returns>
        [Header("csignal"), Alias("raise")]
        public static extern int Raise (int sig);
    }

    [Imported, Header("csignal"), Alias("sig_atomic_t")]
    public enum SignalAtomic : int
    {}

    #endregion // csignal

    #region cstdio

    /// <summary>
    /// This class is to to perform Input/Output operations.
    /// </summary>
    /// <remarks>http://www.cplusplus.com/reference/cstdio/ </remarks>
    [Imported, Alias("")]
    public static class IO
    {
        /// <summary>
        /// Deletes the file whose name is specified in filename.
        /// </summary>
        /// <returns>
        /// If the file is successfully deleted, a zero value is returned.
        /// On failure, a nonzero value is returned.
        /// On most library implementations, the errno variable is also set to a system-specific error code on failure.
        /// </returns>
        /// <param name="path">C string containing the name of the file to be deleted.</param>
        [Header("cstdio"), Alias("remove", "_wremove")]
        public static extern int RemoveFile (string filename);

        /// <summary>
        /// Changes the name of the file or directory specified by oldname to newname.
        /// </summary>
        /// <returns>
        /// If the file is successfully renamed, a zero value is returned.
        /// On failure, a nonzero value is returned.
        /// On most library implementations, the errno variable is also set to a system-specific error code on failure.
        /// </returns>
        /// <param name="oldName">C string containing the name of an existing file to be renamed and/or moved.</param>
        /// <param name="newName">C string containing the new name for the file.</param>
        [Header("cstdio"), Alias("rename", "_wrename")]
        public static extern int RenameFile (string oldName, string newName);
    }

    #endregion // cstdio

    #region cstdlib

    [Imported, Alias("")]
    public static class Convert
    {
        [Header("cstdlib"), Alias("atof", "_wtof")]
        public static extern float ToSingle(string str);

        [Header("cstdlib"), Alias("atof", "_wtof")]
        public static extern double ToDouble(string str);

        [Header("cstdlib"), Alias("atoi", "_wtoi")]
        public static extern int ToInt(string str);

        [Header("cstdlib"), Alias("atoll", "_wtoll")]
        public static extern long ToInt64(string str);
    }

    [Imported, Alias("")]
    public static class Random
    {
        [Header("cstdlib"), Alias("RAND_MAX")]
        public static readonly int MaxValue;
        
        /// <summary>
        /// Initialize random number generator
        /// </summary>
        /// <param name="seed">An integer value to be used as seed by the pseudo-random number generator algorithm.</param>
        [Header("cstdlib"), Alias("srand")]
        public static extern void Initialize(uint seed);

        /// <summary>
        /// Generate random number
        /// </summary>
        /// <returns>An integer value between 0 and RAND_MAX.<returns>
        [Header("cstdlib"), Alias("rand")]
        public static extern int Generate();
    }

    [Imported, Alias("")]
    public static class Memory
    {
        /// <summary>
        /// Allocate and zero-initialize array
        /// </summary>
        /// <param name="num">Number of elements to allocate.</param>
        /// <param name="size">Size of each element.</param>
        /// <returns>>
        /// On success, a pointer to the memory block allocated by the function.
        /// The type of this pointer is always void*, which can be cast to the desired type of data pointer in order to be dereferenceable.
        /// If the function failed to allocate the requested block of memory, a null pointer is returned.
        /// </returns>
        [Header("cstdlib"), Alias("calloc")]
        public static extern IntPtr AllocZero(int num, int size);

        /// <summary>
        /// Allocate memory block
        /// </summary>
        /// <param name="size">Size of the memory block, in bytes.</param>
        /// <returns>
        /// On success, a pointer to the memory block allocated by the function.
        /// The type of this pointer is always void*, which can be cast to the desired type of data pointer in order to be dereferenceable.
        /// If the function failed to allocate the requested block of memory, a null pointer is returned.
        /// <returns>
        [Header("cstdlib"), Alias("malloc")]
        public static extern IntPtr Alloc(int size);

        /// <summary>
        /// Reallocate memory block
        /// </summary>
        /// <returns>The alloc.</returns>
        /// <param name="ptr">Pointer to a memory block previously allocated with malloc, calloc or realloc. Alternatively, this can be a null pointer, in which case a new block is allocated (as if malloc was called).</param>
        /// <param name="size">New size for the memory block, in bytes.</param>
        /// <returns>
        /// A pointer to the reallocated memory block, which may be either the same as ptr or a new location.
        /// The type of this pointer is void*, which can be cast to the desired type of data pointer in order to be dereferenceable.
        /// A null-pointer indicates that the function failed to allocate storage, and thus the block pointed by ptr was not modified.
        /// <returns>
        [Header("cstdlib"), Alias("realloc")]
        public static extern IntPtr ReAlloc(IntPtr ptr, int size);

        /// <summary>
        /// Deallocate memory block
        /// </summary>
        /// <param name="ptr">Pointer to a memory block previously allocated with malloc, calloc or realloc.</param>
        [Header("cstdlib"), Alias("free")]
        public static extern void Free(IntPtr ptr);
    }

    [Imported, Alias("")]
    public static class Environment
    {
        public delegate void ExitHandler();
        
        /// <summary>
        /// Aborts the current process, producing an abnormal program termination.
        /// </summary>
        [Header("cstdlib"), Alias("abort")]
        public static extern void Abort();

        /// <summary>
        /// Set function to be executed on exit
        /// </summary>
        /// <returns>A zero value is returned if the function was successfully registered. If it failed, a non-zero value is returned.</returns>
        /// <param name="exitHandler">Function to be called. The function shall return no value and take no arguments.</param>
        [Header("cstdlib"), Alias("atexit")]
        public static extern int OnExit(ExitHandler exitHandler);

        /// <summary>
        /// The function pointed by func is automatically called (without arguments) when quick_exit is called.
        /// </summary>
        /// <returns>A zero value is returned if the function was successfully registered. If it failed, a non-zero value is returned.</returns>
        /// <param name="exitHandler">Function to be called. The function shall return no value and take no arguments.</param>
        [Header("cstdlib"), Alias("at_quick_exit")]
        public static extern int OnQuickExit(ExitHandler exitHandler);

        /// <summary>
        /// Terminate calling process
        /// </summary>
        /// <param name="status">Status code. If this is 0 or EXIT_SUCCESS, it indicates success. If it is EXIT_FAILURE, it indicates failure..</param>
        [Header("cstdlib"), Alias("exit")]
        public static extern void Exit(int status);

        /// <summary>
        /// Terminate calling process quick
        /// </summary>
        /// <param name="status">Status code. If this is 0 or EXIT_SUCCESS, it indicates success. If it is EXIT_FAILURE, it indicates failure..</param>
        [Header("cstdlib"), Alias("quick_exit")]
        public static extern void QuickExit(int status);

        /// <summary>
        /// Get environment string
        /// </summary>
        /// <returns>A C-string with the value of the requested environment variable, or a null pointer if such environment variable does not exist.</returns>
        /// <param name="name">C-string containing the name of the requested variable. Depending on the platform, this may either be case sensitive or not.</param>
        [Header("cstdlib"), Alias("getenv", "_wgetenv")]
        public static extern string GetVariable(string name);

        /// <summary>
        /// Invokes the command processor to execute a command.
        /// </summary>
        /// <returns>
        /// If command is a null pointer, the function returns a non-zero value in case a command processor is available and a zero value if it is not.
        /// If command is not a null pointer, the value returned depends on the system and library implementations, but it is generally expected to be the status code returned by the called command, if supported.
        /// </returns>
        /// <param name="command">C-string containing the system command to be executed. Or, alternatively, a null pointer, to check for a command processor.</param>
        [Header("cstdlib"), Alias("system", "_wsystem")]
        public static extern int CallSystem(string command);
    }

    #endregion // cstdlib

    #region ctime

    [Imported, Alias("tm")]
    public struct DateTime
    {
        /// <summary>
        /// seconds after the minute (0-60)
        /// </summary>
        /// <remarks>The extra 60 is to accommodate for leap seconds in certain systems.</remarks>
        [Alias("tm_sec")]
        public static int Second;

        /// <summary>
        /// minutes after the hour (0-59)
        /// </summary>
        [Alias("tm_min")]
        public static int Minute;

        /// <summary>
        /// hours since midnight (0-23)
        /// </summary>
        [Alias("tm_hour")]
        public static int Hour;

        /// <summary>
        /// day of the month (1-31)
        /// </summary>
        [Alias("tm_mday")]
        public static int DayOfMonth;

        /// <summary>
        /// months since January (0-11)
        /// </summary>
        [Alias("tm_mon")]
        public static int Month;

        /// <summary>
        /// years since 1900
        /// </summary>
        [Alias("tm_year")]
        public static int Year;

        /// <summary>
        /// days since Sunday (0-6)
        /// </summary>
        [Alias("tm_wday")]
        public static int WeekDay;

        /// <summary>
        /// days since January 1 (0-365)
        /// </summary>
        [Alias("tm_yday")]
        public static int YearDay;

        /// <summary>
        /// Daylight Saving Time flag
        /// </summary>
        [Alias("tm_isdst")]
        public static bool IsDst;
    }

    /// <summary>
    /// This class contains definitions of functions to get and manipulate date and time information.
    /// </summary>
    [Imported, Alias("")]
    public static class Clock
    {
        [Header("ctime"), Alias("clock")]
        public static extern long Ticks();

        /// <summary>
        /// Return difference between two times
        /// </summary>
        /// <param name="end">Higher bound of the time interval whose length is calculated.</param>
        /// <param name="beginning">Lower bound of the time interval whose length is calculated. If this describes a time point later than end, the result is negative.</param>
        /// <returns>The result of (end-beginning) in seconds as a floating-point value of type double.<returns>
        [Header("ctime"), Alias("difftime")]
        public static extern double Diff(long end, long beginning);

        /// <summary>
        /// Convert tm structure to time_t
        /// </summary>
        /// <returns>A time_t value corresponding to the calendar time passed as argument. If the calendar time cannot be represented, a value of -1 is returned.</returns>
        /// <param name="time">Pointer to a tm structure that contains a calendar time broken down into its components (see struct tm).</param>
        [Header("ctime"), Alias("mktime")]
        public static extern long MakeTime(ref DateTime time);

        /// <summary>
        /// Get the current calendar time as a value of type time_t.
        /// </summary>
        /// <param name="time">Pointer to an object of type time_t, where the time value is stored.</param>
        [Header("ctime"), Alias("time")]
        public static extern void GetTime(out long time);

        /// <summary>
        /// Get the current calendar time as a value of type time_t.
        /// </summary>
        /// <returns>The current calendar time as a time_t object.<returns>
        [Header("ctime"), Alias("time"), Zeros(1)]
        public static extern long GetTime();

        /// <summary>
        /// Convert tm structure to string
        /// </summary>
        /// <returns>A C-string containing the date and time information in a human-readable format. The returned value points to an internal array whose validity or value may be altered by any subsequent call to asctime or ctime.</returns>
        /// <param name="time">Pointer to a tm structure that contains a calendar time broken down into its components (see struct tm).</param>
        [Header("ctime"), Alias("asctime")]
        public static extern string ConvertToString(ref DateTime time);

        /// <summary>
        /// Convert time_t value to string
        /// </summary>
        /// <returns>A C-string containing the date and time information in a human-readable format. The returned value points to an internal array whose validity or value may be altered by any subsequent call to asctime or ctime.</returns>
        /// <param name="time">Pointer to an object of type time_t that contains a time value.</param>
        [Header("ctime"), Alias("ctime")]
        public static extern string ConvertToString(ref long time);

        /// <summary>
        /// Convert time_t to tm as UTC time
        /// </summary>
        /// <returns>The UTC time.</returns>
        /// <param name="time">Pointer to an object of type time_t that contains a time value.</param>
        [Header("ctime"), Alias("gmtime"), ReturnType(ReturnType.Pointer)]
        public static extern DateTime GetUtcTime(ref long time);

        /// <summary>
        /// Convert time_t to tm as local time
        /// </summary>
        /// <returns>The local time.</returns>
        /// <param name="time">Pointer to an object of type time_t that contains a time value.</param>
        [Header("ctime"), Alias("localtime"), ReturnType(ReturnType.Pointer)]
        public static extern DateTime GetLocalTime(ref long time);
    }

    #endregion // ctime
}
