namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class WinTabPage : WinControl, IUITestTabPageControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.TabItem);

        public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();

        IUITestControl IUITestTabPageControl.Header => throw new NotImplementedException();
    }
}
