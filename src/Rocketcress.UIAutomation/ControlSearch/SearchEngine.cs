namespace Rocketcress.UIAutomation.ControlSearch
{
    public static class SearchEngine
    {
        public static AutomationElement FindFirst(By locationKey) => FindFirst(locationKey, AutomationElement.RootElement);
        public static AutomationElement FindFirst(By locationKey, AutomationElement parent) => FindAll(locationKey, parent).FirstOrDefault();

        public static IEnumerable<AutomationElement> FindAll(By locationKey) => FindAll(locationKey, AutomationElement.RootElement);
        public static IEnumerable<AutomationElement> FindAll(By locationKey, AutomationElement parent)
        {
            return locationKey.RootSearchPart.FindElements(parent, TreeWalker.RawViewWalker);
        }

        internal static IEnumerable<AutomationElement> FindElements(AutomationElement element, TreeWalker treeWalker, ISearchCondition condition, ISearchPart childPart)
        {
            if (condition?.Check(element, treeWalker) != false)
            {
                if (childPart == null)
                {
                    yield return element;
                }
                else
                {
                    foreach (var x in childPart.FindElements(element, treeWalker))
                        yield return x;
                }
            }
        }
    }
}
