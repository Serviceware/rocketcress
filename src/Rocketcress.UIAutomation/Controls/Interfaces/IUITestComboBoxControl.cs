namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a combo box UIAutomation control.
/// </summary>
/// <seealso cref="IUITestControl" />
/// <seealso cref="IUITestExpandableControl" />
public interface IUITestComboBoxControl : IUITestControl, IUITestExpandableControl
{
    /// <summary>
    /// Gets or sets the selected item.
    /// </summary>
    string SelectedItem { get; set; }

    /// <summary>
    /// Gets or sets the index of the selected item.
    /// </summary>
    int SelectedIndex { get; set; }

    /// <summary>
    /// Gets or sets the text that is displayed inside this <see cref="IUITestComboBoxControl"/>.
    /// </summary>
    string Text { get; set; }

    /// <summary>
    /// Gets the items of this <see cref="IUITestComboBoxControl"/>.
    /// </summary>
    IEnumerable<IUITestControl> Items { get; }
}
