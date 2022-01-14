namespace Rocketcress.UIAutomation.Controls;

public interface IUITestRowControl : IUITestControl
{
    bool CanSelectMultiple { get; }
    IEnumerable<IUITestControl> Cells { get; }
    IUITestControl Header { get; }
    int RowIndex { get; }
    bool Selected { get; }
    IUITestControl GetCell(int index);

    IUITestControl this[int cellIndex] { get; }
}
