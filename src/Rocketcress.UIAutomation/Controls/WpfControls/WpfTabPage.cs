namespace Rocketcress.UIAutomation.Controls.WpfControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfTabPage : WpfControl, IUITestTabPageControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.TabItem);

    public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();

    [UIMapControl]
    public virtual IUITestControl Header { get; protected set; } = InitUsing<UITestControl>(() => By.Empty);
}
