namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a list item UIAutomation control.
/// </summary>
/// <seealso cref="IUITestControl" />
/// <seealso cref="IUITestDisplayTextControl" />
public interface IUITestListItemControl : IUITestControl, IUITestDisplayTextControl
{
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="IUITestListItemControl"/> is selected.
    /// </summary>
    bool Selected { get; set; }

    /// <summary>
    /// Selects this <see cref="IUITestListItemControl"/>.
    /// </summary>
    void Select();
}
