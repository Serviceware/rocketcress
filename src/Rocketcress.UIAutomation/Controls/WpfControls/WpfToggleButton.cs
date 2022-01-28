using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation toggle button control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestToggleButtonControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfToggleButton : WpfControl, IUITestToggleButtonControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey
        .AndControlType(ControlType.Button)
        .AndPatternAvailable<TogglePattern>();

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Base Location Key should be on top.")]
    private ToggleControlSupport _toggleControlSupport;

    /// <summary>
    /// Gets the toggle pattern.
    /// </summary>
    public TogglePattern TogglePattern => GetPattern<TogglePattern>();

    /// <inheritdoc/>
    public virtual string DisplayText => Name;

    /// <inheritdoc/>
    public virtual bool Indeterminate => TogglePattern.Current.ToggleState == ToggleState.Indeterminate;

    /// <inheritdoc/>
    public virtual bool Checked
    {
        get => _toggleControlSupport.GetChecked();
        set => _toggleControlSupport.SetChecked(value);
    }

    partial void OnInitialized()
    {
        _toggleControlSupport = new ToggleControlSupport(this);
    }
}
