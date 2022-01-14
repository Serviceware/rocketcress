using Rocketcress.UIAutomation.Common;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfToolBar : WpfControl, IUITestToolBarControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ToolBar);

    public By ItemLocationKey { get; set; } = By.Framework(FrameworkIds.Wpf).AndProperty(AutomationElement.IsKeyboardFocusableProperty, true);
    public virtual IEnumerable<IUITestControl> Items => FindElements(ItemLocationKey);
}
