namespace Rocketcress.Core.Assertion;

/// <summary>
/// Represents an assert operation that returns a value.
/// </summary>
/// <typeparam name="T">The type of value the operation returns.</typeparam>
public interface IAssertWithResult<T>
{
    /// <summary>
    /// Runs the specified assertion and handles the result as configured.
    /// </summary>
    /// <param name="assertAction">The assertion to execute.</param>
    /// <returns>One of the configured values depending on wether the assertion failed or succeeded.</returns>
    T That(Action<Assert> assertAction);
}
