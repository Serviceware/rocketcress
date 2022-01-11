namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class WpfImage : WpfControl, IUITestImageControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Image);
    }
}
