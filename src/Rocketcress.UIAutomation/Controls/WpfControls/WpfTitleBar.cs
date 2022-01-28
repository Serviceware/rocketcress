namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation title bar control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestTitleBarControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfTitleBar : WpfControl, IUITestTitleBarControl
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
