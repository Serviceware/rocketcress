namespace Rocketcress.Core.Extensions;

/// <summary>
/// Provides extension methods for the ICollection interface.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Adds multiple items to the collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="list">The list to add items to.</param>
    /// <param name="itemsToAdd">The items to add to the collection.</param>
    public static void AddRange<T>(this ICollection<T> list, params T[] itemsToAdd) => AddRange(list, (IEnumerable<T>)itemsToAdd);

    /// <summary>
    /// Adds multiple items to the collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="list">The list to add items to.</param>
    /// <param name="itemsToAdd">The items to add to the collection.</param>
    public static void AddRange<T>(this ICollection<T> list, IEnumerable<T> itemsToAdd)
    {
        foreach (var item in itemsToAdd)
            list.Add(item);
    }
}
