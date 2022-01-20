namespace Rocketcress.UIAutomation.ControlSearch.Conditions;

public class HasElementCondition : SearchConditionBase
{
    public ISearchPart ElementPart { get; set; }

    public HasElementCondition(ISearchPart elementPart)
    {
        ElementPart = elementPart ?? throw new ArgumentNullException(nameof(elementPart));
    }

    public override bool Check(AutomationElement element, TreeWalker treeWalker)
    {
        return ElementPart.FindElements(element, treeWalker).Any();
    }

    protected override SearchConditionBase CloneInternal()
    {
        var elementPart = (ISearchPart)ElementPart.Clone();
        return new HasElementCondition(elementPart);
    }

    public override string GetDescription()
    {
        return $".{ElementPart.GetDescription()}";
    }
}
