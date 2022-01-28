using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation list control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestListControl{TItem}" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfList : WpfControl, IUITestListControl<WpfListItem>
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.List);

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Base Location Key should be on top.")]
    private static readonly By ByItem = By.Scope(TreeScope.Children).AndFramework(FrameworkIds.Wpf).AndControlType(ControlType.ListItem);

    private ListControlSupport _listControlSupport;

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
    public virtual string[] SelectedItems
    {
        get => _listControlSupport.GetSelectedItems().ToArray();
        set => _listControlSupport.SetSelectedItems(value);
    }

    /// <inheritdoc/>
    public virtual int[] SelectedIndices
    {
        get => _listControlSupport.GetSelectedIndices().ToArray();
        set => _listControlSupport.SetSelectedIndices(value);
    }

    /// <inheritdoc/>
    IEnumerable<IUITestControl> IUITestContainerControl.Items => _listControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

    /// <inheritdoc/>
    public virtual IEnumerable<WpfListItem> Items => ((IUITestContainerControl)this).Items.OfType<WpfListItem>();

    /// <inheritdoc/>
    public virtual bool IsMultiSelection => SelectionPattern.Current.CanSelectMultiple;

    partial void OnInitialized()
    {
        _listControlSupport = new ListControlSupport(this, ByItem);
    }
}
