using Rocketcress.UIAutomation.Common;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation menu bar control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestMenuBarControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfMenuBar : WpfControl, IUITestMenuBarControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.MenuBar);

    /// <inheritdoc/>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public IEnumerable<IUITestControl> Items => FindElements(By.Framework(FrameworkIds.Wpf).AndProperty(AutomationElement.IsKeyboardFocusableProperty, true));
}
