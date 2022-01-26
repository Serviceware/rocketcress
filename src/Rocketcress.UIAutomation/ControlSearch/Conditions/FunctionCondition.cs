namespace Rocketcress.UIAutomation.ControlSearch.Conditions;

/// <summary>
/// Delegate to determine whether an <see cref="AutomationElement"/> matches or not.
/// </summary>
/// <param name="element">The element.</param>
/// <param name="treeWalker">The tree walker.</param>
/// <returns><c>true</c> if the element matches; otherwise <c>false</c>.</returns>
public delegate bool FunctionConditionDelegate(AutomationElement element, TreeWalker treeWalker);

/// <summary>
/// Represents a search condition for finding UIAutomation elements that uses a <see cref="FunctionConditionDelegate"/>.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.Conditions.SearchConditionBase" />
public class FunctionCondition : SearchConditionBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionCondition"/> class.
    /// </summary>
    /// <param name="conditionName">Name of the condition.</param>
    /// <param name="condition">The condition.</param>
    public FunctionCondition(string conditionName, FunctionConditionDelegate condition)
    {
        ConditionName = Guard.NotNull(conditionName);
        Condition = Guard.NotNull(condition);
    }

    /// <summary>
    /// Gets the name of the condition.
    /// </summary>
    public string ConditionName { get; internal set; }

    /// <summary>
    /// Gets the condition.
    /// </summary>
    public FunctionConditionDelegate Condition { get; internal set; }

    /// <inheritdoc />
    public override bool Check(AutomationElement element, TreeWalker treeWalker)
    {
        return Condition(element, treeWalker);
    }

    /// <inheritdoc />
    public override string GetDescription()
    {
        return $"func('{ConditionName}')";
    }

    /// <inheritdoc />
    protected override SearchConditionBase CloneInternal()
    {
        return new FunctionCondition(ConditionName, Condition);
    }
}
