using Rocketcress.Core;
using Rocketcress.UIAutomation.Exceptions;

namespace Rocketcress.UIAutomation.Controls.ControlSupport;

/// <summary>
/// Contains supporting code for controls with the selection item pattern.
/// </summary>
public class SelectionItemControlSupport
{
    private readonly UITestControl _control;

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectionItemControlSupport"/> class.
    /// </summary>
    /// <param name="control">The control to use.</param>
    public SelectionItemControlSupport(UITestControl control)
    {
        _control = control;
    }

    /// <summary>
    /// Gets a value indicating whether the current control is selected.
    /// </summary>
    /// <returns>A value indicating whether the current control is selected.</returns>
    public bool GetSelected() => GetSelected(GetSelectionItemPattern());

    /// <summary>
    /// Gets a value indicating whether the current control is selected.
    /// </summary>
    /// <param name="selectionItemPattern">The pattern to get the value from.</param>
    /// <returns>A value indicating whether the current control is selected.</returns>
    public bool GetSelected(SelectionItemPattern selectionItemPattern)
    {
        Guard.NotNull(selectionItemPattern);
        return selectionItemPattern.Current.IsSelected;
    }

    /// <summary>
    /// Sets a value indicating whether the current control is selected.
    /// </summary>
    /// <param name="value">If <c>true</c> the current control will be selected; otherwise it will be deselected.</param>
    /// <exception cref="UIActionFailedException">Selected has not been set correctly.</exception>
    public void SetSelected(bool value)
    {
        var selectionItemPattern = GetSelectionItemPattern();

        if (value != GetSelected(selectionItemPattern))
        {
            _control.Click();
            if (!Wait.Until(() => value == GetSelected(selectionItemPattern)).WithTimeout(UITestControl.ShortControlActionTimeout).WithTimeGap(0).Start().Value)
            {
                _control.LogWarning("Selected could not be set via control click. Setting property via pattern now.");
                selectionItemPattern.Select();
                if (!value)
                    selectionItemPattern.RemoveFromSelection();
                if (value != GetSelected(selectionItemPattern))
                    throw new UIActionFailedException("Selected has not been set correctly.", _control);
            }
        }
    }

    /// <summary>
    /// Gets the <see cref="AutomationElement"/> that supports the <see cref="SelectionPattern"/> control pattern and acts as the container for the calling object.
    /// </summary>
    /// <returns>The <see cref="AutomationElement"/> that supports the <see cref="SelectionPattern"/> control pattern and acts as the container for the calling object.</returns>
    public AutomationElement GetSelectionContainer()
    {
        return GetSelectionItemPattern().Current.SelectionContainer;
    }

    private SelectionItemPattern GetSelectionItemPattern()
    {
        if (!_control.TryGetPattern<SelectionItemPattern>(out var selectionItemPattern))
            throw new UIActionNotSupportedException("The control needs to support the SelectionItemPattern.", _control);
        return selectionItemPattern;
    }
}
