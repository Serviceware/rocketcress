namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a UIAutomation control that is expandable.
/// </summary>
/// <seealso cref="IUITestControl" />
public interface IUITestExpandableControl : IUITestControl
{
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="IUITestExpandableControl"/> is expanded.
    /// </summary>
    bool Expanded { get; set; }
}
