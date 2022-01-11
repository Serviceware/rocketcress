#nullable disable

namespace Rocketcress.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the IEnumerable interface.
    /// </summary>
    public static class EnumerableExtensions
    {
        #region http://stackoverflow.com/questions/2471588/how-to-get-index-using-linq

        /// <summary>Finds the index of the first item matching an expression in an enumerable.</summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="items">The enumerable to search.</param>
        /// <param name="predicate">The expression to test the items against.</param>
        /// <returns>The index of the first matching item, or -1 if no items match.</returns>
        public static int IndexOf<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            int retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item))
                    return retVal;
                retVal++;
            }

            return -1;
        }

        /// <summary>Finds the index of the first occurence of an item in an enumerable.</summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="items">The enumerable to search.</param>
        /// <param name="item">The item to find.</param>
        /// <returns>The index of the first matching item, or -1 if the item was not found.</returns>
        public static int IndexOf<T>(this IEnumerable<T> items, T item)
        {
            return items.IndexOf(i => EqualityComparer<T>.Default.Equals(item, i));
        }

        #endregion

        /// <summary>
        /// Iterates through an enumerable and executes an action.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="items">The enumerable.</param>
        /// <param name="action">The action to execute for each element.</param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }

        /// <summary>
        /// Iterates through an enumerable and executes an action.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="items">The enumerable.</param>
        /// <param name="action">The action to execute for each element.</param>
        /// <returns>Returns an enumerable of values after executing the action.</returns>
        public static IEnumerable<T> Each<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
                yield return item;
            }
        }

        /// <summary>
        /// Checks wether the enumerable is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="enumerable">The enumerable to check.</param>
        /// <returns>Returns true if the enumerable is null or has not elements; otherwise false.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }

        /// <summary>
        /// Tries to get the first value out of an enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="value">The first value.</param>
        /// <returns>Return true if a first element existed; otherwise false.</returns>
        public static bool TryFirst<T>(this IEnumerable<T> enumerable, out T value)
        {
            using var e = enumerable.GetEnumerator();
            var result = e.MoveNext();
            value = result ? e.Current : default;
            return result;
        }

        /// <summary>
        /// Tries to get the first value out of an enumerable that fulfills a condition.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="predicate">The condition.</param>
        /// <param name="value">The first matching value.</param>
        /// <returns>Return true if a first element that fulfills the condition existed; otherwise false.</returns>
        public static bool TryFirst<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, out T value)
        {
            foreach (T element in enumerable)
            {
                if (predicate(element))
                {
                    value = element;
                    return true;
                }
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Wraps an enumerable into a LazyList.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>Returns a LazyList that wraps the given enumerable.</returns>
        public static LazyList<T> ToLazyList<T>(this IEnumerable<T> enumerable)
        {
            return new LazyList<T>(enumerable);
        }

        /// <summary>
        /// Converts an enumerator to an enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="enumerator">The enumerator.</param>
        /// <returns>Returns an enumerable containing all element from the enumerator.</returns>
        public static IEnumerable<T> AsEnumerable<T>(this IEnumerator<T> enumerator)
        {
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }
    }
}
