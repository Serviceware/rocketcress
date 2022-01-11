using Rocketcress.Core.Extensions;
using System.Windows;

namespace Rocketcress.UIAutomation.Controls.Win32Controls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class Win32Button : Win32Control, IUITestButtonControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Button);

        public InvokePattern InvokePattern => GetPattern<InvokePattern>();

        public override Point ClickablePoint => BoundingRectangle.GetAbsoluteCenter();

        public string DisplayText => Name;

        public void Invoke() => InvokePattern.Invoke();
    }
}
