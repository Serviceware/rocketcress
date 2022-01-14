using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.CommonControls;

[AutoDetectControl(Priority = -50)]
[GenerateUIMapParts]
public partial class CommonMenu : UITestControl, IUITestMenuControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Menu);

    private static readonly By ByItem = By.ControlType(ControlType.MenuItem);
    public MenuControlSupport MenuControlSupport { get; private set; }
    public virtual IEnumerable<IUITestControl> Items => MenuControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

    partial void OnInitialized()
    {
        MenuControlSupport = new MenuControlSupport(this, ByItem, ControlType.Menu, ControlType.MenuItem);
    }
}
