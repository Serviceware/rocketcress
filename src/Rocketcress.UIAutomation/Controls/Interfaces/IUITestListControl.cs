namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a list UIAutomation control.
/// </summary>
/// <seealso cref="IUITestControl" />
/// <seealso cref="IUITestContainerControl" />
public interface IUITestListControl : IUITestControl, IUITestContainerControl
{
    /// <summary>
    /// Gets or sets the selected items.
    /// </summary>
    string[] SelectedItems { get; set; }

    /// <summary>
    /// Gets or sets the indices of selected items.
    /// </summary>
    int[] SelectedIndices { get; set; }

    /// <summary>
    /// Gets a value indicating whether multiple items can be selected in this <see cref="IUITestListControl"/>.
    /// </summary>
    bool IsMultiSelection { get; }
}

/// <summary>
/// Represents a list UIAutomation control.
/// </summary>
/// <typeparam name="TListItem">The type of list items.</typeparam>
/// <seealso cref="IUITestListControl" />
/// <seealso cref="IUITestContainerControl{TItem}" />
public interface IUITestListControl<TListItem> : IUITestListControl, IUITestContainerControl<TListItem>
    where TListItem : IUITestListItemControl
{
}
