namespace Rocketcress.UIAutomation.ControlSearch;

/// <summary>
/// Represents a search part for finding UIAutomation elements.
/// </summary>
/// <seealso cref="System.ICloneable" />
public interface ISearchPart : ICloneable
{
    /// <summary>
    /// Gets or sets the condition.
    /// </summary>
    ISearchCondition Condition { get; set; }

    /// <summary>
    /// Finds the elements matching the <see cref="Condition"/>.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="treeWalker">The tree walker.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that iterates through all elements matching the <see cref="Condition"/>.</returns>
    IEnumerable<AutomationElement> FindElements(AutomationElement element, TreeWalker treeWalker);

    /// <summary>
    /// Gets the description of this <see cref="ISearchPart"/>.
    /// </summary>
    /// <returns>The description of this <see cref="ISearchPart"/>.</returns>
    string GetDescription();
}

/// <summary>
/// Represents a search part for finding UIAutomation elements with a nested search part.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.ISearchPart" />
public interface INestedSearchPart : ISearchPart
{
    /// <summary>
    /// Gets or sets the child part.
    /// </summary>
    ISearchPart ChildPart { get; set; }
}

/// <summary>
/// Represents a search part for finding UIAutomation elements with max depth.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.ISearchPart" />
public interface IDepthSearchPart : ISearchPart
{
    /// <summary>
    /// Gets or sets the maximum search depth.
    /// </summary>
    int MaxDepth { get; set; }
}
