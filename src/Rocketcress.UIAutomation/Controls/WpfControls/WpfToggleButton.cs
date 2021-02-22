using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfToggleButton : WpfControl, IUITestToggleButtonControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey
            .AndControlType(ControlType.Button)
            .AndPatternAvailable<TogglePattern>();

        #region Private Fields
        private ToggleControlSupport _toggleControlSupport;
        #endregion

        #region Patterns
        public TogglePattern TogglePattern => GetPattern<TogglePattern>();
        #endregion

        #region Constructors
        public WpfToggleButton(By locationKey) : base(locationKey) { }
        public WpfToggleButton(IUITestControl parent) : base(parent) { }
        public WpfToggleButton(AutomationElement element) : base(element) { }
        public WpfToggleButton(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WpfToggleButton(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WpfToggleButton() { }

        protected override void Initialize()
        {
            base.Initialize();
            _toggleControlSupport = new ToggleControlSupport(this);
        }
        #endregion

        #region Public Properties
        public virtual string DisplayText => Name;
        public virtual bool Indeterminate => TogglePattern.Current.ToggleState == ToggleState.Indeterminate;
        public virtual bool Pressed
        {
            get => _toggleControlSupport.GetChecked();
            set => _toggleControlSupport.SetChecked(value);
        }
        #endregion
    }
}
