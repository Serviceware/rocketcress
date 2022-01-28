using Rocketcress.UIAutomation.Common;

namespace Rocketcress.UIAutomation.Controls.Win32Controls;

/// <summary>
/// Represents a Win32 control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.UITestControl" />
[AutoDetectControl(Priority = -100)]
[GenerateUIMapParts]
public partial class Win32Control : UITestControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndFramework(FrameworkIds.Win32);
}
