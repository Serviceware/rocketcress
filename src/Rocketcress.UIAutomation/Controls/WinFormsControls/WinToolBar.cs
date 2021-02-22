using Rocketcress.UIAutomation.Common;
using System.Collections.Generic;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    public class WinToolBar : WinControl, IUITestToolBarControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ToolBar);

        #region Construcotrs
        public WinToolBar(By locationKey) : base(locationKey) { }
        public WinToolBar(IUITestControl parent) : base(parent) { }
        public WinToolBar(AutomationElement element) : base(element) { }
        public WinToolBar(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WinToolBar(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WinToolBar() { }
        #endregion

        #region Public Properties
        public By ItemLocationKey { get; set; } = By.Framework(FrameworkIds.WindowsForms).AndProperty(AutomationElement.IsKeyboardFocusableProperty, true);
        public virtual IEnumerable<IUITestControl> Items => FindElements(ItemLocationKey);
        #endregion
    }
}
