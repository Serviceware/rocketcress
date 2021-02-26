using System;
using System.Linq;

namespace Rocketcress.UIAutomation.Extensions
{
    public static class ByOptionsExtensions
    {
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

        public static string GetDescription(this ByOptions options)
        {
            return string.Join(", ", Enum.GetValues(typeof(ByOptions)).OfType<ByOptions>().Where(x => x > 0 && options.HasFlag(x)));
        }
    }
}
