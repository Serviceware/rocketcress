namespace Rocketcress.Selenium;

/// <summary>
/// Specifies constants for knwon browsers.
/// </summary>
[Flags]
public enum Browser : int
{
    /// <summary>
    /// Unknown browser.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Microsoft Internet Explorer.
    /// </summary>
    InternetExplorer = 1,

    /// <summary>
    /// Mozilla Firefox.
    /// </summary>
    Firefox = 2,

    /// <summary>
    /// Google Chrome.
    /// </summary>
    Chrome = 3 | Chromium,

    /// <summary>
    /// Microsoft Edge (Chromium).
    /// </summary>
    Edge = 4 | Chromium,

    /// <summary>
    /// Browsers based on Chromium or Chromium itself.
    /// </summary>
    Chromium = 0b1_0000_0000,
}
