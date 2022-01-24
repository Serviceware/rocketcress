using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls;

/// <summary>
/// Represents a Windows Forms menu control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WinFormsControls.WinControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestMenuControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WinMenu : WinControl, IUITestMenuControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Menu);

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Base Location Key should be on top.")]
    private static readonly By ByItem = By.Framework(FrameworkIds.WindowsForms).AndControlType(ControlType.MenuItem);

    private MenuControlSupport _menuControlSupport;

    /// <inheritdoc/>
    public virtual IEnumerable<IUITestControl> Items => _menuControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

    partial void OnInitialized()
    {
        _menuControlSupport = new MenuControlSupport(this, ByItem, ControlType.Menu, ControlType.MenuItem);
    }
}
