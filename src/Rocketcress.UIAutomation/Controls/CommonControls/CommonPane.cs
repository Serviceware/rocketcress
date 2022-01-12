namespace Rocketcress.UIAutomation.Controls.CommonControls;

[AutoDetectControl(Priority = -50)]
[GenerateUIMapParts]
public partial class CommonPane : UITestControl, IUITestTextControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Pane);

    public virtual string DisplayText => Name;
    public virtual string Text => Name;
}
