using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.ControlSearch.SearchParts
{
    public class AncestorsSearchPart : SearchPartBase, INestedSearchPart, IDepthSearchPart
    {
        public int MaxDepth { get; set; }
        public ISearchPart ChildPart { get; set; }

        public AncestorsSearchPart(int maxDepth) : this(maxDepth, null, null) { }
        public AncestorsSearchPart(int maxDepth, ISearchCondition condition) : this (maxDepth, condition, null) { }
        public AncestorsSearchPart(int maxDepth, ISearchCondition condition, ISearchPart childPart)
        {
            MaxDepth = maxDepth;
            ChildPart = childPart;
            Condition = condition;
        }

        protected override IEnumerable<AutomationElement> FindElementsInternal(AutomationElement element, TreeWalker treeWalker)
        {
            IEnumerable<AutomationElement> currentLevel = new[] { element };
            List<AutomationElement> nextLevel;
            var depth = 0;

            while (currentLevel.Any() && (depth < MaxDepth || MaxDepth < 0))
            {
                nextLevel = new List<AutomationElement>();

                foreach (var item in currentLevel)
                {
                    var current = treeWalker.GetParent(item);
                    if (Condition?.Check(current, treeWalker) != false)
                    {
                        if (ChildPart == null)
                            yield return current;
                        else
                        {
                            foreach (var res in ChildPart.FindElements(current, treeWalker))
                                yield return res;
                        }
                    }
                    nextLevel.Add(current);
                }

                currentLevel = nextLevel;
                depth++;
            }
        }

        protected override SearchPartBase CloneInternal()
        {
            var childPart = (ISearchPart)ChildPart?.Clone();
            return new AncestorsSearchPart(MaxDepth, null, childPart);
        }

        public override string GetDescription()
        {
            var prefix = "/" + (MaxDepth == 1 ? ".." : MaxDepth < 0 ? "..." : $"...{{{MaxDepth}}}");
            return prefix + GetConditionDescription() + GetSkipTakeDescription();
        }
    }
}
