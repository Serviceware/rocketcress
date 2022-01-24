using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls;

/// <summary>
/// Represents a Windows Forms list control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WinFormsControls.WinControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestListControl{TITem}" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WinList : WinControl, IUITestListControl<WinListItem>
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.List);

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Base Location Key should be on top.")]
    private static readonly By ByItem = By.Scope(TreeScope.Children).AndFramework(FrameworkIds.WindowsForms).AndControlType(ControlType.ListItem);
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

    /// <summary>
    /// Gets the synchronized input pattern.
    /// </summary>
    public SynchronizedInputPattern SynchronizedInputPattern => GetPattern<SynchronizedInputPattern>();

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
    public virtual IEnumerable<WinListItem> Items => ((IUITestContainerControl)this).Items.OfType<WinListItem>();

    /// <summary>
    /// Gets the native items.
    /// </summary>
    public virtual IEnumerable<AutomationElement> NativeItems => _listControlSupport.EnumerateItems();

    /// <inheritdoc/>
    public virtual bool IsMultiSelection => SelectionPattern.Current.CanSelectMultiple;

    /// <inheritdoc/>
    IEnumerable<IUITestControl> IUITestContainerControl.Items => _listControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

    partial void OnInitialized()
    {
        _listControlSupport = new ListControlSupport(this, ByItem);
    }
}
