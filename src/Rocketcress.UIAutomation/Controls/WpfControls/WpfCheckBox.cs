using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Windows;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation check box control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestCheckBoxControl" />
[AutoDetectControl]
[GenerateUIMapParts(IdStyle = IdStyle.Disabled)]
public partial class WpfCheckBox : WpfControl, IUITestCheckBoxControl, IValueAccessor
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.CheckBox);

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Base Location Key should be on top.")]
    private ToggleControlSupport _toggleControlSupport;

    /// <summary>
    /// Gets the toggle pattern.
    /// </summary>
    public TogglePattern TogglePattern => GetPattern<TogglePattern>();

    /// <inheritdoc/>
    public override Point ClickablePoint => ContentControl.Exists ? ContentControl.ClickablePoint : base.ClickablePoint;

    /// <inheritdoc/>
    public virtual bool Checked
    {
        get => _toggleControlSupport.GetChecked();
        set => _toggleControlSupport.SetChecked(value);
    }

    /// <inheritdoc/>
    public bool Indeterminate => TogglePattern.Current.ToggleState == ToggleState.Indeterminate;

    /// <summary>
    /// Gets or sets the content control.
    /// </summary>
    [UIMapControl]
    protected virtual UITestControl ContentControl { get; set; }

    /// <inheritdoc/>
    public void SetValue(object value)
    {
        if (value is bool boolean)
            Checked = boolean;
        else if (value is int @int)
            Checked = @int != 0;
        else if (value is long @long)
            Checked = @long != 0;
        else if (value is string @string)
            Checked = bool.Parse(@string);
        else
            throw new InvalidOperationException();
    }

    /// <inheritdoc/>
    public object GetValue()
    {
        return Checked;
    }

    partial void OnInitialized()
    {
        _toggleControlSupport = new ToggleControlSupport(this);
    }
}
