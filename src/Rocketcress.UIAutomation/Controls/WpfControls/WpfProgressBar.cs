namespace Rocketcress.UIAutomation.Controls.WpfControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfProgressBar : WpfControl, IUITestProgressBarControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ProgressBar);

    public RangeValuePattern RangeValuePattern => GetPattern<RangeValuePattern>();

    public virtual double MinimumValue => RangeValuePattern.Current.Minimum;
    public virtual double MaximumValue => RangeValuePattern.Current.Maximum;
    public virtual double Position => RangeValuePattern.Current.Value;
}
