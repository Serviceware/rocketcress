namespace Rocketcress.UIAutomation.Controls.WpfControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfRow : WpfControl, IUITestRowControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.DataItem);

    public InvokePattern InvokePattern => GetPattern<InvokePattern>();
    public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();
    public ScrollItemPattern ScrollItemPattern => GetPattern<ScrollItemPattern>();
    public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();
    public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();

    public virtual bool CanSelectMultiple => SelectionPattern.Current.CanSelectMultiple;
    public virtual IEnumerable<IUITestControl> Cells => FindElements(By.PatternAvailable<GridItemPattern>());
    public virtual IUITestControl Header => FindElement(By.ControlType(ControlType.HeaderItem));
    public virtual int RowIndex => Cells.FirstOrDefault()?.GetPattern<TableItemPattern>().Current.Row ?? -1;
    public virtual bool Selected => SelectionItemPattern.Current.IsSelected;
    public virtual IUITestControl GetCell(int index) => new UITestControl(Application, By.PatternAvailable<GridItemPattern>().AndSkip(index), this);

    public virtual IUITestControl this[int cellIndex] => GetCell(cellIndex);
}
