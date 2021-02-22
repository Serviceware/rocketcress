using System;

namespace Rocketcress.Core
{
    /// <summary>
    /// Specifies constants for known languages.
    /// </summary>
    public enum LanguageOptions
    {
        /// <summary>
        /// English (United States) - 1033
        /// </summary>
        English = 1033,

        /// <summary>
        /// German (Germany) - 1031
        /// </summary>
        German = 1031,

        /// <summary>
        /// French (France) - 1036
        /// </summary>
        French = 1036,

        /// <summary>
        /// Italian (Italy) - 1040
        /// </summary>
        Italian = 1040,

        /// <summary>
        /// German (Switzerland) - 2055
        /// </summary>
        GermanSwitzerland = 2055
    }

    /// <summary>
    /// Specifies constants for knwon browsers.
    /// </summary>
    [Flags]
    public enum Browser : int
    {
        /// <summary>
        /// Unknown browser
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Microsoft Internet Explorer
        /// </summary>
        InternetExplorer = 1, 

        /// <summary>
        /// Mozilla Firefox
        /// </summary>
        Firefox = 2, 

        /// <summary>
        /// Google Chrome
        /// </summary>
        Chrome = 3 | Chromium,

        /// <summary>
        /// Microsoft Edge (Chromium)
        /// </summary>
        Edge = 4 | Chromium,

        /// <summary>
        /// Browsers based on Chromium or Chromium itself
        /// </summary>
        Chromium = 0b1_0000_0000
    }

    /// <summary>
    /// Specifies constants for known parent languages.
    /// </summary>
    public enum KnownLanguages
    {
        /// <summary>
        /// English (9)
        /// </summary>
        English = 9,

        /// <summary>
        /// German (7)
        /// </summary>
        German = 7,

        /// <summary>
        /// French (12)
        /// </summary>
        French = 12,

        /// <summary>
        /// Italian (16)
        /// </summary>
        Italian = 16
    }

    /// <summary>
    /// Specifies constants for log levels.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Used for log messages that are used to debug.
        /// </summary>
        Debug,

        /// <summary>
        /// Used for log messages that show useful information.
        /// </summary>
        Info,

        /// <summary>
        /// Used for log messages that warns about some issue.
        /// </summary>
        Warning,

        /// <summary>
        /// Used for log messages that notifies about errors.
        /// </summary>
        Error,

        /// <summary>
        /// Used for log messages that notifies about errors that lead to the application failing.
        /// </summary>
        Critical,
    }

    /// <summary>
    /// Specifies constants for order directions.
    /// </summary>
    public enum Order
    {
        /// <summary>
        /// Undefined sort order.
        /// </summary>
        None,

        /// <summary>
        /// Ascending sort order.
        /// </summary>
        Ascending,

        /// <summary>
        /// Descending sort order.
        /// </summary>
        Descending
    }
}
