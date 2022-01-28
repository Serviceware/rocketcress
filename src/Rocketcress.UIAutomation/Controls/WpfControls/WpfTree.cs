using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation tree control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestTreeControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfTree : WpfControl, IUITestTreeControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Tree);

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Base Location Key should be on top.")]
    private static readonly By ByItem = By.Framework(FrameworkIds.Wpf).AndControlType(ControlType.TreeItem);
    private MenuControlSupport _menuControlSupport;

    /// <summary>
    /// Gets the item container pattern.
    /// </summary>
    public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();

    /// <summary>
    /// Gets the scroll pattern.
    /// </summary>
    public ScrollPattern ScrollPattern => GetPattern<ScrollPattern>();

    /// <summary>
    /// Gets the selection pattern.
    /// </summary>
    public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();

    /// <inheritdoc/>
    public virtual IEnumerable<IUITestControl> Nodes => _menuControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

    /// <inheritdoc/>
    IEnumerable<IUITestControl> IUITestContainerControl.Items => Nodes;

    partial void OnInitialized()
    {
        _menuControlSupport = new MenuControlSupport(this, ByItem, ControlType.Tree, ControlType.TreeItem);
    }
}
