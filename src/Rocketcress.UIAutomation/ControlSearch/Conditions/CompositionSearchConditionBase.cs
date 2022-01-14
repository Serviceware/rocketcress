namespace Rocketcress.UIAutomation.ControlSearch.Conditions;

public abstract class CompositionSearchConditionBase : SearchConditionBase, ICompositeSearchCondition
{
    public abstract SearchConditionOperator OperatorType { get; }
    public IList<ISearchCondition> Conditions { get; set; }

    public CompositionSearchConditionBase(params ISearchCondition[] conditions)
        : this((IEnumerable<ISearchCondition>)conditions)
    {
    }

    public CompositionSearchConditionBase(IEnumerable<ISearchCondition> conditions)
    {
        Conditions = conditions.ToList();
    }

    public override object Clone()
    {
        var clone = (CompositionSearchConditionBase)base.Clone();
        clone.Conditions = Conditions.Select(x => (ISearchCondition)x.Clone()).ToList();
        return clone;
    }
}
