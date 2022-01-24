namespace Rocketcress.UIAutomation.ControlSearch;

/// <summary>
/// Provides extension methods for the <see cref="ISearchCondition"/> interface.
/// </summary>
public static class SearchConditionExtensions
{
    /// <summary>
    /// Gets the conditions.
    /// </summary>
    /// <param name="condition">The condition.</param>
    /// <returns>The sub conditions.</returns>
    public static IEnumerable<ISearchCondition> GetConditions(this ICompositeSearchCondition condition)
    {
        Guard.NotNull(condition);
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
