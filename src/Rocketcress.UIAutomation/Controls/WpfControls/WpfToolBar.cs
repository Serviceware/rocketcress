using Rocketcress.UIAutomation.Common;
using System.Collections.Generic;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfToolBar : WpfControl, IUITestToolBarControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ToolBar);

        #region Construcotrs
        public WpfToolBar(By locationKey) : base(locationKey) { }
        public WpfToolBar(IUITestControl parent) : base(parent) { }
        public WpfToolBar(AutomationElement element) : base(element) { }
        public WpfToolBar(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WpfToolBar(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WpfToolBar() { }
        #endregion

        #region Public Properties
        public By ItemLocationKey { get; set; } = By.Framework(FrameworkIds.Wpf).AndProperty(AutomationElement.IsKeyboardFocusableProperty, true);
        public virtual IEnumerable<IUITestControl> Items => FindElements(ItemLocationKey);
        #endregion
    }
}
