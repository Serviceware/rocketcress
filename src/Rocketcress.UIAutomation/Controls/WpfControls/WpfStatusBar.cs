using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfStatusBar : WpfControl, IUITestStatusBarControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.StatusBar);

        #region Constructors
        public WpfStatusBar(By locationKey)
            : base(locationKey)
        {
        }

        public WpfStatusBar(IUITestControl parent)
            : base(parent)
        {
        }

        public WpfStatusBar(AutomationElement element)
            : base(element)
        {
        }

        public WpfStatusBar(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WpfStatusBar(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WpfStatusBar()
        {
        }
        #endregion
    }
}
