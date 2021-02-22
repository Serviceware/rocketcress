using Rocketcress.UIAutomation.ControlSearch.Conditions;
using Rocketcress.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rocketcress.UIAutomation.ControlSearch
{
    public static class SearchPartExtensions
    {
        public static void AppendCondition(this ISearchPart part, ISearchCondition condition, SearchConditionOperator @operator)
        {
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
                    part.Condition = CreateCompositeCondition(@operator, part.Condition, condition);
            }
            else
            {
                part.Condition = CreateCompositeCondition(@operator, part.Condition, condition);
            }
        }

        public static void RemoveCondition(this ISearchPart part, Func<ISearchCondition, bool> predicate) => RemoveCondition<ISearchCondition>(part, predicate);
        public static void RemoveCondition<T>(this ISearchPart part, Func<T, bool> predicate) where T : ISearchCondition
        {
            if (part.Condition is ICompositeSearchCondition compositeSearchCondition)
            {
                var result = RemoveConditionRecursion(compositeSearchCondition, predicate);
                if (result)
                    part.Condition = compositeSearchCondition.Conditions.FirstOrDefault();
            }
            else if (part.Condition is T condition && predicate(condition))
                part.Condition = null;
        }
        
        public static IEnumerable<ISearchCondition> GetConditionList(this ISearchPart part)
        {
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
            ICompositeSearchCondition result;
            switch (@operator)
            {
                case SearchConditionOperator.And:
                    result = new AndCondition();
                    break;
                case SearchConditionOperator.Or:
                    result = new OrCondition();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(@operator));
            }
            result.Conditions.AddRange(conditions);
            return result;
        }

        private static bool RemoveConditionRecursion<T>(ICompositeSearchCondition currentCondition, Func<T, bool> predicate) where T : ISearchCondition
        {
            foreach(var condition in currentCondition.Conditions.ToList())
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
                else if (condition is T tCondition && predicate(tCondition))
                    currentCondition.Conditions.Remove(condition);
            }

            return currentCondition.Conditions.Count < 2;
        }
    }
}
