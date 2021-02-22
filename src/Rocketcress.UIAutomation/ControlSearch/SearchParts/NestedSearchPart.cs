using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.ControlSearch.SearchParts
{
    public class NestedSearchPart : SearchPartBase, INestedSearchPart
    {
        public ISearchPart ChildPart
        {
            get => Parts[Parts.Count - 1];
            set => Parts[Parts.Count - 1] = value;
        }
        public IList<ISearchPart> Parts { get; set; }

        public override ISearchCondition Condition
        {
            get => base.Condition;
            set => throw new NotSupportedException("Setting the condition is not supporten on a nested search part.");
        }

        public NestedSearchPart(params ISearchPart[] parts) : this((IEnumerable<ISearchPart>)parts) { }
        public NestedSearchPart(IEnumerable<ISearchPart> parts)
        {
            Parts = parts.ToList();
        }

        protected override IEnumerable<AutomationElement> FindElementsInternal(AutomationElement element, TreeWalker treeWalker)
        {
            return FindElementsRecursive(element, treeWalker, 0);
        }

        private IEnumerable<AutomationElement> FindElementsRecursive(AutomationElement element, TreeWalker treeWalker, int currentPartIndex)
        {
            foreach (var match in Parts[currentPartIndex].FindElements(element, treeWalker))
            {
                if (currentPartIndex + 1 < Parts.Count)
                {
                    foreach (var recursiveMatch in FindElementsRecursive(match, treeWalker, currentPartIndex + 1))
                        yield return recursiveMatch;
                }
                else
                    yield return match;
            }
        }

        protected override SearchPartBase CloneInternal()
        {
            var parts = Parts.Select(x => (ISearchPart)x.Clone());
            return new NestedSearchPart(parts);
        }

        public override string GetDescription()
        {
            return string.Concat(Parts.Select(x => x.GetDescription()));
        }
    }
}
