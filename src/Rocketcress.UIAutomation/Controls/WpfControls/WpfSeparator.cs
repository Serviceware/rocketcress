namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class WpfSeparator : WpfControl, IUITestSeparatorControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Separator);
    }
}
