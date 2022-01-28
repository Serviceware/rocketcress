namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a row UIAutomation control.
/// </summary>
/// <seealso cref="IUITestControl" />
public interface IUITestRowControl : IUITestControl
{
    /// <summary>
    /// Gets a value indicating whether the user can select multiple cells in this <see cref="IUITestRowControl"/>.
    /// </summary>
    bool CanSelectMultiple { get; }

    /// <summary>
    /// Gets the cells of this <see cref="IUITestRowControl"/>.
    /// </summary>
    IEnumerable<IUITestControl> Cells { get; }

    /// <summary>
    /// Gets the header.
    /// </summary>
    IUITestControl Header { get; }

    /// <summary>
    /// Gets the index of the row.
    /// </summary>
    int RowIndex { get; }

    /// <summary>
    /// Gets a value indicating whether this <see cref="IUITestRowControl"/> is selected.
    /// </summary>
    bool Selected { get; }

    /// <summary>
    /// Gets the <see cref="IUITestControl"/> at the specified cell index.
    /// </summary>
    /// <param name="cellIndex">Index of the cell.</param>
    /// <returns>The <see cref="IUITestControl"/> at the specified cell index.</returns>
    IUITestControl this[int cellIndex] { get; }

    /// <summary>
    /// Gets the cell at a specified column index.
    /// </summary>
    /// <param name="index">The column index.</param>
    /// <returns>The cell inside this <see cref="IUITestRowControl"/> inside the column with index <paramref name="index"/>.</returns>
    IUITestControl GetCell(int index);
}
