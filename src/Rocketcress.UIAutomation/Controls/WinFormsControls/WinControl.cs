using Rocketcress.UIAutomation.Common;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl(Priority = -100)]
    [GenerateUIMapParts]
    public partial class WinControl : UITestControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndFramework(FrameworkIds.WindowsForms);
    }
}
