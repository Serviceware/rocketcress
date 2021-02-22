using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    public class WinPane : WinControl, IUITestPaneControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Pane);

        public WinPane(By locationKey) : base(locationKey) { }
        public WinPane(IUITestControl parent) : base(parent) { }
        public WinPane(AutomationElement element) : base(element) { }
        public WinPane(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WinPane(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WinPane() { }
    }
}
