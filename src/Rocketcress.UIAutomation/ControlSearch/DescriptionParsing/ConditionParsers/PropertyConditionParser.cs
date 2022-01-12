using PropertyCondition = Rocketcress.UIAutomation.ControlSearch.Conditions.PropertyCondition;

namespace Rocketcress.UIAutomation.ControlSearch.DescriptionParsing.ConditionParsers;

internal class PropertyConditionParser : IConditionParser
{
    public bool IsMatch(string condition) => RegularExpressions.PropertyConditionRegex.IsMatch(condition);

    public ISearchCondition ParseCondition(string condition)
    {
        var conditionMatch = RegularExpressions.PropertyConditionRegex.Match(condition);

        var propertyName = conditionMatch.Groups["Property"].Value;
        var property = UIAutomationSearchDescriptionHelper.GetPropertyByName(propertyName);
        var value = conditionMatch.Groups["Value"].Value;

        return new PropertyCondition(property, value);
    }
}
