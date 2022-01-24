namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a radio button UIAutomation control.
/// </summary>
/// <seealso cref="IUITestControl" />
public interface IUITestRadioButtonControl : IUITestControl
{
    /// <summary>
    /// Gets the group in which this <see cref="IUITestRadioButtonControl"/> is in.
    /// </summary>
    IUITestControl Group { get; }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="IUITestRadioButtonControl"/> is selected.
    /// </summary>
    bool Selected { get; set; }
}
