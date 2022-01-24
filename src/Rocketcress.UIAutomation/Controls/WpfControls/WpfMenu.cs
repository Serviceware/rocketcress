using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation menu control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfMenu : WpfControl, IUITestMenuControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Menu);

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Base Location Key should be on top.")]
    private static readonly By ByItem = By.Framework(FrameworkIds.Wpf).AndControlType(ControlType.MenuItem);

    /// <summary>
    /// Gets the menu control support.
    /// </summary>
    public MenuControlSupport MenuControlSupport { get; private set; }

    /// <inheritdoc/>
    public virtual IEnumerable<IUITestControl> Items => MenuControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

    partial void OnInitialized()
    {
        MenuControlSupport = new MenuControlSupport(this, ByItem, ControlType.Menu, ControlType.MenuItem);
    }
}
