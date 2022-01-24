namespace Rocketcress.UIAutomation.Controls.WinFormsControls;

/// <summary>
/// Represents a Windows Forms list item control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WinFormsControls.WinControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestListItemControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WinListItem : WinControl, IUITestListItemControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ListItem);

    /// <summary>
    /// Gets the selection item pattern.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();

    /// <summary>
    /// Gets the scroll item pattern.
    /// </summary>
    public ScrollItemPattern ScrollItemPattern => GetPattern<ScrollItemPattern>();

    /// <summary>
    /// Gets the virtualized item pattern.
    /// </summary>
    public VirtualizedItemPattern VirtualizedItemPattern => GetPattern<VirtualizedItemPattern>();

    /// <inheritdoc/>
    public virtual bool Selected
    {
        get => SelectionItemPattern.Current.IsSelected;
        set
        {
            if (value)
                SelectionItemPattern.AddToSelection();
            else
                SelectionItemPattern.RemoveFromSelection();
        }
    }

    /// <inheritdoc/>
    public virtual string DisplayText => Name;

    /// <inheritdoc/>
    public virtual void Select()
    {
        if (TryGetPattern<VirtualizedItemPattern>(out var virtualizedItemPattern))
            virtualizedItemPattern.Realize();
        if (TryGetPattern<ScrollItemPattern>(out var scrollItemPattern))
            scrollItemPattern.ScrollIntoView();
        SelectionItemPattern.Select();
    }
}
