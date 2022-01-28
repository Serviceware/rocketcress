namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation tab page control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestTabPageControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfTabPage : WpfControl, IUITestTabPageControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.TabItem);

    /// <summary>
    /// Gets the selection item pattern.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();

    /// <summary>
    /// Gets or sets the header control.
    /// </summary>
    [UIMapControl]
    public virtual IUITestControl Header { get; protected set; } = InitUsing<UITestControl>(() => By.Empty);
}
