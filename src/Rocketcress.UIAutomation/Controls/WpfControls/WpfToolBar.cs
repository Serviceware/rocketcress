using Rocketcress.UIAutomation.Common;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation tool bar control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestToolBarControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfToolBar : WpfControl, IUITestToolBarControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ToolBar);

    /// <summary>
    /// Gets or sets the location key that is used to find the items.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public By ItemLocationKey { get; set; } = By.Framework(FrameworkIds.Wpf).AndProperty(AutomationElement.IsKeyboardFocusableProperty, true);

    /// <inheritdoc/>
    public virtual IEnumerable<IUITestControl> Items => FindElements(ItemLocationKey);
}
