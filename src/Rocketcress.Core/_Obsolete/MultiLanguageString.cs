using Newtonsoft.Json;
using System.Globalization;

namespace Rocketcress.Core.Models
{
    /// <summary>
    /// Represents a string that can be translated to other languages.
    /// </summary>
    [JsonConverter(typeof(LanguageDependentJsonConverter))]
    [Obsolete("Use LanguageDependent<string> instead.")]
    public class MultiLanguageString : LanguageDependent<string>
    {
        /// <summary>
        /// Sets the value for a given language.
        /// </summary>
        /// <param name="language">The id of the language.</param>
        /// <param name="value">The new value.</param>
        /// <returns>Returns the current instance of the MultiLanguageString class to chain multiple method calls.</returns>
        public new MultiLanguageString SetLanguage(int language, string value) => (MultiLanguageString)base.SetLanguage(language, value);

        /// <summary>
        /// Sets the value for a given language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="value">The new value.</param>
        /// <returns>Returns the current instance of the MultiLanguageString class to chain multiple method calls.</returns>
        public new MultiLanguageString SetLanguage(CultureInfo language, string value) => (MultiLanguageString)base.SetLanguage(language, value);

        /// <summary>
        /// Sets the value for the english language.
        /// </summary>
        /// <param name="value">The new value.</param>
        /// <returns>Returns the current instance of the MultiLanguageString class to chain multiple method calls.</returns>
        public MultiLanguageString SetEnglish(string value) => SetLanguage(CultureInfo.GetCultureInfo("en"), value);

        /// <summary>
        /// Sets the value for the german language.
        /// </summary>
        /// <param name="value">The new value.</param>
        /// <returns>Returns the current instance of the MultiLanguageString class to chain multiple method calls.</returns>
        public MultiLanguageString SetGerman(string value) => SetLanguage(CultureInfo.GetCultureInfo("de"), value);

        /// <summary>
        /// Sets the value for the french language.
        /// </summary>
        /// <param name="value">The new value.</param>
        /// <returns>Returns the current instance of the MultiLanguageString class to chain multiple method calls.</returns>
        public MultiLanguageString SetFrench(string value) => SetLanguage(CultureInfo.GetCultureInfo("fr"), value);

        /// <summary>
        /// Sets the value for the italian language.
        /// </summary>
        /// <param name="value">The new value.</param>
        /// <returns>Returns the current instance of the MultiLanguageString class to chain multiple method calls.</returns>
        public MultiLanguageString SetItalian(string value) => SetLanguage(CultureInfo.GetCultureInfo("it"), value);
    }
}
