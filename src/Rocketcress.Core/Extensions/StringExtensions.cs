using System;
using System.Text;

namespace Rocketcress.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the string class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Removes all occurrences of a string from the end of another string.
        /// </summary>
        /// <param name="str">The string to trim.</param>
        /// <param name="trimString">The string to remove.</param>
        /// <returns>Returns the trimmed string.</returns>
        public static string TrimEnd(this string str, string trimString)
            => TrimInternal(str, trimString, StringComparison.CurrentCulture, false, true);

        /// <summary>
        /// Removes all occurrences of a string from the end of another string.
        /// </summary>
        /// <param name="str">The string to trim.</param>
        /// <param name="trimString">The string to remove.</param>
        /// <param name="comparison">The comparison to use when matching the trimString.</param>
        /// <returns>Returns the trimmed string.</returns>
        public static string TrimEnd(this string str, string trimString, StringComparison comparison)
            => TrimInternal(str, trimString, comparison, false, true);

        /// <summary>
        /// Removes all occurrences of a string from the start of another string.
        /// </summary>
        /// <param name="str">The string to trim.</param>
        /// <param name="trimString">The string to remove.</param>
        /// <returns>Returns the trimmed string.</returns>
        public static string TrimStart(this string str, string trimString)
            => TrimInternal(str, trimString, StringComparison.CurrentCulture, true, false);

        /// <summary>
        /// Removes all occurrences of a string from the start of another string.
        /// </summary>
        /// <param name="str">The string to trim.</param>
        /// <param name="trimString">The string to remove.</param>
        /// <param name="comparison">The comparison to use when matching the trimString.</param>
        /// <returns>Returns the trimmed string.</returns>
        public static string TrimStart(this string str, string trimString, StringComparison comparison)
            => TrimInternal(str, trimString, comparison, true, false);

        /// <summary>
        /// Removes all occurrences of a string from the start and end of another string.
        /// </summary>
        /// <param name="str">The string to trim.</param>
        /// <param name="trimString">The string to remove.</param>
        /// <returns>Returns the trimmed string.</returns>
        public static string Trim(this string str, string trimString)
            => TrimInternal(str, trimString, StringComparison.CurrentCulture, true, true);

        /// <summary>
        /// Removes all occurrences of a string from the start and end of another string.
        /// </summary>
        /// <param name="str">The string to trim.</param>
        /// <param name="trimString">The string to remove.</param>
        /// <param name="comparison">The comparison to use when matching the trimString.</param>
        /// <returns>Returns the trimmed string.</returns>
        public static string Trim(this string str, string trimString, StringComparison comparison)
            => TrimInternal(str, trimString, comparison, true, true);

        private static string TrimInternal(string str, string trimString, StringComparison comparison, bool trimStart, bool trimEnd)
        {
            if (str.Length < trimString.Length)
                return str;
            var result = new StringBuilder(str);
            if (trimStart)
            {
                while (result.Length >= trimString.Length && string.Equals(result.ToString(0, trimString.Length), trimString, comparison))
                    result.Remove(0, trimString.Length);
            }
            if (trimEnd)
            {
                while (result.Length >= trimString.Length && string.Equals(result.ToString(result.Length - trimString.Length, trimString.Length), trimString, comparison))
                    result.Remove(result.Length - trimString.Length, trimString.Length);
            }
            return result.ToString();
        }
    }
}
