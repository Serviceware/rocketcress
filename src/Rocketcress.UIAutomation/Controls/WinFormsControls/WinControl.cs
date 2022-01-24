using Rocketcress.UIAutomation.Common;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls;

/// <summary>
/// Represents a Windows Forms control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.UITestControl" />
[AutoDetectControl(Priority = -100)]
[GenerateUIMapParts]
public partial class WinControl : UITestControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndFramework(FrameworkIds.WindowsForms);
}
