namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation status bar control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestStatusBarControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfStatusBar : WpfControl, IUITestStatusBarControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.StatusBar);
}
