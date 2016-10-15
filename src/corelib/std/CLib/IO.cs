namespace std
{
    /// <summary>
    /// C library to perform Input/Output operations.
    /// </summary>
    /// <remarks>
    /// http://www.cplusplus.com/reference/cstdio/
    /// </remarks>
    [Imported, Alias("")]
    public struct IO
    {
        #region Formatted input/output

        /// <summary>
        /// Print formatted data to stdout.
        /// All strings passed into this function should be converted to C-Style string first. E.g. string.CStr
        /// </summary>
        /// <param name="format">
        /// C string that contains the text to be written to stdout.
        /// It can optionally contain embedded format specifiers that are replaced by the values specified in subsequent additional arguments and formatted as requested.
        /// </param>
        /// <param name="args">
        /// Depending on the format string, the function may expect a sequence of additional arguments, each containing a value to be used to replace a format specifier in the format string (or a pointer to a storage location, for n).
        /// There should be at least as many of these arguments as the number of values specified in the format specifiers. Additional arguments are ignored by the function.
        /// </param>
        /// <returns>
        /// On success, the total number of characters written is returned.
        /// If a writing error occurs, the error indicator (ferror) is set and a negative number is returned.
        /// If a multibyte character encoding error occurs while writing wide characters, errno is set to EILSEQ and a negative number is returned.
        /// </returns>
        [Alias("printf", "wprintf")]
        public static extern int PrintF(string format, params object[] args);

        [Alias("getchar")]
        public static extern int GetChar();

        #endregion
    }
}
