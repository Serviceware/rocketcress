namespace Rocketcress.UIAutomation.ControlSearch.Conditions;

/// <summary>
/// Represents an OR condition for finding UIAutomation elements.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.Conditions.CompositionSearchConditionBase" />
public class OrCondition : CompositionSearchConditionBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrCondition"/> class.
    /// </summary>
    /// <param name="conditions">The composed conditions.</param>
    public OrCondition(params ISearchCondition[] conditions)
        : base(conditions)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrCondition"/> class.
    /// </summary>
    /// <param name="conditions">The composed conditions.</param>
    public OrCondition(IEnumerable<ISearchCondition> conditions)
        : base(conditions)
    {
    }

    /// <inheritdoc />
    public override SearchConditionOperator OperatorType => SearchConditionOperator.Or;

    /// <inheritdoc />
    public override bool Check(AutomationElement element, TreeWalker treeWalker)
    {
        bool? result = null;
        foreach (var condition in Conditions)
        {
            result = condition.Check(element, treeWalker);
            if (result == true)
                break;
        }

        return result ?? true;
    }

    /// <inheritdoc />
    public override string GetDescription()
    {
        if (Conditions.Count == 0)
            return null;
        if (Conditions.Count == 1)
            return Conditions[0].GetDescription();
        return $"({string.Join(" or ", Conditions.Select(x => x.GetDescription()))})";
    }

    /// <inheritdoc />
    protected override SearchConditionBase CloneInternal()
    {
        return new OrCondition();
    }
}
