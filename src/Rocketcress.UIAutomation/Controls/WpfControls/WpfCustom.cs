namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class WpfCustom : WpfControl, IUITestCustomControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Custom);
    }
}
