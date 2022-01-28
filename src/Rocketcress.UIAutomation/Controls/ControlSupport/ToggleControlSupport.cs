using Rocketcress.Core;
using Rocketcress.UIAutomation.Exceptions;

namespace Rocketcress.UIAutomation.Controls.ControlSupport;

/// <summary>
/// Contains supporting code for controls that have the toggle pattern.
/// </summary>
public class ToggleControlSupport
{
    private readonly UITestControl _control;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToggleControlSupport"/> class.
    /// </summary>
    /// <param name="control">The control to use.</param>
    public ToggleControlSupport(UITestControl control)
    {
        _control = control;
    }

    /// <summary>
    /// Gets a value indicating whether the current control is checked.
    /// </summary>
    /// <returns>A value indicating whether the current control is checked.</returns>
    /// <exception cref="Rocketcress.UIAutomation.Exceptions.UIActionNotSupportedException">This control does not support toggling (TogglePattern not available).</exception>
    public bool GetChecked()
    {
        if (!_control.TryGetPattern<TogglePattern>(out var togglePattern))
            throw new UIActionNotSupportedException("This control does not support toggling (TogglePattern not available).", _control);
        return GetChecked(togglePattern);
    }

    /// <summary>
    /// Sets a value indicating whether the current control is checked.
    /// </summary>
    /// <param name="value">If set to <c>true</c> the control is checked; otherwise unchecked.</param>
    /// <exception cref="Rocketcress.UIAutomation.Exceptions.UIActionNotSupportedException">This control does not support toggling (TogglePattern not available).</exception>
    /// <exception cref="System.Exception">Checked has not been correctly changed.</exception>
    public void SetChecked(bool value)
    {
        if (!_control.TryGetPattern<TogglePattern>(out var togglePattern))
            throw new UIActionNotSupportedException("This control does not support toggling (TogglePattern not available).", _control);
        if (GetChecked(togglePattern) != value)
        {
            _control.Click();
            if (!Wait.Until(() => value == GetChecked(togglePattern)).WithTimeout(UITestControl.ShortControlActionTimeout).WithTimeGap(0).Start().Value)
            {
                _control.LogWarning($"Checked was not set from click on the control. State is now set with the automation pattern.");
                togglePattern.Toggle();
                if (value != GetChecked(togglePattern))
                    throw new Exception("Checked has not been correctly changed.");
            }
        }
    }

    private bool GetChecked(TogglePattern togglePattern)
    {
        return togglePattern.Current.ToggleState == ToggleState.On;
    }
}
