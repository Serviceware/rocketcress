using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    public class WinText : WinControl, IUITestTextControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Text);

        #region Construcotrs
        public WinText(By locationKey) : base(locationKey) { }
        public WinText(IUITestControl parent) : base(parent) { }
        public WinText(AutomationElement element) : base(element) { }
        public WinText(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WinText(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WinText() { }
        #endregion

        #region Public Properties
        public virtual string DisplayText => Name;
        public virtual string Text => Name;
        #endregion
    }
}
