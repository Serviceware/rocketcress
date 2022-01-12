using Rocketcress.UIAutomation.Common;

namespace Rocketcress.UIAutomation.Controls.Win32Controls;

[AutoDetectControl(Priority = -100)]
[GenerateUIMapParts]
public partial class Win32Control : UITestControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndFramework(FrameworkIds.Win32);
}
