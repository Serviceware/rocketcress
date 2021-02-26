using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfCustom : WpfControl, IUITestCustomControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Custom);

        #region Constructors
        public WpfCustom(By locationKey)
            : base(locationKey)
        {
        }

        public WpfCustom(IUITestControl parent)
            : base(parent)
        {
        }

        public WpfCustom(AutomationElement element)
            : base(element)
        {
        }

        public WpfCustom(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WpfCustom(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WpfCustom()
        {
        }
        #endregion
    }
}
