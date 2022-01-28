using System.Runtime.CompilerServices;

namespace Rocketcress.Core.Common;

/// <summary>
/// Guard class for validating arguments.
/// </summary>
public static class Guard
{
    /// <summary>
    /// Validates that the specified value is not null.
    /// </summary>
    /// <typeparam name="T">The type of value to validate.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="name">The name of the parameter.</param>
    /// <returns>The same value as <paramref name="value"/>.</returns>
    /// <exception cref="System.ArgumentNullException"><paramref name="value"/> is null.</exception>
    [return: NotNull]
    public static T NotNull<T>([NotNull] T value, [CallerArgumentExpression("value")] string name = "")
    {
        if (value == null)
            throw new ArgumentNullException(name);
        return value;
    }

    /// <summary>
    /// Validates that the specified value is neither null nor empty.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="name">The name of the parameter.</param>
    /// <returns>The same value as <paramref name="value"/>.</returns>
    /// <exception cref="System.ArgumentException">The parameter value cannot be empty.</exception>
    public static string NotNullOrEmpty([NotNull] string? value, [CallerArgumentExpression("value")] string name = "")
    {
        _ = NotNull(value, name);
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("The parameter value cannot be empty.", name);
        return value;
    }

    /// <summary>
    /// Validates that the specified value is neither null, empty nor consist of only whitespace characters.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="name">The name of the parameter.</param>
    /// <returns>The same value as <paramref name="value"/>.</returns>
    /// <exception cref="System.ArgumentException">The parameter value cannot be empty nor should consist of only white space characters.</exception>
    public static string NotNullOrWhiteSpace([NotNull] string? value, [CallerArgumentExpression("value")] string name = "")
    {
        _ = NotNull(value, name);
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("The parameter value cannot be empty nor should consist of only white space characters.", name);
        return value;
    }
}
