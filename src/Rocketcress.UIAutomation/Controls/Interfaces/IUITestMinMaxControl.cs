namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a UIAutomation control that has a position that is limited by minimum and maximum values.
/// </summary>
/// <seealso cref="IUITestControl" />
public interface IUITestMinMaxControl : IUITestControl
{
    /// <summary>
    /// Gets the maximum position.
    /// </summary>
    double MaximumPosition { get; }

    /// <summary>
    /// Gets the minimum position.
    /// </summary>
    double MinimumPosition { get; }

    /// <summary>
    /// Gets or sets the current position.
    /// </summary>
    double Position { get; set; }
}
