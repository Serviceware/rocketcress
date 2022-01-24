namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation pane control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestPaneControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfPane : WpfControl, IUITestPaneControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Pane);

    /// <summary>
    /// Gets the scroll pattern.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public ScrollPattern ScrollPattern => GetPattern<ScrollPattern>();
}
