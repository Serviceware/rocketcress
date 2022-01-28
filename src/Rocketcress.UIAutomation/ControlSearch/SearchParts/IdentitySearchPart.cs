namespace Rocketcress.UIAutomation.ControlSearch.SearchParts;

/// <summary>
/// Represents a <see cref="ISearchPart"/> that returns the same element as passed in.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.SearchParts.SearchPartBase" />
public class IdentitySearchPart : SearchPartBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentitySearchPart"/> class.
    /// </summary>
    public IdentitySearchPart()
        : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentitySearchPart"/> class.
    /// </summary>
    /// <param name="condition">The condition.</param>
    public IdentitySearchPart(ISearchCondition condition)
    {
        Condition = condition;
    }

    /// <inheritdoc/>
    public override string GetDescription()
    {
        return "/_" + GetConditionDescription() + GetSkipTakeDescription();
    }

    /// <inheritdoc/>
    protected override IEnumerable<AutomationElement> FindElementsInternal(AutomationElement element, TreeWalker treeWalker)
    {
        if (Condition?.Check(element, treeWalker) != false)
            yield return element;
    }

    /// <inheritdoc/>
    protected override SearchPartBase CloneInternal() => new IdentitySearchPart();
}
