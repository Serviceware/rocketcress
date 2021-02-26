using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Windows;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    public class WinCheckBox : WinControl, IUITestCheckBoxControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.CheckBox);

        #region Private Fields
        private ToggleControlSupport _toggleControlSupport;
        #endregion

        #region Patterns
        public TogglePattern TogglePattern => GetPattern<TogglePattern>();
        #endregion

        #region Construcotrs
        public WinCheckBox(By locationKey)
            : base(locationKey)
        {
        }

        public WinCheckBox(IUITestControl parent)
            : base(parent)
        {
        }

        public WinCheckBox(AutomationElement element)
            : base(element)
        {
        }

        public WinCheckBox(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WinCheckBox(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WinCheckBox()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            _toggleControlSupport = new ToggleControlSupport(this);
            ContentControl = new UITestControl(By.Empty, this);
        }
        #endregion

        #region Properties
        protected virtual UITestControl ContentControl { get; set; }

        public override Point ClickablePoint => ContentControl.Exists ? ContentControl.ClickablePoint : base.ClickablePoint;

        public virtual bool Checked
        {
            get => _toggleControlSupport.GetChecked();
            set => _toggleControlSupport.SetChecked(value);
        }
        #endregion
    }
}
