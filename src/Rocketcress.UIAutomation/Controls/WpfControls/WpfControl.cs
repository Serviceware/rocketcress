using Rocketcress.UIAutomation.Common;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

[AutoDetectControl(Priority = -100)]
[GenerateUIMapParts]
public partial class WpfControl : UITestControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndFramework(FrameworkIds.Wpf);

    public SynchronizedInputPattern SynchronizedInputPattern => GetPattern<SynchronizedInputPattern>();
}
