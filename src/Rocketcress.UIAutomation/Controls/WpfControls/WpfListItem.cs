namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation list item control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestListItemControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfListItem : WpfControl, IUITestListItemControl
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

    /// <summary>
    /// Gets the inner text control.
    /// </summary>
    [UIMapControl]
    public WpfText InnerText { get; private set; } = InitUsing<WpfText>(() => By.ClassName("TextBlock"));

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

    /// <summary>
    /// Gets the actual display name.
    /// </summary>
    public virtual string ActualDisplayName => InnerText.DisplayText;

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
