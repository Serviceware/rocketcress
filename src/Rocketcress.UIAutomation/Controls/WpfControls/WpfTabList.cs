using Rocketcress.Core.Extensions;
using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfTabList : WpfControl, IUITestTabListControl<WpfTabPage>
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Tab);

    private ListControlSupport _listControlSupport;

    public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();
    public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();

    protected virtual IEnumerable<IUITestControl> TabsInternal => _listControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

    public virtual int SelectedIndex
    {
        get => _listControlSupport.GetSelectedIndices().TryFirst(out var index) ? index : -1;
        set => _listControlSupport.SetSelectedIndex(value);
    }

    public IEnumerable<WpfTabPage> Tabs => TabsInternal.OfType<WpfTabPage>();

    IEnumerable<IUITestControl> IUITestTabListControl.Tabs => TabsInternal;

    partial void OnInitialized()
    {
        _listControlSupport = new ListControlSupport(this, By.Framework(FrameworkIds.Wpf).AndControlType(ControlType.TabItem));
    }
}
