namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a scroll bar UIAutomation control.
/// </summary>
/// <seealso cref="IUITestControl" />
/// <seealso cref="IUITestMinMaxControl" />
public interface IUITestScrollBarControl : IUITestControl, IUITestMinMaxControl
{
    /// <summary>
    /// Gets the orientation.
    /// </summary>
    OrientationType Orientation { get; }
}
