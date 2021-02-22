using Rocketcress.Core.Extensions;
using System.Windows;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    public class WinButton : WinControl, IUITestButtonControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Button);

        public InvokePattern InvokePattern => GetPattern<InvokePattern>();

        public WinButton(By locationKey) : base(locationKey) { }
        public WinButton(IUITestControl parent) : base(parent) { }
        public WinButton(AutomationElement element) : base(element) { }
        public WinButton(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WinButton(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WinButton() { }

        public override Point ClickablePoint => BoundingRectangle.GetAbsoluteCenter();

        public string DisplayText => Name;

        public void Invoke() => InvokePattern.Invoke();
    }
}
