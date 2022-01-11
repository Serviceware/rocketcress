namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class WinGroup : WinControl, IUITestGroupControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Group);
    }
}
