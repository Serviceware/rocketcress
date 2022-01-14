namespace Rocketcress.UIAutomation.Controls.CommonControls;

[AutoDetectControl(Priority = -50)]
[GenerateUIMapParts]
public partial class CommonText : UITestControl, IUITestTextControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Text);

    public virtual string DisplayText => Name;
    public virtual string Text => Name;
}
