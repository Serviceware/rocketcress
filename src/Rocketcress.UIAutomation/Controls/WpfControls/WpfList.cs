using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfList : WpfControl, IUITestListControl<WpfListItem>
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.List);

    private static readonly By ByItem = By.Scope(TreeScope.Children).AndFramework(FrameworkIds.Wpf).AndControlType(ControlType.ListItem);

    private ListControlSupport _listControlSupport;

    public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();
    public ScrollPattern ScrollPattern => GetPattern<ScrollPattern>();
    public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();

    public virtual string[] SelectedItems
    {
        get => _listControlSupport.GetSelectedItems().ToArray();
        set => _listControlSupport.SetSelectedItems(value);
    }

    public virtual int[] SelectedIndices
    {
        get => _listControlSupport.GetSelectedIndices().ToArray();
        set => _listControlSupport.SetSelectedIndices(value);
    }

    IEnumerable<IUITestControl> IUITestListControl.Items => Items;
    public virtual IEnumerable<WpfListItem> Items => _listControlSupport.EnumerateItems().Select(x => new WpfListItem(Application, x));
    public virtual bool IsMultiSelection => SelectionPattern.Current.CanSelectMultiple;

    partial void OnInitialized()
    {
        _listControlSupport = new ListControlSupport(this, ByItem);
    }
}
