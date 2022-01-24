namespace Rocketcress.UIAutomation.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="ByOptions"/> enum.
/// </summary>
public static class ByOptionsExtensions
{
    /// <summary>
    /// Checks the specified value.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <returns><c>true</c> if the <paramref name="actual"/> value matches the <paramref name="expected"/> value.</returns>
    public static bool Check(this ByOptions options, string expected, string actual)
    {
        bool result;
        if (options.HasFlag(ByOptions.IgnoreCase))
        {
            actual = actual?.ToLower();
            expected = expected?.ToLower();
        }

        if (options.HasFlag(ByOptions.UseContains))
            result = actual?.Contains(expected) ?? false;
        else
            result = string.Equals(actual, expected);
        return result ^ options.HasFlag(ByOptions.Unequal);
    }

    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <returns>The description.</returns>
    public static string GetDescription(this ByOptions options)
    {
        return string.Join(", ", Enum.GetValues(typeof(ByOptions)).OfType<ByOptions>().Where(x => x > 0 && options.HasFlag(x)));
    }
}
