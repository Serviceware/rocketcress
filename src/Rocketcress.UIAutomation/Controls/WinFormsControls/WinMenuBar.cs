using Rocketcress.UIAutomation.Common;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls;

/// <summary>
/// Represents a Windows Forms menu bar control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WinFormsControls.WinControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestMenuBarControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WinMenuBar : WinControl, IUITestMenuBarControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.MenuBar);

    /// <inheritdoc/>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public virtual IEnumerable<IUITestControl> Items => FindElements(By.Framework(FrameworkIds.WindowsForms).AndProperty(AutomationElement.IsKeyboardFocusableProperty, true));
}
