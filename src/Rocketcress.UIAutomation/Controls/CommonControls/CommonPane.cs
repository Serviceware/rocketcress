using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.CommonControls
{
    [AutoDetectControl(Priority = -50)]
    public class CommonPane : UITestControl, IUITestTextControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Pane);

        #region Construcotrs
        public CommonPane(By locationKey) : base(locationKey) { }
        public CommonPane(IUITestControl parent) : base(parent) { }
        public CommonPane(AutomationElement element) : base(element) { }
        public CommonPane(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public CommonPane(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected CommonPane() { }
        #endregion

        #region Public Properties
        public virtual string DisplayText => Name;
        public virtual string Text => Name;
        #endregion
    }
}
