using Rocketcress.UIAutomation.Common;
using System.Collections.Generic;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    public class WinMenuBar : WinControl, IUITestMenuBarControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.MenuBar);

        public WinMenuBar(By locationKey) : base(locationKey) { }
        public WinMenuBar(IUITestControl parent) : base(parent) { }
        public WinMenuBar(AutomationElement element) : base(element) { }
        public WinMenuBar(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WinMenuBar(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WinMenuBar() { }

        #region Public Properties
        public virtual IEnumerable<IUITestControl> Items => FindElements(By.Framework(FrameworkIds.WindowsForms).AndProperty(AutomationElement.IsKeyboardFocusableProperty, true));
        #endregion
    }
}
