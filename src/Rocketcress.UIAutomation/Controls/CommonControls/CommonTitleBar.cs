namespace Rocketcress.UIAutomation.Controls.CommonControls
{
    [AutoDetectControl(Priority = -50)]
    [GenerateUIMapParts]
    public partial class CommonTitleBar : UITestControl, IUITestTitleBarControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.TitleBar);

        public ValuePattern ValuePattern => GetPattern<ValuePattern>();

        public virtual string DisplayText => ValuePattern.Current.Value;
    }
}
