namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class WpfMenuBar : WpfControl, IUITestMenuBarControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.MenuBar);
    }
}
