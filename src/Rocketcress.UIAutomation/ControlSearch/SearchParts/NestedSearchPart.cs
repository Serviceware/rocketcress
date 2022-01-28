namespace Rocketcress.UIAutomation.ControlSearch.SearchParts;

/// <summary>
/// Represents a <see cref="INestedSearchPart"/>.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.SearchParts.SearchPartBase" />
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.INestedSearchPart" />
public class NestedSearchPart : SearchPartBase, INestedSearchPart
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NestedSearchPart"/> class.
    /// </summary>
    /// <param name="parts">The nested parts.</param>
    public NestedSearchPart(params ISearchPart[] parts)
        : this((IEnumerable<ISearchPart>)parts)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NestedSearchPart"/> class.
    /// </summary>
    /// <param name="parts">The nested parts.</param>
    public NestedSearchPart(IEnumerable<ISearchPart> parts)
    {
        Parts = parts.ToList();
    }

    /// <inheritdoc/>
    public ISearchPart ChildPart
    {
        get => Parts[Parts.Count - 1];
        set => Parts[Parts.Count - 1] = value;
    }

    /// <summary>
    /// Gets or sets the nested parts.
    /// </summary>
    public IList<ISearchPart> Parts { get; set; }

    /// <inheritdoc/>
    public override ISearchCondition Condition
    {
        get => base.Condition;
        set => throw new NotSupportedException("Setting the condition is not supporten on a nested search part.");
    }

    /// <inheritdoc/>
    public override string GetDescription()
    {
        return string.Concat(Parts.Select(x => x.GetDescription()));
    }

    /// <inheritdoc/>
    protected override IEnumerable<AutomationElement> FindElementsInternal(AutomationElement element, TreeWalker treeWalker)
    {
        return FindElementsRecursive(element, treeWalker, 0);
    }

    /// <inheritdoc/>
    protected override SearchPartBase CloneInternal()
    {
        var parts = Parts.Select(x => (ISearchPart)x.Clone());
        return new NestedSearchPart(parts);
    }

    private IEnumerable<AutomationElement> FindElementsRecursive(AutomationElement element, TreeWalker treeWalker, int currentPartIndex)
    {
        foreach (var match in Parts[currentPartIndex].FindElements(element, treeWalker))
        {
            if (currentPartIndex + 1 < Parts.Count)
            {
                foreach (var recursiveMatch in FindElementsRecursive(match, treeWalker, currentPartIndex + 1))
                    yield return recursiveMatch;
            }
            else
            {
                yield return match;
            }
        }
    }
}
