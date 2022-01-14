namespace Rocketcress.UIAutomation.ControlSearch.SearchParts;

public class RelativesSearchPart : SearchPartBase, INestedSearchPart
{
    public RelativesSearchOptions Options { get; set; }
    public ISearchPart ChildPart { get; set; }

    public RelativesSearchPart(RelativesSearchOptions options)
        : this(options, null, null)
    {
    }

    public RelativesSearchPart(RelativesSearchOptions options, ISearchCondition condition)
        : this(options, condition, null)
    {
    }

    public RelativesSearchPart(RelativesSearchOptions options, ISearchCondition condition, ISearchPart childPart)
    {
        Options = options;
        ChildPart = childPart;
        Condition = condition;
    }

    protected override IEnumerable<AutomationElement> FindElementsInternal(AutomationElement element, TreeWalker treeWalker)
        => FindElements(element, treeWalker, Options, Condition, ChildPart);

    public static IEnumerable<AutomationElement> FindElements(AutomationElement element, TreeWalker treeWalker, RelativesSearchOptions options, ISearchCondition condition, ISearchPart childPart)
    {
        if (options.HasFlag(RelativesSearchOptions.IncludeElement))
        {
            foreach (var res in SearchEngine.FindElements(element, treeWalker, condition, childPart))
                yield return res;
        }

        if (options.HasFlag(RelativesSearchOptions.PrecedingRelatives))
        {
            foreach (var res in FindElements(element, treeWalker, condition, childPart, (e, t) => t.GetPreviousSibling(e)))
                yield return res;
        }

        if (options.HasFlag(RelativesSearchOptions.SubsequentRelatives))
        {
            foreach (var res in FindElements(element, treeWalker, condition, childPart, (e, t) => t.GetNextSibling(e)))
                yield return res;
        }
    }

    protected override SearchPartBase CloneInternal()
    {
        var childPart = (ISearchPart)ChildPart?.Clone();
        return new RelativesSearchPart(Options, null, childPart);
    }

    private static IEnumerable<AutomationElement> FindElements(AutomationElement element, TreeWalker treeWalker, ISearchCondition condition, ISearchPart childPart, Func<AutomationElement, TreeWalker, AutomationElement> nextElement)
    {
        var current = nextElement(element, treeWalker);
        while (current != null)
        {
            foreach (var res in SearchEngine.FindElements(current, treeWalker, condition, childPart))
                yield return res;
            current = nextElement(current, treeWalker);
        }
    }

    public override string GetDescription()
    {
        return "/" +
            (Options.HasFlag(RelativesSearchOptions.IncludeElement) ? "." : string.Empty) +
            (Options.HasFlag(RelativesSearchOptions.PrecedingRelatives) ? "<" : string.Empty) +
            (Options.HasFlag(RelativesSearchOptions.SubsequentRelatives) ? ">" : string.Empty) +
            GetConditionDescription() +
            (ChildPart == null ? string.Empty : ChildPart.GetDescription());
    }
}

public enum RelativesSearchOptions
{
    None = 0x0,
    PrecedingRelatives = 0x1,
    SubsequentRelatives = 0x2,
    AllRelatives = 0x3,
    IncludeElement = 0x4,
}
