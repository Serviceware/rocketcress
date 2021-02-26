using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfText : WpfControl, IUITestTextControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Text);

        #region Construcotrs
        public WpfText(By locationKey)
            : base(locationKey)
        {
        }

        public WpfText(IUITestControl parent)
            : base(parent)
        {
        }

        public WpfText(AutomationElement element)
            : base(element)
        {
        }

        public WpfText(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WpfText(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WpfText()
        {
        }
        #endregion

        #region Public Properties
        public virtual string DisplayText => Name;
        public virtual string Text => Name;
        #endregion
    }
}
