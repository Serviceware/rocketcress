using System.Drawing;

namespace Rocketcress.Core.Extensions;

/// <summary>
/// Provides extension methods for the Color struct.
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// Generates a hex-string out of the color.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    /// <returns>Returns a string of the format #RRGGBB of the given color.</returns>
    public static string ToHexValue(this Color color)
    {
        return FormattableString.Invariant($"#{color.R:X2}{color.G:X2}{color.B:X2}");
    }
}
