namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a tab page UIAutomation control.
/// </summary>
/// <seealso cref="IUITestControl" />
public interface IUITestTabPageControl : IUITestControl
{
    /// <summary>
    /// Gets the header.
    /// </summary>
    IUITestControl Header { get; }
}
