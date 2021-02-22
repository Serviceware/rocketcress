using Rocketcress.UIAutomation.Exceptions;
using Rocketcress.Core;
using System.Text.RegularExpressions;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.ControlSupport
{
    public class ValueControlSupport
    {
        private readonly UITestControl _control;

        public ValueControlSupport(UITestControl control)
        {
            _control = control;
        }

        public void SetValue(string value)
        {
            if (value == null)
                value = string.Empty;
            if (!_control.TryGetPattern<ValuePattern>(out var valuePattern))
                throw new UIActionNotSupportedException("This control does not support setting a value (ValuePattern not available)", _control);
            if (!Waiter.WaitUntil(() => !valuePattern.Current.IsReadOnly))
                throw new UIAutomationControlException("This control is read only, so the value cannot be set.", _control);
            _control.SendKeys("^a{DEL}" + Regex.Replace(value, "[+^%~(){}]", "{$0}"));
            bool isPassord = _control.AutomationElement.Current.IsPassword;
            if (!isPassord && !Waiter.WaitUntil(() => value == valuePattern.Current.Value, UITestControl.ShortControlActionTimeout, 0))
            {
                _control.LogWarning($"The keys have not been send correctly to the control. The value is now set via the pattern.");
                valuePattern.SetValue(value);
                if (!isPassord && value != valuePattern.Current.Value)
                    throw new UIAutomationControlException($"The value has not been set correctly. (expected: <{value}>, actual: <{valuePattern.Current.Value}>)", _control);
            }
        }
    }
}
