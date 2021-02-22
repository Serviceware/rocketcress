using System.Collections.Generic;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.ControlSearch.SearchParts
{
    public class IdentitySearchPart : SearchPartBase
    {        
        public IdentitySearchPart() : this(null) { }
        public IdentitySearchPart(ISearchCondition condition)
        {
            Condition = condition;
        }

        protected override IEnumerable<AutomationElement> FindElementsInternal(AutomationElement element, TreeWalker treeWalker)
        {
            if (Condition?.Check(element, treeWalker) != false)
                yield return element;
        }

        protected override SearchPartBase CloneInternal() => new IdentitySearchPart();

        public override string GetDescription()
        {
            return "/_" + GetConditionDescription() + GetSkipTakeDescription();
        }
    }
}
