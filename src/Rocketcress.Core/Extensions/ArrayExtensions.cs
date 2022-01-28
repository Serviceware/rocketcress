namespace Rocketcress.Core.Extensions;

/// <summary>
/// Provides extensions for <see cref="Array"/>s.
/// </summary>
public static class ArrayExtensions
{
    private static readonly char[] HexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

    /// <summary>
    /// Converts the <see cref="Array"/> of <see cref="byte"/>s to an hexadecimal representation.
    /// </summary>
    /// <param name="bytes">The <see cref="byte"/>s to convert.</param>
    /// <returns>An <see cref="string"/> that contains the hexadecimal representation of the given <see cref="byte"/>s.</returns>
    public static string ToHexString(this byte[]? bytes)
    {
        return new string(ToHexChars(bytes));
    }

    /// <summary>
    /// Converts the <see cref="Array"/> of <see cref="byte"/>s to an hexadecimal representation.
    /// </summary>
    /// <param name="bytes">The <see cref="byte"/>s to convert.</param>
    /// <returns>An <see cref="Array"/> of <see cref="char"/>s that contains the hexadecimal representation of the given <see cref="byte"/>s.</returns>
    [return: NotNullIfNotNull("bytes")]
    public static char[]? ToHexChars(this byte[]? bytes)
    {
        if (bytes is null)
            return null;

        var result = new char[bytes.Length * 2];
        for (int i = 0; i < bytes.Length; i++)
        {
            result[i * 2] = HexChars[bytes[i] >> 4];
            result[(i * 2) + 1] = HexChars[bytes[i] & 0xF];
        }

        return result;
    }
}
