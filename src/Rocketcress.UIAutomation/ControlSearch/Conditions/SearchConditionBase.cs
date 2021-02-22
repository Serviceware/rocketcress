using System.Windows.Automation;

namespace Rocketcress.UIAutomation.ControlSearch.Conditions
{
    public abstract class SearchConditionBase : ISearchCondition
    {
        public abstract bool Check(AutomationElement element, TreeWalker treeWalker);

        public virtual object Clone()
        {
            var clone = CloneInternal();
            return clone;
        }

        protected abstract SearchConditionBase CloneInternal();
        public abstract string GetDescription();
    }
}
