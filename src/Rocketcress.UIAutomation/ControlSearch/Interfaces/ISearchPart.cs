namespace Rocketcress.UIAutomation.ControlSearch
{
    public interface ISearchPart : ICloneable
    {
        ISearchCondition Condition { get; set; }

        IEnumerable<AutomationElement> FindElements(AutomationElement element, TreeWalker treeWalker);
        string GetDescription();
    }

    public interface INestedSearchPart : ISearchPart
    {
        ISearchPart ChildPart { get; set; }
    }

    public interface IDepthSearchPart : ISearchPart
    {
        int MaxDepth { get; set; }
    }
}
