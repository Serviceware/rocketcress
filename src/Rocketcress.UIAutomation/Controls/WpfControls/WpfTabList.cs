using Rocketcress.Core.Extensions;
using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation tab list control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestTabListControl{TPage}" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfTabList : WpfControl, IUITestTabListControl<WpfTabPage>
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Tab);

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Base Location Key should be on top.")]
    private ListControlSupport _listControlSupport;

    /// <summary>
    /// Gets the item container pattern.
    /// </summary>
    public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();

    /// <summary>
    /// Gets the selection pattern.
    /// </summary>
    public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();

    /// <inheritdoc/>
    public virtual int SelectedIndex
    {
        get => _listControlSupport.GetSelectedIndices().TryFirst(out var index) ? index : -1;
        set => _listControlSupport.SetSelectedIndex(value);
    }

    /// <inheritdoc/>
    public IEnumerable<WpfTabPage> Tabs => ((IUITestContainerControl)this).Items.OfType<WpfTabPage>();

    /// <inheritdoc/>
    IEnumerable<IUITestControl> IUITestTabListControl.Tabs => ((IUITestContainerControl)this).Items;

    /// <inheritdoc/>
    public IEnumerable<WpfTabPage> Items => Tabs;

    /// <inheritdoc/>
    IEnumerable<IUITestControl> IUITestContainerControl.Items => _listControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

    partial void OnInitialized()
    {
        _listControlSupport = new ListControlSupport(this, By.Framework(FrameworkIds.Wpf).AndControlType(ControlType.TabItem));
    }
}
