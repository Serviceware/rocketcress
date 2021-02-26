using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    public class WinGroup : WinControl, IUITestGroupControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Group);

        public WinGroup(By locationKey)
            : base(locationKey)
        {
        }

        public WinGroup(IUITestControl parent)
            : base(parent)
        {
        }

        public WinGroup(AutomationElement element)
            : base(element)
        {
        }

        public WinGroup(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WinGroup(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WinGroup()
        {
        }
    }
}
