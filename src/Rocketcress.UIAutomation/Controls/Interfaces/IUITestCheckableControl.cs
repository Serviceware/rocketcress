namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a UIAutomation control that is checkable.
/// </summary>
/// <seealso cref="IUITestControl" />
public interface IUITestCheckableControl : IUITestControl
{
    /// <summary>
    /// Gets a value indicating whether this <see cref="IUITestCheckableControl"/> is in an indeterminate state.
    /// </summary>
    bool Indeterminate { get; }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="IUITestCheckableControl"/> is checked.
    /// </summary>
    bool Checked { get; set; }
}
