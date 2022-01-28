namespace Rocketcress.UIAutomation.ControlSearch.Conditions;

/// <summary>
/// Represents a search condition for finding UIAutomation elements composed of multiple <see cref="ISearchCondition"/> instances.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.Conditions.SearchConditionBase" />
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.ICompositeSearchCondition" />
public abstract class CompositionSearchConditionBase : SearchConditionBase, ICompositeSearchCondition
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CompositionSearchConditionBase"/> class.
    /// </summary>
    /// <param name="conditions">The composed conditions.</param>
    public CompositionSearchConditionBase(params ISearchCondition[] conditions)
        : this((IEnumerable<ISearchCondition>)conditions)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositionSearchConditionBase"/> class.
    /// </summary>
    /// <param name="conditions">The composed conditions.</param>
    public CompositionSearchConditionBase(IEnumerable<ISearchCondition> conditions)
    {
        Conditions = conditions.ToList();
    }

    /// <inheritdoc />
    public abstract SearchConditionOperator OperatorType { get; }

    /// <inheritdoc />
    public IList<ISearchCondition> Conditions { get; set; }

    /// <inheritdoc />
    public override object Clone()
    {
        var clone = (CompositionSearchConditionBase)base.Clone();
        clone.Conditions = Conditions.Select(x => (ISearchCondition)x.Clone()).ToList();
        return clone;
    }
}
