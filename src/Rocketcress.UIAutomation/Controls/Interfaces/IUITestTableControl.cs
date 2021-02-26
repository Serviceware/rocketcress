using System.Collections.Generic;

namespace Rocketcress.UIAutomation.Controls
{
    public interface IUITestTableControl : IUITestControl
    {
        bool CanSelectMultiple { get; }
        IEnumerable<IUITestControl> Cells { get; }
        int ColumnCount { get; }
        IEnumerable<IUITestControl> ColumnHeaders { get; }
        int RowCount { get; }
        IEnumerable<IUITestControl> RowHeaders { get; }
        IEnumerable<IUITestControl> Rows { get; }

        IUITestControl GetCell(int row, int column);
        string[] GetColumnNames();
        IUITestControl GetRow(int index);
        IUITestControl FindFirstCellWithValue(string value);
        IUITestControl this[int rowIndex] { get; }
    }

    public interface IUITestTableControl<TRow, TCell> : IUITestTableControl
        where TRow : IUITestRowControl
        where TCell : IUITestCellControl
    {
        new IEnumerable<TCell> Cells { get; }
        new IEnumerable<TRow> Rows { get; }

        new TCell GetCell(int row, int column);
        new TRow GetRow(int index);
        new TCell FindFirstCellWithValue(string value);
        new TRow this[int rowIndex] { get; }
    }
}