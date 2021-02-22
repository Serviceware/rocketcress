using Rocketcress.UIAutomation.ControlSearch.SearchParts;
using System;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.ControlSearch.Conditions
{
    public class RelativeToCondition : SearchConditionBase
    {
        public int Distance { get; set; }
        public ISearchCondition Condition { get; set; }

        public RelativeToCondition(int distance, ISearchCondition condition)
        {
            Distance = distance;
            Condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }

        public override bool Check(AutomationElement element, TreeWalker treeWalker)
        {
            bool result;
            if (Distance == 0)
                result = Condition.Check(element, treeWalker);
            else
            {
                var options = Distance > 0 ? RelativesSearchOptions.PrecedingRelatives : RelativesSearchOptions.SubsequentRelatives;
                result = RelativesSearchPart.FindElements(element, treeWalker, options, Condition, null).Skip(Math.Abs(Distance) - 1).Any();
            }
            return result;
        }

        protected override SearchConditionBase CloneInternal()
        {
            var condition = (ISearchCondition)Condition.Clone();
            return new RelativeToCondition(Distance, condition);
        }

        public override string GetDescription()
        {
            return $"relativeto({Condition.GetDescription()}, {Distance})";
        }
    }
}
