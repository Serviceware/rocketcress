namespace Rocketcress.UIAutomation.ControlSearch;

/// <summary>
/// Search engine for finding UIAutomation elements.
/// </summary>
public static class SearchEngine
{
    /// <summary>
    /// Finds the first element matching the given location key.
    /// </summary>
    /// <param name="locationKey">The location key.</param>
    /// <returns>If an element was found it is returned; otherwise <c>null</c> is returned.</returns>
    public static AutomationElement FindFirst(By locationKey) => FindFirst(locationKey, AutomationElement.RootElement);

    /// <summary>
    /// Finds the first element matching the given location key starting the search from a given element.
    /// </summary>
    /// <param name="locationKey">The location key.</param>
    /// <param name="parent">The element to start the search from.</param>
    /// <returns>If an element was found it is returned; otherwise <c>null</c> is returned.</returns>
    public static AutomationElement FindFirst(By locationKey, AutomationElement parent) => FindAll(locationKey, parent).FirstOrDefault();

    /// <summary>
    /// Finds all elements matching the given location key.
    /// </summary>
    /// <param name="locationKey">The location key.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> iterating through all matching elements.</returns>
    public static IEnumerable<AutomationElement> FindAll(By locationKey) => FindAll(locationKey, AutomationElement.RootElement);

    /// <summary>
    /// Finds all elements matching the given location key.
    /// </summary>
    /// <param name="locationKey">The location key.</param>
    /// <param name="parent">The element to start the search from.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> iterating through all matching elements.</returns>
    public static IEnumerable<AutomationElement> FindAll(By locationKey, AutomationElement parent)
    {
        Guard.NotNull(locationKey);
        Guard.NotNull(parent);
        return locationKey.RootSearchPart.FindElements(parent, TreeWalker.RawViewWalker);
    }

    internal static IEnumerable<AutomationElement> FindElements(AutomationElement element, TreeWalker treeWalker, ISearchCondition condition, ISearchPart childPart)
    {
        if (condition?.Check(element, treeWalker) != false)
        {
            if (childPart == null)
            {
                yield return element;
            }
            else
            {
                foreach (var x in childPart.FindElements(element, treeWalker))
                    yield return x;
            }
        }
    }
}
