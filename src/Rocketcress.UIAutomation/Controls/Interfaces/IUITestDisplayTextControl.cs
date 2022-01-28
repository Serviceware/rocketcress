namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a UIAutomation control that provides a display text.
/// </summary>
/// <seealso cref="IUITestControl" />
public interface IUITestDisplayTextControl : IUITestControl
{
    /// <summary>
    /// Gets the display text.
    /// </summary>
    string DisplayText { get; }
}
