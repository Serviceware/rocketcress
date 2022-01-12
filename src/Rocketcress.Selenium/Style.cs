using Rocketcress.Core.Extensions;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Rocketcress.Selenium;

/// <summary>
/// Represents a collection of css styles.
/// </summary>
public class Style
{
    private readonly bool _isComputedStyle;
    private readonly IDictionary<string, string> _dictionary;
    private readonly IWebElement _element;
    private readonly ColorConverter _colorConverter = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Style"/> class.
    /// </summary>
    /// <param name="styleText">The css text to parse.</param>
    public Style(string styleText)
    {
        _dictionary = Parse(styleText);
        _isComputedStyle = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Style"/> class.
    /// </summary>
    /// <param name="element">The DOM element to inspect.</param>
    public Style(IWebElement element)
    {
        _element = element;
        _isComputedStyle = true;
    }

    /// <summary>
    /// Gets the background color ("background-color") attribute value.
    /// </summary>
    public Color BackgroundColor => ConvertFromCssColor(this["background-color"] ?? "White");

    /// <summary>
    /// Gets the text color ("color") attribute value.
    /// </summary>
    public Color TextColor => ConvertFromCssColor(this["color"] ?? "Black");

    /// <summary>
    /// Gets a value indicating whether the text is Bold.
    /// </summary>
    public bool IsBold => this["font-weight"] == "bold";

    /// <summary>
    /// Determines wether a css attribute is set.
    /// </summary>
    /// <param name="key">The attribute to check.</param>
    /// <returns>True if the attribute is set; otherwise false.</returns>
    public bool ContainsKey(string key)
    {
        return _isComputedStyle || _dictionary.ContainsKey(key.ToLower());
    }

    /// <summary>
    /// Gets a CSS value.
    /// </summary>
    /// <param name="key">The name of the CSS attribute to get the value of.</param>
    /// <returns>The value of the given CSS attribute.</returns>
    public string this[string key]
    {
        get
        {
            if (_isComputedStyle)
                return _element.GetCssValue(key.ToLower());
            else
                return _dictionary.TryGetValue(key.ToLower());
        }
    }

    private static IDictionary<string, string> Parse(string styleText)
    {
        return styleText.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Split(':')).Where(x => x.Length == 2).ToDictionary(x => x[0].Trim().ToLower(), x => x[1].Trim());
    }

    private Color ConvertFromCssColor(string value)
    {
        Match match = Regex.Match(value, @"\A\s*rgb\s*\(\s*(?<r>[0-9]{1,3})\s*,\s*(?<g>[0-9]{1,3})\s*,\s*(?<b>[0-9]{1,3})\s*\)\s*\Z");
        if (match.Success)
        {
            return Color.FromArgb(
                int.Parse(match.Groups["r"].Value),
                int.Parse(match.Groups["g"].Value),
                int.Parse(match.Groups["b"].Value));
        }

        match = Regex.Match(value, @"\A\s*rgba\s*\(\s*(?<r>[0-9]{1,3})\s*,\s*(?<g>[0-9]{1,3})\s*,\s*(?<b>[0-9]{1,3})\s*,\s*(?<a>[0-9]{1,3})\s*\)\s*\Z");
        if (match.Success)
        {
            return Color.FromArgb(
                int.Parse(match.Groups["a"].Value),
                int.Parse(match.Groups["r"].Value),
                int.Parse(match.Groups["g"].Value),
                int.Parse(match.Groups["b"].Value));
        }

        match = Regex.Match(value, @"\A\s*hsl\s*\(\s*(?<h>[0-9]{1,3})\s*,\s*(?<s>[0-9]{1,3})\s*,\s*(?<l>[0-9]{1,3})\s*\)\s*\Z");
        if (match.Success)
            throw new NotSupportedException("HSL css format is currently not supported.");
        match = Regex.Match(value, @"\A\s*hsla\s*\(\s*(?<h>[0-9]{1,3})\s*,\s*(?<s>[0-9]{1,3})\s*,\s*(?<l>[0-9]{1,3})\s*,\s*(?<a>[0-9]{1,3})\s*\)\s*\Z");
        if (match.Success)
            throw new NotSupportedException("HSLA css format is currently not supported.");

        return (Color)_colorConverter.ConvertFromString(value);
    }
}
