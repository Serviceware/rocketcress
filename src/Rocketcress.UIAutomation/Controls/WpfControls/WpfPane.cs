namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class WpfPane : WpfControl, IUITestPaneControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Pane);

        public ScrollPattern ScrollPattern => GetPattern<ScrollPattern>();
    }
}
