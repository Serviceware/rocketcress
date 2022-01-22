using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.CommonControls;

/// <summary>
/// Represents a common menu control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.UITestControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestMenuControl" />
[AutoDetectControl(Priority = -50)]
[GenerateUIMapParts]
public partial class CommonMenu : UITestControl, IUITestMenuControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Menu);

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Base Location Key should be on top.")]
    private static readonly By ByItem = By.ControlType(ControlType.MenuItem);
    private MenuControlSupport _menuControlSupport;

    /// <inheritdoc/>
    public virtual IEnumerable<IUITestControl> Items => _menuControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

    partial void OnInitialized()
    {
        _menuControlSupport = new MenuControlSupport(this, ByItem, ControlType.Menu, ControlType.MenuItem);
    }
}
