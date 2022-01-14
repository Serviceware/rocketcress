namespace Rocketcress.UIAutomation.Controls.WinFormsControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WinTitleBar : WinControl, IUITestTitleBarControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.TitleBar);

    public ValuePattern ValuePattern => GetPattern<ValuePattern>();

    public virtual string DisplayText => ValuePattern.Current.Value;
}
