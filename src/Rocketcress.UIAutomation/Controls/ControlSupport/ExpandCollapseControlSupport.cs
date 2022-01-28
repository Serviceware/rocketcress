using Rocketcress.Core;
using Rocketcress.UIAutomation.Exceptions;

namespace Rocketcress.UIAutomation.Controls.ControlSupport;

/// <summary>
/// Contains supporting code for controls that have the expand/collapse pattern.
/// </summary>
public class ExpandCollapseControlSupport
{
    private readonly UITestControl _control;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpandCollapseControlSupport"/> class.
    /// </summary>
    /// <param name="control">The control to use.</param>
    public ExpandCollapseControlSupport(UITestControl control)
    {
        _control = control;
    }

    /// <summary>
    /// Gets a value indicating whether the control is expanded.
    /// </summary>
    /// <returns>A value indicating whether the control is expanded.</returns>
    /// <exception cref="Rocketcress.UIAutomation.Exceptions.UIActionNotSupportedException">This control does not support expanding/collapsing (ExpandCollapsePattern not available).</exception>
    public bool GetExpanded()
    {
        if (!_control.TryGetPattern<ExpandCollapsePattern>(out var expandCollapsePattern))
            throw new UIActionNotSupportedException("This control does not support expanding/collapsing (ExpandCollapsePattern not available)", _control);
        return GetExpanded(expandCollapsePattern);
    }

    /// <summary>
    /// Sets a value indicating whether the control is expanded.
    /// </summary>
    /// <param name="value">If set to <c>true</c> the control will be expanded; otherwise it is collapsed.</param>
    /// <exception cref="Rocketcress.UIAutomation.Exceptions.UIActionNotSupportedException">This control does not support expanding/collapsing (ExpandCollapsePattern not available).</exception>
    /// <exception cref="Rocketcress.UIAutomation.Exceptions.UIActionFailedException">GetExpanded has not been correctly changed.</exception>
    public void SetExpanded(bool value)
    {
        if (!_control.TryGetPattern<ExpandCollapsePattern>(out var expandCollapsePattern))
            throw new UIActionNotSupportedException("This control does not support expanding/collapsing (ExpandCollapsePattern not available)", _control);
        if (GetExpanded(expandCollapsePattern) != value)
        {
            _control.Click();
            if (!Wait.Until(() => value == GetExpanded(expandCollapsePattern)).WithTimeout(UITestControl.ShortControlActionTimeout).WithTimeGap(0).Start().Value)
            {
                _control.LogWarning($"Expanded was not set from click on the control. State is now set with the automation pattern.");
                (value ? (Action)expandCollapsePattern.Expand : expandCollapsePattern.Collapse).Invoke();
                if (value != GetExpanded(expandCollapsePattern))
                    throw new UIActionFailedException("GetExpanded has not been correctly changed.");
            }
        }
    }

    private bool GetExpanded(ExpandCollapsePattern expandCollapsePattern)
    {
        return expandCollapsePattern.Current.ExpandCollapseState == ExpandCollapseState.Expanded;
    }
}
