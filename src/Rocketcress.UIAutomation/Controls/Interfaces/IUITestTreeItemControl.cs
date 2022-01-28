namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a tree item UIAutomation control.
/// </summary>
/// <seealso cref="IUITestControl" />
/// <seealso cref="IUITestExpandableControl" />
/// <seealso cref="IUITestContainerControl" />
public interface IUITestTreeItemControl : IUITestControl, IUITestExpandableControl, IUITestContainerControl
{
    /// <summary>
    /// Gets a value indicating whether this <see cref="IUITestTreeItemControl"/> has child nodes.
    /// </summary>
    bool HasChildNodes { get; }

    /// <summary>
    /// Gets the header.
    /// </summary>
    string Header { get; }

    /// <summary>
    /// Gets the child nodes.
    /// </summary>
    IEnumerable<IUITestControl> Nodes { get; }

    /// <summary>
    /// Gets the parent node.
    /// </summary>
    IUITestControl ParentNode { get; }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="IUITestTreeItemControl"/> is selected.
    /// </summary>
    bool Selected { get; set; }
}
