namespace Rocketcress.UIAutomation.Controls.WpfControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfListItem : WpfControl, IUITestListItemControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ListItem);

    public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();
    public ScrollItemPattern ScrollItemPattern => GetPattern<ScrollItemPattern>();
    public VirtualizedItemPattern VirtualizedItemPattern => GetPattern<VirtualizedItemPattern>();

    [UIMapControl]
    public WpfText InnerText { get; private set; } = InitUsing<WpfText>(() => By.ClassName("TextBlock"));

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

    public virtual string DisplayText => Name;
    public virtual string ActualDisplayName => InnerText.DisplayText;

    public virtual void Select()
    {
        if (TryGetPattern<VirtualizedItemPattern>(out var virtualizedItemPattern))
            virtualizedItemPattern.Realize();
        if (TryGetPattern<ScrollItemPattern>(out var scrollItemPattern))
            scrollItemPattern.ScrollIntoView();
        SelectionItemPattern.Select();
    }
}
