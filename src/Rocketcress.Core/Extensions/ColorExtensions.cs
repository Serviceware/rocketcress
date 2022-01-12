using System.Drawing;
using System.Globalization;

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
        return "#" + color.R.ToString("X2", CultureInfo.InvariantCulture) +
                     color.G.ToString("X2", CultureInfo.InvariantCulture) +
                     color.B.ToString("X2", CultureInfo.InvariantCulture);
    }
}
