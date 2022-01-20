namespace Rocketcress.UIAutomation.Controls.WpfControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfGroup : WpfControl, IUITestGroupControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Group);
}
