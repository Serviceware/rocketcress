using Rocketcress.UIAutomation.Exceptions;
using Rocketcress.Core;
using System;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.ControlSupport
{
    public class ToggleControlSupport
    {
        private readonly UITestControl _control;

        public ToggleControlSupport(UITestControl control)
        {
            _control = control;
        }

        public bool GetChecked()
        {
            if (!_control.TryGetPattern<TogglePattern>(out var togglePattern))
                throw new UIActionNotSupportedException("This control does not support toggling (TogglePattern not available)", _control);
            return GetChecked(togglePattern);
        }
        private bool GetChecked(TogglePattern togglePattern)
        {
            return togglePattern.Current.ToggleState == ToggleState.On;
        }

        public void SetChecked(bool value) => SetChecked(value, false);
        public void SetChecked(bool value, bool closePopup)
        {
            if (!_control.TryGetPattern<TogglePattern>(out var togglePattern))
                throw new UIActionNotSupportedException("This control does not support toggling (TogglePattern not available)", _control);
            if (GetChecked(togglePattern) != value)
            {
                _control.Click();
                if (!Waiter.WaitUntil(() => value == GetChecked(togglePattern), UITestControl.ShortControlActionTimeout, 0))
                {
                    _control.LogWarning($"Checked was not set from click on the control. State is now set with the automation pattern.");
                    togglePattern.Toggle();
                    if (value != GetChecked(togglePattern))
                        throw new Exception("Checked has not been correctly changed.");
                }
            }
        }
    }
}
