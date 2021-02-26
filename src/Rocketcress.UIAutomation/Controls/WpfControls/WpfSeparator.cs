using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfSeparator : WpfControl, IUITestSeparatorControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Separator);

        #region Constructors
        public WpfSeparator(By locationKey)
            : base(locationKey)
        {
        }

        public WpfSeparator(IUITestControl parent)
            : base(parent)
        {
        }

        public WpfSeparator(AutomationElement element)
            : base(element)
        {
        }

        public WpfSeparator(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WpfSeparator(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WpfSeparator()
        {
        }
        #endregion
    }
}
