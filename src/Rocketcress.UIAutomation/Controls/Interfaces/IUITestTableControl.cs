namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a table UIAutomation control.
/// </summary>
/// <seealso cref="IUITestControl" />
public interface IUITestTableControl : IUITestControl
{
    /// <summary>
    /// Gets a value indicating whether this <see cref="IUITestTableControl"/> allows selection of multiple rows.
    /// </summary>
    bool CanSelectMultiple { get; }

    /// <summary>
    /// Gets the cells.
    /// </summary>
    IEnumerable<IUITestControl> Cells { get; }

    /// <summary>
    /// Gets the column count.
    /// </summary>
    int ColumnCount { get; }

    /// <summary>
    /// Gets the column headers.
    /// </summary>
    IEnumerable<IUITestControl> ColumnHeaders { get; }

    /// <summary>
    /// Gets the row count.
    /// </summary>
    int RowCount { get; }

    /// <summary>
    /// Gets the row headers.
    /// </summary>
    IEnumerable<IUITestControl> RowHeaders { get; }

    /// <summary>
    /// Gets the rows.
    /// </summary>
    IEnumerable<IUITestControl> Rows { get; }

    /// <summary>
    /// Gets the row at the specified row index.
    /// </summary>
    /// <param name="rowIndex">Index of the row.</param>
    /// <returns>The row at index <paramref name="rowIndex"/>.</returns>
    IUITestControl this[int rowIndex] { get; }

    /// <summary>
    /// Gets the cell at the specified location.
    /// </summary>
    /// <param name="row">The row index of the cell.</param>
    /// <param name="column">The column index of the cell.</param>
    /// <returns>The cell that is in row index <paramref name="row"/> and in column index <paramref name="column"/>.</returns>
    IUITestControl GetCell(int row, int column);

    /// <summary>
    /// Gets the column names.
    /// </summary>
    /// <returns>An array of column names.</returns>
    string[] GetColumnNames();

    /// <summary>
    /// Gets the row at a specified index.
    /// </summary>
    /// <param name="index">The index of the row.</param>
    /// <returns>The row at row index <paramref name="index"/>.</returns>
    IUITestControl GetRow(int index);

    /// <summary>
    /// Finds the first cell with a specified value.
    /// </summary>
    /// <param name="value">The value to find.</param>
    /// <returns>The first cell that has <paramref name="value"/> as its value.</returns>
    IUITestControl FindFirstCellWithValue(string value);
}

/// <summary>
/// Represents a table UIAutomation control.
/// </summary>
/// <typeparam name="TRow">The type of the rows.</typeparam>
/// <typeparam name="TCell">The type of the cells.</typeparam>
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestControl" />
public interface IUITestTableControl<TRow, TCell> : IUITestTableControl
    where TRow : IUITestRowControl
    where TCell : IUITestCellControl
{
    /// <summary>
    /// Gets the cells.
    /// </summary>
    new IEnumerable<TCell> Cells { get; }

    /// <summary>
    /// Gets the rows.
    /// </summary>
    new IEnumerable<TRow> Rows { get; }

    /// <summary>
    /// Gets the row at the specified row index.
    /// </summary>
    /// <param name="rowIndex">Index of the row.</param>
    /// <returns>The row at index <paramref name="rowIndex"/>.</returns>
    new TRow this[int rowIndex] { get; }

    /// <summary>
    /// Gets the cell at the specified location.
    /// </summary>
    /// <param name="row">The row index of the cell.</param>
    /// <param name="column">The column index of the cell.</param>
    /// <returns>The cell that is in row index <paramref name="row"/> and in column index <paramref name="column"/>.</returns>
    new TCell GetCell(int row, int column);

    /// <summary>
    /// Gets the row at a specified index.
    /// </summary>
    /// <param name="index">The index of the row.</param>
    /// <returns>The row at row index <paramref name="index"/>.</returns>
    new TRow GetRow(int index);

    /// <summary>
    /// Finds the first cell with a specified value.
    /// </summary>
    /// <param name="value">The value to find.</param>
    /// <returns>The first cell that has <paramref name="value"/> as its value.</returns>
    new TCell FindFirstCellWithValue(string value);
}
