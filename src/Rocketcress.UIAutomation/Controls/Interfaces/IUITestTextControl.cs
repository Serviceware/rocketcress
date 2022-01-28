namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a text UIAutomation control.
/// </summary>
/// <seealso cref="IUITestControl" />
/// <seealso cref="IUITestDisplayTextControl" />
public interface IUITestTextControl : IUITestControl, IUITestDisplayTextControl
{
    /// <summary>
    /// Gets the text.
    /// </summary>
    string Text { get; }
}
