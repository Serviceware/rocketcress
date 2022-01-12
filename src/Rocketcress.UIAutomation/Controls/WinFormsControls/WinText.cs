namespace Rocketcress.UIAutomation.Controls.WinFormsControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WinText : WinControl, IUITestTextControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Text);

    public virtual string DisplayText => Name;
    public virtual string Text => Name;
}
