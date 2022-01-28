using Rocketcress.UIAutomation.ControlSearch.SearchParts;

namespace Rocketcress.UIAutomation.ControlSearch.Conditions;

/// <summary>
/// Represents a search condition for finding UIAutomation elements that are relatives to other elements matched by a specified <see cref="ISearchCondition"/>.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.Conditions.SearchConditionBase" />
public class RelativeToCondition : SearchConditionBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RelativeToCondition"/> class.
    /// </summary>
    /// <param name="distance">The distance to the other control (negative values for preceding relatives).</param>
    /// <param name="condition">The condition for the relative element.</param>
    public RelativeToCondition(int distance, ISearchCondition condition)
    {
        Distance = distance;
        Condition = Guard.NotNull(condition);
    }

    /// <summary>
    /// Gets or sets the distance to the other control (negative values for preceding relatives).
    /// </summary>
    public int Distance { get; set; }

    /// <summary>
    /// Gets or sets the condition for the relative element.
    /// </summary>
    public ISearchCondition Condition { get; set; }

    /// <inheritdoc />
    public override bool Check(AutomationElement element, TreeWalker treeWalker)
    {
        bool result;
        if (Distance == 0)
        {
            result = Condition.Check(element, treeWalker);
        }
        else
        {
            var options = Distance > 0 ? RelativesSearchOptions.PrecedingRelatives : RelativesSearchOptions.SubsequentRelatives;
            result = RelativesSearchPart.FindElements(element, treeWalker, options, Condition, null).Skip(Math.Abs(Distance) - 1).Any();
        }

        return result;
    }

    /// <inheritdoc />
    public override string GetDescription()
    {
        return $"relativeto({Condition.GetDescription()}, {Distance})";
    }

    /// <inheritdoc />
    protected override SearchConditionBase CloneInternal()
    {
        var condition = (ISearchCondition)Condition.Clone();
        return new RelativeToCondition(Distance, condition);
    }
}
