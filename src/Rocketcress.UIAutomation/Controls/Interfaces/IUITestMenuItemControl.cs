namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a menu item UIAutomation control.
/// </summary>
/// <seealso cref="IUITestControl" />
/// <seealso cref="IUITestExpandableControl" />
/// <seealso cref="IUITestCheckableControl" />
/// <seealso cref="IUITestContainerControl" />
public interface IUITestMenuItemControl : IUITestControl, IUITestExpandableControl, IUITestCheckableControl, IUITestContainerControl
{
    /// <summary>
    /// Gets the header text.
    /// </summary>
    string Header { get; }
}
