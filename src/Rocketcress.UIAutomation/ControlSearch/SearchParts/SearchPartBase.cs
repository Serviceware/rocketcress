using Rocketcress.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using PropertyCondition = Rocketcress.UIAutomation.ControlSearch.Conditions.PropertyCondition;

namespace Rocketcress.UIAutomation.ControlSearch.SearchParts
{
    public abstract class SearchPartBase : ISearchPart
    {
        public virtual ISearchCondition Condition { get; set; }
        public virtual int? SkipCount { get; set; }
        public virtual int? TakeCount { get; set; }
        
        public IEnumerable<AutomationElement> FindElements(AutomationElement element, TreeWalker treeWalker)
        {
            var result = FindElementsInternal(element, treeWalker);
            if (SkipCount.HasValue && SkipCount > 0)
                result = result.Skip(SkipCount.Value);
            if (TakeCount.HasValue && TakeCount >= 0)
                result = result.Take(TakeCount.Value);
            return result;
        }

        protected abstract IEnumerable<AutomationElement> FindElementsInternal(AutomationElement element, TreeWalker treeWalker);

        public object Clone()
        {
            var clone = CloneInternal();
            try { clone.Condition = (ISearchCondition)Condition?.Clone(); } catch { }
            try { clone.SkipCount = SkipCount; } catch { }
            try { clone.TakeCount = TakeCount; } catch { }
            return clone;
        }

        protected abstract SearchPartBase CloneInternal();
        public abstract string GetDescription();

        protected string GetSkipTakeDescription()
        {
            if (SkipCount.HasValue || TakeCount.HasValue)
                return $"[{(SkipCount ?? 0) + 1}{(TakeCount.HasValue ? $":{TakeCount}" : "")}]";
            return null;
        }

        protected string GetConditionDescription()
        {
            if (Condition == null)
                return "";

            var prefix = "";
            var condition = Condition;
            if (Condition is ICompositeSearchCondition compositeCondition &&
                compositeCondition.Conditions.OfType<PropertyCondition>().TryFirst(x => x.Property == AutomationElement.ControlTypeProperty, out var tempCtp))
            {
                var clone = (ICompositeSearchCondition)Condition.Clone();
                clone.Conditions.Remove(tempCtp);

                prefix = ((ControlType)tempCtp.Value).ProgrammaticName.Split('.').Last();
                condition = clone.Conditions.Count == 0 ? null : clone;
            }
            else if (Condition is PropertyCondition tempPc && tempPc.Property == AutomationElement.ControlTypeProperty)
            {
                prefix = ((ControlType)tempPc.Value).ProgrammaticName.Split('.').Last();
                condition = null;
            }

            string conditionStr = null;
            if (condition != null)
            {
                conditionStr = condition.GetDescription();
                if (conditionStr.StartsWith("(") && conditionStr.EndsWith(")"))
                    conditionStr = conditionStr.Substring(1, conditionStr.Length - 2);
                conditionStr = $"[{conditionStr}]";
            }

            return $"{prefix}{conditionStr}";
        }
    }
}
