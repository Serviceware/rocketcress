using Rocketcress.UIAutomation.ControlSearch.Conditions;
using Rocketcress.UIAutomation.ControlSearch.SearchParts;
using System.Text.RegularExpressions;

namespace Rocketcress.UIAutomation.ControlSearch.DescriptionParsing.ConditionParsers;

internal class HasElementConditionParser : IConditionParser
{
    public bool IsMatch(string condition) => RegularExpressions.SplitPartsRegex.IsMatch(condition);

    public ISearchCondition ParseCondition(string condition)
    {
        var part = new NestedSearchPart();

        bool isFirst = true;
        foreach (Match childMatch in RegularExpressions.SplitPartsRegex.Matches(condition))
        {
            if (string.IsNullOrWhiteSpace(childMatch.Value))
                continue;

            var path = childMatch.Groups["Path"].Value;
            if (isFirst && string.IsNullOrEmpty(path))
                path = "/" + path;

            var p = ControlSearchDescriptionParser.ParsePart(path, childMatch.Groups["ControlType"].Value, childMatch.Groups["Condition"].Value, childMatch.Groups["Skip"].Value);
            part.Parts.Add(p);
            isFirst = false;
        }

        return new HasElementCondition(part.Parts.Count == 1 ? part.Parts[0] : part);
    }
}
