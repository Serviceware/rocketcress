using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfButton : WpfControl, IUITestButtonControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Button);

        #region Patterns
        public InvokePattern InvokePattern => GetPattern<InvokePattern>();
        #endregion

        #region Constructors
        public WpfButton(By locationKey) : base(locationKey) { }
        public WpfButton(IUITestControl parent) : base(parent) { }
        public WpfButton(AutomationElement element) : base(element) { }
        public WpfButton(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WpfButton(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WpfButton() { }
        #endregion

        #region Public Properties
        public virtual string DisplayText => Name;
        #endregion

        #region Public Methods
        public virtual void Invoke() => InvokePattern.Invoke();
        #endregion
    }
}
