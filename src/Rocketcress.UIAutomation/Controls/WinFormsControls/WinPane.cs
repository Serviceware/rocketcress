namespace Rocketcress.UIAutomation.Controls.WinFormsControls;

/// <summary>
/// Represents a Windows Forms pane control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WinFormsControls.WinControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestPaneControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WinPane : WinControl, IUITestPaneControl
{
    /// <inheritdoc />
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Pane);
}
