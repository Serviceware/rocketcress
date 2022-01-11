namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class WpfTitleBar : WpfControl, IUITestTitleBarControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.TitleBar);

        public ValuePattern ValuePattern => GetPattern<ValuePattern>();

        public virtual string DisplayText => ValuePattern.Current.Value;
    }
}
