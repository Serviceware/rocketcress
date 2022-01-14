namespace Rocketcress.UIAutomation.Controls.WpfControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfButton : WpfControl, IUITestButtonControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Button);

    public InvokePattern InvokePattern => GetPattern<InvokePattern>();

    public virtual string DisplayText => Name;

    public virtual void Invoke() => InvokePattern.Invoke();
}
