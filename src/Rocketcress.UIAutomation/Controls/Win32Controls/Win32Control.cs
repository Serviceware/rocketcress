using Rocketcress.UIAutomation.Common;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.Win32Controls
{
    [AutoDetectControl(Priority = -100)]
    public class Win32Control : UITestControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndFramework(FrameworkIds.Win32);

        public Win32Control(By locationKey)
            : base(locationKey)
        {
        }

        public Win32Control(IUITestControl parent)
            : base(parent)
        {
        }

        public Win32Control(AutomationElement element)
            : base(element)
        {
        }

        public Win32Control(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public Win32Control(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected Win32Control()
        {
        }
    }
}
