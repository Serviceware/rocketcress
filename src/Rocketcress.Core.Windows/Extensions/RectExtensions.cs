using System.Windows;

namespace Rocketcress.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the Rect struct.
    /// </summary>
    public static class RectExtensions
    {
        /// <summary>
        /// Get the absolute center point of a rectangle.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>Returns the absolute center point of the rectangle.</returns>
        public static Point GetAbsoluteCenter(this Rect rect)
        {
            return new Point(rect.Left + (rect.Width / 2), rect.Top + (rect.Height / 2));
        }

        /// <summary>
        /// Gets the center point relative to the top left point of a rectangle.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>Returns the center point relative to the top left point of the rectangle.</returns>
        public static Point GetRelativeCenter(this Rect rect)
        {
            return new Point(rect.Width / 2, rect.Height / 2);
        }
    }
}
