namespace Rocketcress.UIAutomation.ControlSearch.SearchParts;

public class CompositeSearchPart : SearchPartBase
{
    public IList<ISearchPart> Parts { get; set; }

    public CompositeSearchPart(params ISearchPart[] parts)
        : this((IEnumerable<ISearchPart>)parts)
    {
    }

    public CompositeSearchPart(IEnumerable<ISearchPart> parts)
    {
        Parts = parts?.ToList() ?? throw new ArgumentNullException(nameof(parts));
    }

    protected override IEnumerable<AutomationElement> FindElementsInternal(AutomationElement element, TreeWalker treeWalker)
    {
        return Parts.SelectMany(x => x.FindElements(element, treeWalker)).Where(x => Condition?.Check(x, treeWalker) != false);
    }

    protected override SearchPartBase CloneInternal()
    {
        var parts = Parts.Select(x => (ISearchPart)x.Clone());
        return new CompositeSearchPart(parts);
    }

    public override string GetDescription()
    {
        if (Parts.Count == 0)
            return null;

        string result;
        if (Parts.Count == 1)
            result = Parts[0].GetDescription();
        else
            result = $"{string.Join("|", Parts.Select(x => x.GetDescription()))}";

        result += GetConditionDescription() + GetSkipTakeDescription();

        return result;
    }
}
