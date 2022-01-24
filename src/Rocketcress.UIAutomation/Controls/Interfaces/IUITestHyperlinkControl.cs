namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a hyperlink UIAutomation control.
/// </summary>
/// <seealso cref="IUITestControl" />
/// <seealso cref="IUITestInvokableControl" />
public interface IUITestHyperlinkControl : IUITestControl, IUITestInvokableControl
{
    /// <summary>
    /// Gets the alternative text of this <see cref="IUITestHyperlinkControl"/>.
    /// </summary>
    string Alt { get; }
}
