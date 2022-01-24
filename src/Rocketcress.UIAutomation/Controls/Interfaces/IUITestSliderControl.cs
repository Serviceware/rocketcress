namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a slider control.
/// </summary>
/// <seealso cref="IUITestControl" />
/// <seealso cref="IUITestMinMaxControl" />
public interface IUITestSliderControl : IUITestControl, IUITestMinMaxControl
{
    /// <summary>
    /// Gets the orientation.
    /// </summary>
    OrientationType Orientation { get; }

    /// <summary>
    /// Gets the value added to or subtraced from the <see cref="IUITestMinMaxControl.Position"/> when executing the small change in UI.
    /// </summary>
    double SmallChange { get; }

    /// <summary>
    /// Gets the value added to or subtraced from the <see cref="IUITestMinMaxControl.Position"/> when executing the large change in UI.
    /// </summary>
    double LargeChange { get; }
}
