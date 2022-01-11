namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class WinPane : WinControl, IUITestPaneControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Pane);
    }
}
