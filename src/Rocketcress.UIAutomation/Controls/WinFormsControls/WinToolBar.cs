using Rocketcress.UIAutomation.Common;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class WinToolBar : WinControl, IUITestToolBarControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ToolBar);

        public By ItemLocationKey { get; set; } = By.Framework(FrameworkIds.WindowsForms).AndProperty(AutomationElement.IsKeyboardFocusableProperty, true);
        public virtual IEnumerable<IUITestControl> Items => FindElements(ItemLocationKey);
    }
}
