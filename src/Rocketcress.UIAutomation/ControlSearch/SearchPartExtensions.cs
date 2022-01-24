using Rocketcress.Core.Extensions;
using AndCondition = Rocketcress.UIAutomation.ControlSearch.Conditions.AndCondition;
using OrCondition = Rocketcress.UIAutomation.ControlSearch.Conditions.OrCondition;

namespace Rocketcress.UIAutomation.ControlSearch;

/// <summary>
/// Provides extension methods for the <see cref="ISearchPart"/> interface.
/// </summary>
public static class SearchPartExtensions
{
    /// <summary>
    /// Appends a specified condition.
    /// </summary>
    /// <param name="part">The part.</param>
    /// <param name="condition">The condition.</param>
    /// <param name="operator">The operator.</param>
    public static void AppendCondition(this ISearchPart part, ISearchCondition condition, SearchConditionOperator @operator)
    {
        Guard.NotNull(part);

        if (condition == null)
            return;
        if (part.Condition == null)
        {
            part.Condition = condition;
        }
        else if (part.Condition is ICompositeSearchCondition compositeCondition)
        {
            if (compositeCondition.OperatorType == @operator)
            {
                if (condition is ICompositeSearchCondition conditionAsComposite && conditionAsComposite.OperatorType == compositeCondition.OperatorType)
                    compositeCondition.Conditions.AddRange(conditionAsComposite.Conditions);
                else
                    compositeCondition.Conditions.Add(condition);
                compositeCondition.Conditions = compositeCondition.Conditions.Distinct().ToList();
            }
            else
            {
                part.Condition = CreateCompositeCondition(@operator, part.Condition, condition);
            }
        }
        else
        {
            part.Condition = CreateCompositeCondition(@operator, part.Condition, condition);
        }
    }

    /// <summary>
    /// Removes the conditions matching the given predicate.
    /// </summary>
    /// <param name="part">The part.</param>
    /// <param name="predicate">The predicate.</param>
    public static void RemoveCondition(this ISearchPart part, Func<ISearchCondition, bool> predicate) => RemoveCondition<ISearchCondition>(part, predicate);

    /// <summary>
    /// Removes the condition matching the given predicate.
    /// </summary>
    /// <typeparam name="T">The type of search condition to remove.</typeparam>
    /// <param name="part">The part.</param>
    /// <param name="predicate">The predicate.</param>
    public static void RemoveCondition<T>(this ISearchPart part, Func<T, bool> predicate)
        where T : ISearchCondition
    {
        Guard.NotNull(part);
        Guard.NotNull(predicate);

        if (part.Condition is ICompositeSearchCondition compositeSearchCondition)
        {
            var result = RemoveConditionRecursion(compositeSearchCondition, predicate);
            if (result)
                part.Condition = compositeSearchCondition.Conditions.FirstOrDefault();
        }
        else if (part.Condition is T condition && predicate(condition))
        {
            part.Condition = null;
        }
    }

    /// <summary>
    /// Gets the condition list.
    /// </summary>
    /// <param name="part">The part.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> iterating though all conditions.</returns>
    public static IEnumerable<ISearchCondition> GetConditionList(this ISearchPart part)
    {
        Guard.NotNull(part);

        if (part.Condition != null)
            yield return part.Condition;
        if (part.Condition is ICompositeSearchCondition compositeSearchCondition)
        {
            foreach (var c in compositeSearchCondition.GetConditions())
                yield return c;
        }
    }

    private static ICompositeSearchCondition CreateCompositeCondition(SearchConditionOperator @operator, params ISearchCondition[] conditions)
    {
        ICompositeSearchCondition result = @operator switch
        {
            SearchConditionOperator.And => new AndCondition(),
            SearchConditionOperator.Or => new OrCondition(),
            _ => throw new ArgumentOutOfRangeException(nameof(@operator)),
        };
        result.Conditions.AddRange(conditions);
        return result;
    }

    private static bool RemoveConditionRecursion<T>(ICompositeSearchCondition currentCondition, Func<T, bool> predicate)
        where T : ISearchCondition
    {
        foreach (var condition in currentCondition.Conditions.ToList())
        {
            if (condition is ICompositeSearchCondition compositeSearchCondition)
            {
                var result = RemoveConditionRecursion(compositeSearchCondition, predicate);
                if (result)
                {
                    var index = currentCondition.Conditions.IndexOf(compositeSearchCondition);
                    currentCondition.Conditions.RemoveAt(index);
                    if (compositeSearchCondition.Conditions.Count > 0)
                        currentCondition.Conditions.Insert(index, compositeSearchCondition.Conditions[0]);
                }
            }
            else if (condition is T castedCondition && predicate(castedCondition))
            {
                currentCondition.Conditions.Remove(condition);
            }
        }

        return currentCondition.Conditions.Count < 2;
    }
}
