using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfGroup : WpfControl, IUITestGroupControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Group);

        #region Constructors
        public WpfGroup(By locationKey) : base(locationKey) { }
        public WpfGroup(IUITestControl parent) : base(parent) { }
        public WpfGroup(AutomationElement element) : base(element) { }
        public WpfGroup(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WpfGroup(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WpfGroup() { }
        #endregion
    }
}
