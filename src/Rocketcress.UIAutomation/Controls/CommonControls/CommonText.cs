using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.CommonControls
{
    [AutoDetectControl(Priority = -50)]
    public class CommonText : UITestControl, IUITestTextControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Text);

        #region Construcotrs
        public CommonText(By locationKey) : base(locationKey) { }
        public CommonText(IUITestControl parent) : base(parent) { }
        public CommonText(AutomationElement element) : base(element) { }
        public CommonText(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public CommonText(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected CommonText() { }
        #endregion

        #region Public Properties
        public virtual string DisplayText => Name;
        public virtual string Text => Name;
        #endregion
    }
}
