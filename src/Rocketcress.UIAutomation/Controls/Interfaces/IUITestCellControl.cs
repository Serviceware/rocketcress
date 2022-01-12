namespace Rocketcress.UIAutomation.Controls;

public interface IUITestCellControl : IUITestControl
{
    string Value { get; set; }
    int RowIndex { get; }
    int ColumnIndex { get; }
    bool Checked { get; set; }
}
