namespace Rocketcress.UIAutomation.ControlSearch.Conditions;

/// <summary>
/// Represents a search condition for finding UIAutomation elements that have UIAutomation elements matching a <see cref="ISearchCondition"/>.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.Conditions.SearchConditionBase" />
public class HasElementCondition : SearchConditionBase
{
    /// <summary>
    /// Gets the element part.
    /// </summary>
    public ISearchPart ElementPart { get; internal set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HasElementCondition"/> class.
    /// </summary>
    /// <param name="elementPart">The element part.</param>
    public HasElementCondition(ISearchPart elementPart)
    {
        ElementPart = Guard.NotNull(elementPart);
    }

    /// <inheritdoc />
    public override bool Check(AutomationElement element, TreeWalker treeWalker)
    {
        return ElementPart.FindElements(element, treeWalker).Any();
    }

    /// <inheritdoc />
    public override string GetDescription()
    {
        return $".{ElementPart.GetDescription()}";
    }

    /// <inheritdoc />
    protected override SearchConditionBase CloneInternal()
    {
        var elementPart = (ISearchPart)ElementPart.Clone();
        return new HasElementCondition(elementPart);
    }
}
