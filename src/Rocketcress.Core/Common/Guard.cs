using System.Runtime.CompilerServices;

namespace Rocketcress.Core.Common;

internal static class Guard
{
    [return: NotNull]
    public static T NotNull<T>([NotNull] T value, [CallerArgumentExpression("value")] string name = "")
    {
        if (value == null)
            throw new ArgumentNullException(name);
        return value;
    }

    public static string NotNullOrEmpty([NotNull] string? value, [CallerArgumentExpression("value")] string name = "")
    {
        _ = NotNull(value, name);
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("The parameter value cannot be empty.", name);
        return value;
    }

    public static string NotNullOrWhiteSpace([NotNull] string? value, [CallerArgumentExpression("value")] string name = "")
    {
        _ = NotNull(value, name);
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("The parameter value cannot be empty nor should consist of only white space characters.", name);
        return value;
    }
}
