namespace Rocketcress.UIAutomation.Controls.WinFormsControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WinListItem : WinControl, IUITestListItemControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ListItem);

    public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();
    public ScrollItemPattern ScrollItemPattern => GetPattern<ScrollItemPattern>();
    public VirtualizedItemPattern VirtualizedItemPattern => GetPattern<VirtualizedItemPattern>();

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

    public virtual void Select()
    {
        if (TryGetPattern<VirtualizedItemPattern>(out var virtualizedItemPattern))
            virtualizedItemPattern.Realize();
        if (TryGetPattern<ScrollItemPattern>(out var scrollItemPattern))
            scrollItemPattern.ScrollIntoView();
        SelectionItemPattern.Select();
    }
}
