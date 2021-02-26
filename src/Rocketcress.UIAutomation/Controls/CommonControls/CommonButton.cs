using Rocketcress.Core.Extensions;
using System.Windows;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.CommonControls
{
    [AutoDetectControl(Priority = -50)]
    public class CommonButton : UITestControl, IUITestButtonControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Button);

        public InvokePattern InvokePattern => GetPattern<InvokePattern>();

        public CommonButton(By locationKey)
            : base(locationKey)
        {
        }

        public CommonButton(IUITestControl parent)
            : base(parent)
        {
        }

        public CommonButton(AutomationElement element)
            : base(element)
        {
        }

        public CommonButton(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public CommonButton(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected CommonButton()
        {
        }

        public override Point ClickablePoint => BoundingRectangle.GetAbsoluteCenter();

        public string DisplayText => Name;

        public void Invoke() => InvokePattern.Invoke();
    }
}
