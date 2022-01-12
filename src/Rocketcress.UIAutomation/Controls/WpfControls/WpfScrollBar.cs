namespace Rocketcress.UIAutomation.Controls.WpfControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfScrollBar : WpfControl, IUITestScrollBarControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ScrollBar);

    public RangeValuePattern RangeValuePattern => GetPattern<RangeValuePattern>();

    public virtual double MaximumPosition => RangeValuePattern.Current.Maximum;
    public virtual double MinimumPosition => RangeValuePattern.Current.Minimum;
    public virtual OrientationType Orientation => AutomationElement.Current.Orientation;
    public virtual double Position
    {
        get => RangeValuePattern.Current.Value;
        set => RangeValuePattern.SetValue(value);
    }
}
