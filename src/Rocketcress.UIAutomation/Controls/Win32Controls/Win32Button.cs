using Rocketcress.Core.Extensions;
using System.Windows;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.Win32Controls
{
    [AutoDetectControl]
    public class Win32Button : Win32Control, IUITestButtonControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Button);

        public InvokePattern InvokePattern => GetPattern<InvokePattern>();

        public Win32Button(By locationKey) : base(locationKey) { }
        public Win32Button(IUITestControl parent) : base(parent) { }
        public Win32Button(AutomationElement element) : base(element) { }
        public Win32Button(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public Win32Button(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected Win32Button() { }

        public override Point ClickablePoint => BoundingRectangle.GetAbsoluteCenter();

        public string DisplayText => Name;

        public void Invoke() => InvokePattern.Invoke();
    }
}
