namespace Rocketcress.UIAutomation.ControlSearch.Conditions;

/// <summary>
/// Represents an AND condition for finding UIAutomation elements.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.Conditions.CompositionSearchConditionBase" />
public class AndCondition : CompositionSearchConditionBase
{
    public override SearchConditionOperator OperatorType => SearchConditionOperator.And;

    /// <summary>
    /// Initializes a new instance of the <see cref="AndCondition"/> class.
    /// </summary>
    /// <param name="conditions">The conditions to compose.</param>
    public AndCondition(params ISearchCondition[] conditions)
        : base(conditions)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AndCondition"/> class.
    /// </summary>
    /// <param name="conditions">The conditions to compose.</param>
    public AndCondition(IEnumerable<ISearchCondition> conditions)
        : base(conditions)
    {
    }

    /// <inheritdoc />
    public override bool Check(AutomationElement element, TreeWalker treeWalker)
    {
        var result = true;
        foreach (var condition in Conditions)
        {
            result = condition.Check(element, treeWalker);
            if (!result)
                break;
        }

        return result;
    }

    /// <inheritdoc />
    public override string GetDescription()
    {
        if (Conditions.Count == 0)
            return null;
        if (Conditions.Count == 1)
            return Conditions[0].GetDescription();
        return $"({string.Join(" and ", Conditions.Select(x => x.GetDescription()))})";
    }

    /// <inheritdoc />
    protected override SearchConditionBase CloneInternal()
    {
        return new AndCondition();
    }
}
