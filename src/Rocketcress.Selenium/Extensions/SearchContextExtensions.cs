namespace Rocketcress.Selenium.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="ISearchContext"/> interface.
    /// </summary>
    public static class SearchContextExtensions
    {
        /// <summary>
        /// Tries to find an element.
        /// </summary>
        /// <param name="context">The context to search an element in.</param>
        /// <param name="locationKey">The key to locate the element.</param>
        /// <returns>Returns the element if it was found; otherwise null.</returns>
        public static IWebElement TryFindElement(this ISearchContext context, By locationKey)
        {
            return context.FindElements(locationKey).FirstOrDefault();
        }

        /// <summary>
        /// Checks wether an element does exist.
        /// </summary>
        /// <param name="context">The context to search an element in.</param>
        /// <param name="locationKey">The key to locate the element.</param>
        /// <returns>Returns true if an element was found; otherwise false.</returns>
        public static bool IsElementExistent(this ISearchContext context, By locationKey)
        {
            return context.TryFindElement(locationKey) != null;
        }

        /// <summary>
        /// Tries to find an element and retrieve the text.
        /// </summary>
        /// <param name="context">The context to search an element in.</param>
        /// <param name="locationKey">The key to locate the element.</param>
        /// <returns>Returns the inner text of the found element. If no eleemnt was found null is returned.</returns>
        public static string TryGetTextFromElement(this ISearchContext context, By locationKey)
        {
            var element = context.TryFindElement(locationKey);
            return element?.Text;
        }
    }
}
