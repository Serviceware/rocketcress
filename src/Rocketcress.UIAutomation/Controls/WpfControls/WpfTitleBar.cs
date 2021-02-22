using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfTitleBar : WpfControl, IUITestTitleBarControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.TitleBar);

        #region Patterns
        public ValuePattern ValuePattern => GetPattern<ValuePattern>();
        #endregion

        #region Construcotrs
        public WpfTitleBar(By locationKey) : base(locationKey) { }
        public WpfTitleBar(IUITestControl parent) : base(parent) { }
        public WpfTitleBar(AutomationElement element) : base(element) { }
        public WpfTitleBar(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WpfTitleBar(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WpfTitleBar() { }
        #endregion

        #region Public Properties
        public virtual string DisplayText => ValuePattern.Current.Value;
        #endregion
    }
}
