using Rocketcress.UIAutomation.Controls.ControlSupport;
using System;
using System.Windows;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfCheckBox : WpfControl, IUITestCheckBoxControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.CheckBox);

        #region Private Fields
        private ToggleControlSupport _toggleControlSupport;
        #endregion

        #region Patterns
        public TogglePattern TogglePattern => GetPattern<TogglePattern>();
        #endregion

        #region Construcotrs
        public WpfCheckBox(By locationKey)
            : base(locationKey)
        {
        }

        public WpfCheckBox(IUITestControl parent)
            : base(parent)
        {
        }

        public WpfCheckBox(AutomationElement element)
            : base(element)
        {
        }

        public WpfCheckBox(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WpfCheckBox(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WpfCheckBox()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            _toggleControlSupport = new ToggleControlSupport(this);
            ContentControl = new UITestControl(By.Empty, this);
        }

        public void SetValue(object value)
        {
            if (value is bool)
                Checked = (bool)value;
            else if (value is int)
                Checked = (int)value != 0;
            else if (value is long)
                Checked = (long)value != 0;
            else if (value is string)
                Checked = bool.Parse((string)value);
            else
                throw new InvalidOperationException();
        }

        public object GetValue()
        {
            return Checked;
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
