namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation row control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestRowControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfRow : WpfControl, IUITestRowControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.DataItem);

    /// <summary>
    /// Gets the invoke pattern.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public InvokePattern InvokePattern => GetPattern<InvokePattern>();

    /// <summary>
    /// Gets the item container pattern.
    /// </summary>
    public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();

    /// <summary>
    /// Gets the scroll item pattern.
    /// </summary>
    public ScrollItemPattern ScrollItemPattern => GetPattern<ScrollItemPattern>();

    /// <summary>
    /// Gets the selection item pattern.
    /// </summary>
    public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();

    /// <summary>
    /// Gets the selection pattern.
    /// </summary>
    public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();

    /// <inheritdoc/>
    public virtual bool CanSelectMultiple => SelectionPattern.Current.CanSelectMultiple;

    /// <inheritdoc/>
    public virtual IEnumerable<IUITestControl> Cells => FindElements(By.PatternAvailable<GridItemPattern>());

    /// <inheritdoc/>
    public virtual IUITestControl Header => FindElement(By.ControlType(ControlType.HeaderItem));

    /// <inheritdoc/>
    public virtual int RowIndex => Cells.FirstOrDefault()?.GetPattern<TableItemPattern>().Current.Row ?? -1;

    /// <inheritdoc/>
    public virtual bool Selected => SelectionItemPattern.Current.IsSelected;

    /// <inheritdoc/>
    public virtual IUITestControl this[int cellIndex] => GetCell(cellIndex);

    /// <inheritdoc/>
    public virtual IUITestControl GetCell(int index) => new UITestControl(Application, By.PatternAvailable<GridItemPattern>().AndSkip(index), this);
}
