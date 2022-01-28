using Rocketcress.UIAutomation.ControlSearch.DescriptionParsing.ConditionParsers;
using Rocketcress.UIAutomation.ControlSearch.SearchParts;
using System.Text.RegularExpressions;
using AndCondition = Rocketcress.UIAutomation.ControlSearch.Conditions.AndCondition;
using OrCondition = Rocketcress.UIAutomation.ControlSearch.Conditions.OrCondition;
using PropertyCondition = Rocketcress.UIAutomation.ControlSearch.Conditions.PropertyCondition;

namespace Rocketcress.UIAutomation.ControlSearch.DescriptionParsing;

internal class ControlSearchDescriptionParser
{
    private const int DefaultDescendantsMaxDepth = 5;
    private const int DefaultAncestorsMaxDepth = 5;

    private static readonly IList<IConditionParser> _conditionParsers;

    static ControlSearchDescriptionParser()
    {
        _conditionParsers = new List<IConditionParser>
            {
                new PropertyConditionParser(),
                new FunctionConditionParser(),
                new HasElementConditionParser(),
            };
    }

    public static NestedSearchPart ParseSearchDescription(string searchDescription)
    {
        var result = new NestedSearchPart();

        foreach (Match part in RegularExpressions.SplitPartsRegex.Matches(searchDescription))
        {
            if (string.IsNullOrWhiteSpace(part.Value))
                continue;
            var p = ParsePart(part.Groups["Path"].Value, part.Groups["ControlType"].Value, part.Groups["Condition"].Value, part.Groups["Skip"].Value);
            result.Parts.Add(p);
        }

        return result;
    }

    public static ISearchPart ParsePart(string pathGroup, string controlTypeGroup, string conditionGroup, string skipGroup)
    {
        var result = CreateSearchPartFromPath(pathGroup);

        if (!string.IsNullOrEmpty(controlTypeGroup) && controlTypeGroup != "*")
        {
            var controlType = UIAutomationSearchDescriptionHelper.GetControlTypeByName(controlTypeGroup);
            result.AppendCondition(new PropertyCondition(AutomationElement.ControlTypeProperty, controlType), SearchConditionOperator.And);
        }

        if (int.TryParse(skipGroup, out int instance))
            result.SkipCount = instance - 1;

        var condition = ParseCondition(conditionGroup);
        if (condition != null)
            result.AppendCondition(condition, SearchConditionOperator.And);

        return result;
    }

    public static ISearchCondition ParseCondition(string conditionGroup)
    {
        string currentOperator = null;
        var conditions = new List<ISearchCondition>();

        foreach (Match match in RegularExpressions.SplitConditionsRegex.Matches($"{conditionGroup}"))
        {
            if (match.Value == "and" || match.Value == "or")
            {
                if (!string.IsNullOrEmpty(currentOperator))
                    throw new InvalidOperationException($"There are two operators directly nearby: {currentOperator} -> {match.Value}");
                currentOperator = match.Value;
            }
            else
            {
                if (string.IsNullOrEmpty(currentOperator) && conditions.Count > 0)
                    throw new InvalidOperationException("Missing operator");

                var condition = ParseConditionPart(match.Value);

                if (string.IsNullOrEmpty(currentOperator) || currentOperator == "or")
                {
                    conditions.Add(condition);
                }
                else
                {
                    if (conditions.Last() is not AndCondition andCondition)
                    {
                        andCondition = new AndCondition(conditions.Last());
                        conditions.RemoveAt(conditions.Count - 1);
                        conditions.Add(andCondition);
                    }

                    andCondition.Conditions.Add(condition);
                }

                currentOperator = null;
            }
        }

        if (conditions.Count == 0)
            return null;
        else if (conditions.Count == 1)
            return conditions[0];
        return new OrCondition(conditions);
    }

    private static SearchPartBase CreateSearchPartFromPath(string pathGroup)
    {
        var result = new List<SearchPartBase>();

        foreach (var pathPart in pathGroup.Split('|'))
        {
            var pathPartMatch = RegularExpressions.SplitPartPathRegex.Match(pathPart);
            var pathPartPath = pathPartMatch.Groups["Path"].Value;
            var optPath = pathPartPath.Length > 1 && pathPartPath[0] == '/' && pathPartPath[1] != '/' ? pathPartPath.Substring(1) : pathPartPath;
            int? maxDepth = int.TryParse(pathPartMatch.Groups["MaxDepth"].Value, out int tempMd) ? tempMd : (int?)null;

            if (IsPathMatch(pathPartPath, optPath, null, string.Empty, "_", "."))
            {
                result.Add(new IdentitySearchPart());
            }
            else if (IsPathMatch(pathPartPath, optPath, ".."))
            {
                result.Add(new AncestorsSearchPart(maxDepth ?? DefaultAncestorsMaxDepth));
            }
            else if (IsPathMatch(pathPartPath, optPath, "../"))
            {
                result.Add(new NestedSearchPart(new AncestorsSearchPart(1), new DescendantsSearchPart(1)));
            }
            else if (IsPathMatch(pathPartPath, optPath, "./", "/"))
            {
                if (maxDepth.HasValue)
                    throw new InvalidOperationException("The path '/' or './' does not support max depth syntax '{xxx}'. Use './/{xxx}' or '//{xxx}' instead.");
                result.Add(new DescendantsSearchPart(1));
            }
            else if (IsPathMatch(pathPartPath, optPath, ".//", "//"))
            {
                result.Add(new DescendantsSearchPart(maxDepth ?? DefaultDescendantsMaxDepth));
            }
            else if (IsPathMatch(pathPartPath, optPath, "./<", "./.<", "./>", "./.>", "./<>", "./.<>", "<", ".<", ">", ".>", "<>", ".<>"))
            {
                var options = RelativesSearchOptions.None;
                var realPath = pathPartPath.Split('/').Last();
                if (realPath.Contains("."))
                    options |= RelativesSearchOptions.IncludeElement;
                if (realPath.Contains("<"))
                    options |= RelativesSearchOptions.PrecedingRelatives;
                if (realPath.Contains(">"))
                    options |= RelativesSearchOptions.SubsequentRelatives;
                result.Add(new RelativesSearchPart(options));
            }
            else
            {
                throw new InvalidOperationException($"Invalid path syntax: '{pathPartPath}'");
            }
        }

        return result.Count == 1 ? result[0] : new CompositeSearchPart(result);

        bool IsPathMatch(string path, string optionalPath, params string[] validPaths)
        {
            return validPaths.Any(x => path == x || optionalPath == x);
        }
    }

    private static ISearchCondition ParseConditionPart(string condition)
    {
        ISearchCondition result;

        if (condition.StartsWith("(") && condition.EndsWith(")"))
        {
            result = ParseCondition(condition.Substring(1, condition.Length - 2));
        }
        else
        {
            var conditionParser = _conditionParsers.FirstOrDefault(x => x.IsMatch(condition));
            result = conditionParser?.ParseCondition(condition) ?? throw new InvalidOperationException($"Condition part '{condition}' could not be matched.");
        }

        return result;
    }
}
