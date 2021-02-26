namespace Rocketcress.UIAutomation.ControlSearch.DescriptionParsing
{
    internal interface IConditionParser
    {
        bool IsMatch(string condition);
        ISearchCondition ParseCondition(string condition);
    }
}
