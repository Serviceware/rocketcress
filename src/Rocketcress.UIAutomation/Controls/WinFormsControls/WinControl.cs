using Rocketcress.UIAutomation.Common;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl(Priority = -100)]
    public class WinControl : UITestControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndFramework(FrameworkIds.WindowsForms);

        public WinControl(By locationKey) : base(locationKey) { }
        public WinControl(IUITestControl parent) : base(parent) { }
        public WinControl(AutomationElement element) : base(element) { }
        public WinControl(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WinControl(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WinControl() { }
    }
}
