using Rocketcress.UIAutomation.Common;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class WinMenuBar : WinControl, IUITestMenuBarControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.MenuBar);

        public virtual IEnumerable<IUITestControl> Items => FindElements(By.Framework(FrameworkIds.WindowsForms).AndProperty(AutomationElement.IsKeyboardFocusableProperty, true));
    }
}
