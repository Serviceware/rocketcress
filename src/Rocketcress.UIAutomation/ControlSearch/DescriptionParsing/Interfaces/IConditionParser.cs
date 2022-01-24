namespace Rocketcress.UIAutomation.ControlSearch.DescriptionParsing;

#pragma warning disable SA1600 // Elements should be documented

internal interface IConditionParser
{
    bool IsMatch(string condition);
    ISearchCondition ParseCondition(string condition);
}
