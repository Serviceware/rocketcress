namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation scroll bar control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestScrollBarControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfScrollBar : WpfControl, IUITestScrollBarControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ScrollBar);

    /// <summary>
    /// Gets the range value pattern.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public RangeValuePattern RangeValuePattern => GetPattern<RangeValuePattern>();

    /// <inheritdoc/>
    public virtual double MaximumPosition => RangeValuePattern.Current.Maximum;

    /// <inheritdoc/>
    public virtual double MinimumPosition => RangeValuePattern.Current.Minimum;

    /// <inheritdoc/>
    public virtual OrientationType Orientation => AutomationElement.Current.Orientation;

    /// <inheritdoc/>
    public virtual double Position
    {
        get => RangeValuePattern.Current.Value;
        set => RangeValuePattern.SetValue(value);
    }
}
