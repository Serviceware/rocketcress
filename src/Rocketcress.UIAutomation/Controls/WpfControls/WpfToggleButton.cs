using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfToggleButton : WpfControl, IUITestToggleButtonControl
{
    protected override By BaseLocationKey => base.BaseLocationKey
        .AndControlType(ControlType.Button)
        .AndPatternAvailable<TogglePattern>();

    private ToggleControlSupport _toggleControlSupport;

    public TogglePattern TogglePattern => GetPattern<TogglePattern>();

    public virtual string DisplayText => Name;
    public virtual bool Indeterminate => TogglePattern.Current.ToggleState == ToggleState.Indeterminate;
    public virtual bool Pressed
    {
        get => _toggleControlSupport.GetChecked();
        set => _toggleControlSupport.SetChecked(value);
    }

    partial void OnInitialized()
    {
        _toggleControlSupport = new ToggleControlSupport(this);
    }
}
