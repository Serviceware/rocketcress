namespace Rocketcress.UIAutomation.Controls.WinFormsControls;

/// <summary>
/// Represents a Windows Forms tab page control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WinFormsControls.WinControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestTabPageControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WinTabPage : WinControl, IUITestTabPageControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.TabItem);

    /// <summary>
    /// Gets the selection item pattern.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();

    /// <inheritdoc/>
    IUITestControl IUITestTabPageControl.Header => throw new NotImplementedException();
}
