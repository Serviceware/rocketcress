using Rocketcress.UIAutomation.Common;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation table control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestTableControl{TRow, TCell}" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfTable : WpfControl, IUITestTableControl<WpfRow, WpfCell>
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.DataGrid);

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Base Location Key should be on top.")]
    private static readonly By ByRow = By.Framework(FrameworkIds.Wpf).AndControlType(ControlType.DataItem);
    private static readonly By ByCell = By
        .ChildOf(ByRow)
        .AndFramework(FrameworkIds.Wpf)
        .AndPatternAvailable<GridItemPattern>();

    /// <summary>
    /// Gets the scroll pattern.
    /// </summary>
    public ScrollPattern ScrollPattern => GetPattern<ScrollPattern>();

    /// <summary>
    /// Gets the selection pattern.
    /// </summary>
    public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();

    /// <summary>
    /// Gets the table pattern.
    /// </summary>
    public TablePattern TablePattern => GetPattern<TablePattern>();

    /// <summary>
    /// Gets the grid pattern.
    /// </summary>
    public GridPattern GridPattern => GetPattern<GridPattern>();

    /// <summary>
    /// Gets the item container pattern.
    /// </summary>
    public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();

    /// <inheritdoc/>
    public virtual bool CanSelectMultiple => SelectionPattern.Current.CanSelectMultiple;

    /// <inheritdoc/>
    public IEnumerable<WpfCell> Cells => ((IUITestTableControl)this).Cells.OfType<WpfCell>();

    /// <inheritdoc/>
    public virtual int ColumnCount => TablePattern.Current.ColumnCount;

    /// <inheritdoc/>
    public virtual IEnumerable<IUITestControl> ColumnHeaders => TablePattern.Current.GetColumnHeaders().Select(x => ControlUtility.GetControl(Application, x));

    /// <inheritdoc/>
    public virtual int RowCount => TablePattern.Current.RowCount;

    /// <inheritdoc/>
    public virtual IEnumerable<IUITestControl> RowHeaders => TablePattern.Current.GetRowHeaders().Select(x => ControlUtility.GetControl(Application, x));

    /// <inheritdoc/>
    public IEnumerable<WpfRow> Rows => ((IUITestTableControl)this).Rows.OfType<WpfRow>();

    /// <inheritdoc/>
    IEnumerable<IUITestControl> IUITestTableControl.Cells => FindElements(ByCell);

    /// <inheritdoc/>
    IEnumerable<IUITestControl> IUITestTableControl.Rows => FindElements(ByRow);

    /// <inheritdoc/>
    public virtual WpfRow this[int rowIndex] => GetRow(rowIndex);

    /// <inheritdoc/>
    IUITestControl IUITestTableControl.this[int rowIndex] => this[rowIndex];

    /// <inheritdoc/>
    public virtual WpfCell GetCell(int row, int column)
    {
        var element = TablePattern.GetItem(row, column);
        return element == null ? null : new WpfCell(Application, element);
    }

    /// <inheritdoc/>
    public virtual string[] GetColumnNames() => ColumnHeaders.Select(x => x.Name).ToArray();

    /// <inheritdoc/>
    public virtual WpfRow GetRow(int index) => new(Application, ByRow.AndSkip(index), this);

    /// <inheritdoc/>
    public virtual WpfCell FindFirstCellWithValue(string value)
    {
        var element = ItemContainerPattern.FindItemByProperty(null, ValuePattern.ValueProperty, value);
        return element == null ? null : new WpfCell(Application, element);
    }

    /// <inheritdoc/>
    IUITestControl IUITestTableControl.GetCell(int row, int column) => GetCell(row, column);

    /// <inheritdoc/>
    IUITestControl IUITestTableControl.GetRow(int index) => GetRow(index);

    /// <inheritdoc/>
    IUITestControl IUITestTableControl.FindFirstCellWithValue(string value) => FindFirstCellWithValue(value);
}
