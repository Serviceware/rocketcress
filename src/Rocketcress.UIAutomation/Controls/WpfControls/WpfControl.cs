using Rocketcress.UIAutomation.Common;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl(Priority = -100)]
    public class WpfControl : UITestControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndFramework(FrameworkIds.Wpf);

        #region Patterns
        public SynchronizedInputPattern SynchronizedInputPattern => GetPattern<SynchronizedInputPattern>();
        #endregion

        #region Constructors
        public WpfControl(By locationKey) : base(locationKey) { }
        public WpfControl(IUITestControl parent) : base(parent) { }
        public WpfControl(AutomationElement element) : base(element) { }
        public WpfControl(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WpfControl(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WpfControl() { }
        #endregion
    }
}
