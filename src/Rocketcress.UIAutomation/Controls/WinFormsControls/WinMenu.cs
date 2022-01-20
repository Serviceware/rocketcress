using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WinMenu : WinControl, IUITestMenuControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Menu);

    private static readonly By ByItem = By.Framework(FrameworkIds.WindowsForms).AndControlType(ControlType.MenuItem);
    public MenuControlSupport MenuControlSupport { get; private set; }

    public virtual IEnumerable<IUITestControl> Items => MenuControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

    partial void OnInitialized()
    {
        MenuControlSupport = new MenuControlSupport(this, ByItem, ControlType.Menu, ControlType.MenuItem);
    }
}
