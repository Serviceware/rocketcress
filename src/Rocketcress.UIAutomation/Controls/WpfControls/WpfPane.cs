using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfPane : WpfControl, IUITestPaneControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Pane);

        #region Patterns
        public ScrollPattern ScrollPattern => GetPattern<ScrollPattern>();
        #endregion

        #region Constructors
        public WpfPane(By locationKey) : base(locationKey) { }
        public WpfPane(IUITestControl parent) : base(parent) { }
        public WpfPane(AutomationElement element) : base(element) { }
        public WpfPane(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WpfPane(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WpfPane() { }
        #endregion
    }
}
