using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using Rocketcress.UIAutomation.Exceptions;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation radio button control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestRadioButtonControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfRadioButton : WpfControl, IUITestRadioButtonControl, IValueAccessor
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.RadioButton);

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Base Location Key should be on top.")]
    private SelectionItemControlSupport _selectionItemControlSupport;

    /// <summary>
    /// Gets the selection item pattern.
    /// </summary>
    public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();

    /// <inheritdoc/>
    public virtual IUITestControl Group
    {
        get
        {
            var element = _selectionItemControlSupport.GetSelectionContainer();
            return element == null ? null : ControlUtility.GetControl(Application, element);
        }
    }

    /// <inheritdoc/>
    public virtual bool Selected
    {
        get => _selectionItemControlSupport.GetSelected();
        set
        {
            if (!value && Selected)
                throw new UIActionNotSupportedException("RadioButtons cannot be unchecked.", this);
            _selectionItemControlSupport.SetSelected(value);
        }
    }

    /// <inheritdoc/>
    public void SetValue(object value)
    {
        if (!(bool)value)
            throw new System.NotImplementedException();
        if (!Selected)
            Selected = true;
    }

    /// <inheritdoc/>
    public object GetValue()
    {
        return Selected;
    }

    partial void OnInitialized()
    {
        _selectionItemControlSupport = new SelectionItemControlSupport(this);
    }
}
