using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfImage : WpfControl, IUITestImageControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Image);

        #region Constructors
        public WpfImage(By locationKey)
            : base(locationKey)
        {
        }

        public WpfImage(IUITestControl parent)
            : base(parent)
        {
        }

        public WpfImage(AutomationElement element)
            : base(element)
        {
        }

        public WpfImage(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WpfImage(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WpfImage()
        {
        }
        #endregion
    }
}
