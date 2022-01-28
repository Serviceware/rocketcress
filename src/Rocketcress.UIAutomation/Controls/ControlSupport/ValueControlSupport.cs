using Rocketcress.Core;
using Rocketcress.UIAutomation.Exceptions;
using System.Text.RegularExpressions;

namespace Rocketcress.UIAutomation.Controls.ControlSupport;

/// <summary>
/// Contains code supporting control with the value pattern.
/// </summary>
public class ValueControlSupport
{
    private readonly UITestControl _control;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueControlSupport"/> class.
    /// </summary>
    /// <param name="control">The control to use.</param>
    public ValueControlSupport(UITestControl control)
    {
        _control = control;
    }

    /// <summary>
    /// Sets the value of the control.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <exception cref="Rocketcress.UIAutomation.Exceptions.UIActionNotSupportedException">This control does not support setting a value (ValuePattern not available).</exception>
    /// <exception cref="Rocketcress.UIAutomation.Exceptions.UIAutomationControlException">
    /// This control is read only, so the value cannot be set.
    /// or
    /// The value has not been set correctly. (expected: %lt;{value}&gt;, actual: &lt;{valuePattern.Current.Value}&gt;).
    /// </exception>
    public void SetValue(string value)
    {
        if (value == null)
            value = string.Empty;
        if (!_control.TryGetPattern<ValuePattern>(out var valuePattern))
            throw new UIActionNotSupportedException("This control does not support setting a value (ValuePattern not available).", _control);
        if (!Wait.Until(() => !valuePattern.Current.IsReadOnly).Start().Value)
            throw new UIAutomationControlException("This control is read only, so the value cannot be set.", _control);
        _control.SendKeys("^a{DEL}" + Regex.Replace(value, "[+^%~(){}]", "{$0}"));
        bool isPassord = _control.AutomationElement.Current.IsPassword;
        if (!isPassord && !Wait.Until(() => value == valuePattern.Current.Value).WithTimeout(UITestControl.ShortControlActionTimeout).WithTimeGap(0).Start().Value)
        {
            _control.LogWarning($"The keys have not been send correctly to the control. The value is now set via the pattern.");
            valuePattern.SetValue(value);
            if (!isPassord && value != valuePattern.Current.Value)
                throw new UIAutomationControlException($"The value has not been set correctly. (expected: <{value}>, actual: <{valuePattern.Current.Value}>).", _control);
        }
    }
}
