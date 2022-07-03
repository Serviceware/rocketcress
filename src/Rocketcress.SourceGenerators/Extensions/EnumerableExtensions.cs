namespace Rocketcress.SourceGenerators.Extensions
{
    internal static class EnumerableExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            int index = 0;
            foreach (var item in enumerable)
            {
                if (predicate(item))
                    return index;
                index++;
            }

            return -1;
        }
    }
}
