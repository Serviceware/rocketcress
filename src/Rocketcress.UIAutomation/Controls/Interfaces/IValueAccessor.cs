namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents an object that accesses a value.
/// </summary>
public interface IValueAccessor
{
    /// <summary>
    /// Sets the value.
    /// </summary>
    /// <param name="value">The value.</param>
    void SetValue(object value);

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <returns>The value.</returns>
    object GetValue();
}
