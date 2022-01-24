namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a cell UIAutomation control.
/// </summary>
/// <seealso cref="IUITestControl" />
/// <seealso cref="IUITestCheckableControl" />
public interface IUITestCellControl : IUITestControl, IUITestCheckableControl
{
    /// <summary>
    /// Gets or sets the value of this <see cref="IUITestCellControl"/>.
    /// </summary>
    string Value { get; set; }

    /// <summary>
    /// Gets the index of the row this <see cref="IUITestCellControl"/> is in.
    /// </summary>
    int RowIndex { get; }

    /// <summary>
    /// Gets the index of the column this <see cref="IUITestCellControl"/> is in.
    /// </summary>
    int ColumnIndex { get; }
}
