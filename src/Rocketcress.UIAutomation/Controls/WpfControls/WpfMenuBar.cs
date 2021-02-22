using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfMenuBar : WpfControl, IUITestMenuBarControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.MenuBar);

        public WpfMenuBar(By locationKey) : base(locationKey) { }
        public WpfMenuBar(IUITestControl parent) : base(parent) { }
        public WpfMenuBar(AutomationElement element) : base(element) { }
        public WpfMenuBar(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WpfMenuBar(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WpfMenuBar() { }
    }
}
