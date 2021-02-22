using System;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.ControlSearch.Conditions
{
    public class NotCondition : SearchConditionBase
    {
        public ISearchCondition SubCondition { get; set; }

        public NotCondition(ISearchCondition subCondition)
        {
            SubCondition = subCondition ?? throw new ArgumentNullException(nameof(subCondition));
        }

        public override bool Check(AutomationElement element, TreeWalker treeWalker)
        {
            return !SubCondition.Check(element, treeWalker);
        }

        protected override SearchConditionBase CloneInternal()
        {
            var subCondition = (ISearchCondition)SubCondition.Clone();
            return new NotCondition(subCondition);
        }

        public override string GetDescription()
        {
            return $"not({SubCondition.GetDescription()})";
        }
    }
}
