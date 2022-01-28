namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation progress bar control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestProgressBarControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfProgressBar : WpfControl, IUITestProgressBarControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ProgressBar);

    /// <summary>
    /// Gets the range value pattern.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public RangeValuePattern RangeValuePattern => GetPattern<RangeValuePattern>();

    /// <inheritdoc/>
    public virtual double MinimumPosition => RangeValuePattern.Current.Minimum;

    /// <inheritdoc/>
    public virtual double MaximumPosition => RangeValuePattern.Current.Maximum;

    /// <summary>
    /// Gets the position.
    /// </summary>
    public virtual double Position => RangeValuePattern.Current.Value;

    /// <inheritdoc/>
    double IUITestMinMaxControl.Position
    {
        get => Position;
        set => throw new NotSupportedException();
    }
}
