namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a tree UIAutomation control.
/// </summary>
/// <seealso cref="IUITestControl" />
/// <seealso cref="IUITestContainerControl" />
public interface IUITestTreeControl : IUITestControl, IUITestContainerControl
{
    /// <summary>
    /// Gets the nodes.
    /// </summary>
    IEnumerable<IUITestControl> Nodes { get; }
}
