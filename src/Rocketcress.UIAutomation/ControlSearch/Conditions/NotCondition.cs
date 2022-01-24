namespace Rocketcress.UIAutomation.ControlSearch.Conditions;

/// <summary>
/// Represents a NOT condition for finding UIAutomation elements.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.Conditions.SearchConditionBase" />
public class NotCondition : SearchConditionBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotCondition"/> class.
    /// </summary>
    /// <param name="subCondition">The sub condition.</param>
    public NotCondition(ISearchCondition subCondition)
    {
        SubCondition = Guard.NotNull(subCondition);
    }

    /// <summary>
    /// Gets the sub condition.
    /// </summary>
    public ISearchCondition SubCondition { get; internal set; }

    /// <inheritdoc />
    public override bool Check(AutomationElement element, TreeWalker treeWalker)
    {
        return !SubCondition.Check(element, treeWalker);
    }

    /// <inheritdoc />
    public override string GetDescription()
    {
        return $"not({SubCondition.GetDescription()})";
    }

    /// <inheritdoc />
    protected override SearchConditionBase CloneInternal()
    {
        var subCondition = (ISearchCondition)SubCondition.Clone();
        return new NotCondition(subCondition);
    }
}
