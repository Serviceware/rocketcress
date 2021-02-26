using System.Windows;

namespace Rocketcress.Core.Models
{
    /// <summary>
    /// Provides Extensions for the LanguageDependent class.
    /// </summary>
    public static class LanguageDependentExtensions
    {
        /// <summary>
        /// Sets the value for a given language.
        /// </summary>
        /// <param name="obj">LanguageDependent instance of type Point.</param>
        /// <param name="language">The language.</param>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns>Returns the current instance of the LanguageDependent class to chain multiple method calls.</returns>
        public static LanguageDependent<Point> SetLanguage(this LanguageDependent<Point> obj, KnownLanguages language, double x, double y)
        {
            return obj.SetLanguage(language, new Point(x, y));
        }

        /// <summary>
        /// Sets the value for the german language.
        /// </summary>
        /// <param name="obj">LanguageDependent instance of type Point.</param>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns>Returns the current instance of the LanguageDependent class to chain multiple method calls.</returns>
        public static LanguageDependent<Point> SetGerman(this LanguageDependent<Point> obj, double x, double y)
        {
            return obj.SetLanguage(KnownLanguages.German, new Point(x, y));
        }

        /// <summary>
        /// Sets the value for the english language.
        /// </summary>
        /// <param name="obj">LanguageDependent instance of type Point.</param>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns>Returns the current instance of the LanguageDependent class to chain multiple method calls.</returns>
        public static LanguageDependent<Point> SetEnglish(this LanguageDependent<Point> obj, double x, double y)
        {
            return obj.SetLanguage(KnownLanguages.English, new Point(x, y));
        }

        /// <summary>
        /// Sets the value for the french language.
        /// </summary>
        /// <param name="obj">LanguageDependent instance of type Point.</param>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns>Returns the current instance of the LanguageDependent class to chain multiple method calls.</returns>
        public static LanguageDependent<Point> SetFrench(this LanguageDependent<Point> obj, double x, double y)
        {
            return obj.SetLanguage(KnownLanguages.French, new Point(x, y));
        }

        /// <summary>
        /// Sets the value for the italian language.
        /// </summary>
        /// <param name="obj">LanguageDependent instance of type Point.</param>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns>Returns the current instance of the LanguageDependent class to chain multiple method calls.</returns>
        public static LanguageDependent<Point> SetItalian(this LanguageDependent<Point> obj, double x, double y)
        {
            return obj.SetLanguage(KnownLanguages.Italian, new Point(x, y));
        }
    }
}
