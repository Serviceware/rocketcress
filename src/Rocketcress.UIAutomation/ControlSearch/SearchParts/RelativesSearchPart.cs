namespace Rocketcress.UIAutomation.ControlSearch.SearchParts;

/// <summary>
/// Represents a <see cref="INestedSearchPart"/> that searches for relative elements.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.SearchParts.SearchPartBase" />
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.INestedSearchPart" />
public class RelativesSearchPart : SearchPartBase, INestedSearchPart
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RelativesSearchPart"/> class.
    /// </summary>
    /// <param name="options">The search options.</param>
    public RelativesSearchPart(RelativesSearchOptions options)
        : this(options, null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelativesSearchPart"/> class.
    /// </summary>
    /// <param name="options">The search options.</param>
    /// <param name="condition">The condition.</param>
    public RelativesSearchPart(RelativesSearchOptions options, ISearchCondition condition)
        : this(options, condition, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelativesSearchPart"/> class.
    /// </summary>
    /// <param name="options">The search options.</param>
    /// <param name="condition">The condition.</param>
    /// <param name="childPart">The child part.</param>
    public RelativesSearchPart(RelativesSearchOptions options, ISearchCondition condition, ISearchPart childPart)
    {
        Options = options;
        ChildPart = childPart;
        Condition = condition;
    }

    /// <summary>
    /// Gets or sets the search options.
    /// </summary>
    public RelativesSearchOptions Options { get; set; }

    /// <inheritdoc/>
    public ISearchPart ChildPart { get; set; }

    /// <summary>
    /// Finds the elements matching the given conditions.
    /// </summary>
    /// <param name="element">The element from which to start the search.</param>
    /// <param name="treeWalker">The tree walker.</param>
    /// <param name="options">The search options.</param>
    /// <param name="condition">The condition.</param>
    /// <param name="childPart">The child part.</param>
    /// <returns>A <see cref="IEnumerable{T}"/> that iterates through all found elements.</returns>
    public static IEnumerable<AutomationElement> FindElements(AutomationElement element, TreeWalker treeWalker, RelativesSearchOptions options, ISearchCondition condition, ISearchPart childPart)
    {
        Guard.NotNull(element);
        Guard.NotNull(treeWalker);

        if (options.HasFlag(RelativesSearchOptions.IncludeElement))
        {
            foreach (var res in SearchEngine.FindElements(element, treeWalker, condition, childPart))
                yield return res;
        }

        if (options.HasFlag(RelativesSearchOptions.PrecedingRelatives))
        {
            foreach (var res in FindElements(element, treeWalker, condition, childPart, (e, t) => t.GetPreviousSibling(e)))
                yield return res;
        }

        if (options.HasFlag(RelativesSearchOptions.SubsequentRelatives))
        {
            foreach (var res in FindElements(element, treeWalker, condition, childPart, (e, t) => t.GetNextSibling(e)))
                yield return res;
        }
    }

    /// <inheritdoc/>
    public override string GetDescription()
    {
        return "/" +
            (Options.HasFlag(RelativesSearchOptions.IncludeElement) ? "." : string.Empty) +
            (Options.HasFlag(RelativesSearchOptions.PrecedingRelatives) ? "<" : string.Empty) +
            (Options.HasFlag(RelativesSearchOptions.SubsequentRelatives) ? ">" : string.Empty) +
            GetConditionDescription() +
            (ChildPart == null ? string.Empty : ChildPart.GetDescription());
    }

    /// <inheritdoc/>
    protected override IEnumerable<AutomationElement> FindElementsInternal(AutomationElement element, TreeWalker treeWalker)
        => FindElements(element, treeWalker, Options, Condition, ChildPart);

    /// <inheritdoc/>
    protected override SearchPartBase CloneInternal()
    {
        var childPart = (ISearchPart)ChildPart?.Clone();
        return new RelativesSearchPart(Options, null, childPart);
    }

    private static IEnumerable<AutomationElement> FindElements(AutomationElement element, TreeWalker treeWalker, ISearchCondition condition, ISearchPart childPart, Func<AutomationElement, TreeWalker, AutomationElement> nextElement)
    {
        var current = nextElement(element, treeWalker);
        while (current != null)
        {
            foreach (var res in SearchEngine.FindElements(current, treeWalker, condition, childPart))
                yield return res;
            current = nextElement(current, treeWalker);
        }
    }
}

/// <summary>
/// Search options for the <see cref="RelativesSearchPart"/> class.
/// </summary>
public enum RelativesSearchOptions
{
    /// <summary>
    /// No options given.
    /// </summary>
    None = 0x0,

    /// <summary>
    /// Preceding relatives should be searched.
    /// </summary>
    PrecedingRelatives = 0x1,

    /// <summary>
    /// Subsequent relatives should be searched.
    /// </summary>
    SubsequentRelatives = 0x2,

    /// <summary>
    /// All relatives should be searched.
    /// </summary>
    AllRelatives = 0x3,

    /// <summary>
    /// Include the starting element in the search.
    /// </summary>
    IncludeElement = 0x4,
}
