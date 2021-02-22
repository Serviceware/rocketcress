using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.ControlSearch.Conditions
{
    public class OrCondition : CompositionSearchConditionBase
    {
        public override SearchConditionOperator OperatorType => SearchConditionOperator.Or;

        public OrCondition(params ISearchCondition[] conditions) : base(conditions) { }
        public OrCondition(IEnumerable<ISearchCondition> conditions) : base(conditions) { }

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

        protected override SearchConditionBase CloneInternal()
        {
            return new OrCondition();
        }

        public override string GetDescription()
        {
            if (Conditions.Count == 0)
                return null;
            if (Conditions.Count == 1)
                return Conditions[0].GetDescription();
            return $"({string.Join(" or ", Conditions.Select(x => x.GetDescription()))})";
        }
    }
}
