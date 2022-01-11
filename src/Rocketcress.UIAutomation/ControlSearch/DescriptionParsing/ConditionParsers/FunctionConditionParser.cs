using System.Text.RegularExpressions;

namespace Rocketcress.UIAutomation.ControlSearch.DescriptionParsing.ConditionParsers
{
    internal class FunctionConditionParser : IConditionParser
    {
        public bool IsMatch(string condition) => RegularExpressions.FunctionConditionRegex.IsMatch(condition);

        public ISearchCondition ParseCondition(string condition)
        {
            var conditionMatch = RegularExpressions.FunctionConditionRegex.Match(condition);
            ISearchCondition result;

            var functionName = conditionMatch.Groups["Name"].Value;
            var parameters = (from x in RegularExpressions.FunctionConditionSplitParametersRegex.Matches(conditionMatch.Groups["Parameters"].Value).OfType<Match>()
                              where x.Success
                              select x.Value.Trim()).ToArray();
            if (functionName == "contains")
            {
                if (parameters.Length != 2)
                    throw new InvalidOperationException("The contains function needs two parameters.");
                if (RegularExpressions.SearchDescriptionElementRegex.IsMatch(parameters[0]) && RegularExpressions.SearchDescriptionStringRegex.IsMatch(parameters[1]))
                {
                    var property = UIAutomationSearchDescriptionHelper.GetPropertyByName(parameters[0].Substring(1));
                    var stringValue = parameters[1].Substring(1, parameters[1].Length - 2);
                    result = new Conditions.PropertyCondition(property, stringValue, ByOptions.UseContains);
                }
                else
                {
                    throw new InvalidOperationException("Parameters of contains functions faulty. " + conditionMatch.Value);
                }
            }
            else
            {
                throw new InvalidOperationException($"Function with the name '{functionName}' not found.");
            }

            return result;
        }
    }
}
