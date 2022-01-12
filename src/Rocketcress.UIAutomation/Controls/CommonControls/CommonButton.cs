using Rocketcress.Core.Extensions;
using System.Windows;

namespace Rocketcress.UIAutomation.Controls.CommonControls;

[AutoDetectControl(Priority = -50)]
[GenerateUIMapParts]
public partial class CommonButton : UITestControl, IUITestButtonControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Button);

    public InvokePattern InvokePattern => GetPattern<InvokePattern>();

    public override Point ClickablePoint => BoundingRectangle.GetAbsoluteCenter();

    public string DisplayText => Name;

    public void Invoke() => InvokePattern.Invoke();
}
