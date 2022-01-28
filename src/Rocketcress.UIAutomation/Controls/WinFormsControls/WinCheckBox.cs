using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Windows;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls;

/// <summary>
/// Represents a Windows Forms check box control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WinFormsControls.WinControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestCheckBoxControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WinCheckBox : WinControl, IUITestCheckBoxControl
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
    public virtual bool Indeterminate => TogglePattern.Current.ToggleState == ToggleState.Indeterminate;

    /// <summary>
    /// Gets or sets the content control.
    /// </summary>
    [UIMapControl(IdStyle = IdStyle.Disabled)]
    protected virtual UITestControl ContentControl { get; set; }

    partial void OnInitialized()
    {
        _toggleControlSupport = new ToggleControlSupport(this);
    }
}
