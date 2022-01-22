using System.Text.RegularExpressions;

namespace Rocketcress.Core.Common;

internal class SettingKeyEqualityComparer : IEqualityComparer<string>
{
    private static readonly Regex _settingKeyRegex = new(@"(\[(?<Tag>[^\]]*)\])?\s*(?<Key>.*)", RegexOptions.Compiled);

    public static string GetKey(string? keyWithTag)
    {
        if (keyWithTag is null)
            return string.Empty;
        var g = _settingKeyRegex.Match(keyWithTag).Groups["Key"];
        return g.Success ? g.Value : keyWithTag;
    }

    public bool Equals(string? x, string? y)
    {
        return string.Equals(GetKey(x), GetKey(y), StringComparison.Ordinal);
    }

    public int GetHashCode(string? obj)
    {
        return GetKey(obj).GetHashCode();
    }
}
