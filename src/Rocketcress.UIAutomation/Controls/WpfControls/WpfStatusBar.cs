namespace Rocketcress.UIAutomation.Controls.WpfControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfStatusBar : WpfControl, IUITestStatusBarControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.StatusBar);
}
