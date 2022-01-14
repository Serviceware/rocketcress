namespace Rocketcress.UIAutomation.ControlSearch;

public static class SearchConditionExtensions
{
    public static IEnumerable<ISearchCondition> GetConditions(this ICompositeSearchCondition condition)
    {
        foreach (var c in condition.Conditions)
        {
            yield return c;
            if (c is ICompositeSearchCondition cc)
            {
                foreach (var ccc in cc.GetConditions())
                    yield return ccc;
            }
        }
    }
}
