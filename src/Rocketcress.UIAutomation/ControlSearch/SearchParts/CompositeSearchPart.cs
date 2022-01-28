namespace Rocketcress.UIAutomation.ControlSearch.SearchParts;

/// <summary>
/// Represents a <see cref="ISearchPart"/> that is a composite of multiple <see cref="ISearchPart"/>s.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.SearchParts.SearchPartBase" />
public class CompositeSearchPart : SearchPartBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeSearchPart"/> class.
    /// </summary>
    /// <param name="parts">The composed parts.</param>
    public CompositeSearchPart(params ISearchPart[] parts)
        : this((IEnumerable<ISearchPart>)parts)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeSearchPart"/> class.
    /// </summary>
    /// <param name="parts">The composed parts.</param>
    public CompositeSearchPart(IEnumerable<ISearchPart> parts)
    {
        Parts = Guard.NotNull(parts).ToList();
    }

    /// <summary>
    /// Gets or sets the parts.
    /// </summary>
    public IList<ISearchPart> Parts { get; set; }

    /// <inheritdoc/>
    public override string GetDescription()
    {
        if (Parts.Count == 0)
            return null;

        string result;
        if (Parts.Count == 1)
            result = Parts[0].GetDescription();
        else
            result = $"{string.Join("|", Parts.Select(x => x.GetDescription()))}";

        result += GetConditionDescription() + GetSkipTakeDescription();

        return result;
    }

    /// <inheritdoc/>
    protected override IEnumerable<AutomationElement> FindElementsInternal(AutomationElement element, TreeWalker treeWalker)
    {
        return Parts.SelectMany(x => x.FindElements(element, treeWalker)).Where(x => Condition?.Check(x, treeWalker) != false);
    }

    /// <inheritdoc/>
    protected override SearchPartBase CloneInternal()
    {
        var parts = Parts.Select(x => (ISearchPart)x.Clone());
        return new CompositeSearchPart(parts);
    }
}
