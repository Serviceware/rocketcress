using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Windows;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

[AutoDetectControl]
[GenerateUIMapParts(IdStyle = IdStyle.Disabled)]
public partial class WpfCheckBox : WpfControl, IUITestCheckBoxControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.CheckBox);

    private ToggleControlSupport _toggleControlSupport;

    public TogglePattern TogglePattern => GetPattern<TogglePattern>();

    [UIMapControl]
    protected virtual UITestControl ContentControl { get; set; }

    public override Point ClickablePoint => ContentControl.Exists ? ContentControl.ClickablePoint : base.ClickablePoint;

    public virtual bool Checked
    {
        get => _toggleControlSupport.GetChecked();
        set => _toggleControlSupport.SetChecked(value);
    }

    partial void OnInitialized()
    {
        _toggleControlSupport = new ToggleControlSupport(this);
    }

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

    public object GetValue()
    {
        return Checked;
    }
}
