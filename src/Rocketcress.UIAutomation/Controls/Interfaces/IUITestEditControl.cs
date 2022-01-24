namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a edit UIAutomation control.
/// </summary>
/// <seealso cref="IUITestControl" />
public interface IUITestEditControl : IUITestControl
{
    /// <summary>
    /// Gets or sets the text of this <see cref="IUITestEditControl"/>.
    /// </summary>
    string Text { get; set; }

    /// <summary>
    /// Gets a value indicating whether this <see cref="IUITestEditControl"/> is read only.
    /// </summary>
    bool ReadOnly { get; }
}
