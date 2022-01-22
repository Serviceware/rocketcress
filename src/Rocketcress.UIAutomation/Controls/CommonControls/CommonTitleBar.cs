namespace Rocketcress.UIAutomation.Controls.CommonControls;

/// <summary>
/// Represents a common title bar control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.UITestControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestTitleBarControl" />
[AutoDetectControl(Priority = -50)]
[GenerateUIMapParts]
public partial class CommonTitleBar : UITestControl, IUITestTitleBarControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.TitleBar);

    /// <summary>
    /// Gets the value pattern.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public ValuePattern ValuePattern => GetPattern<ValuePattern>();

    /// <inheritdoc/>
    public virtual string DisplayText => ValuePattern.Current.Value;
}
