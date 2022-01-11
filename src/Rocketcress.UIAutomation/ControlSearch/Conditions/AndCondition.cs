namespace Rocketcress.UIAutomation.ControlSearch.Conditions
{
    public class AndCondition : CompositionSearchConditionBase
    {
        public override SearchConditionOperator OperatorType => SearchConditionOperator.And;

        public AndCondition(params ISearchCondition[] conditions)
            : base(conditions)
        {
        }

        public AndCondition(IEnumerable<ISearchCondition> conditions)
            : base(conditions)
        {
        }

        public override bool Check(AutomationElement element, TreeWalker treeWalker)
        {
            var result = true;
            foreach (var condition in Conditions)
            {
                result = condition.Check(element, treeWalker);
                if (!result)
                    break;
            }

            return result;
        }

        protected override SearchConditionBase CloneInternal()
        {
            return new AndCondition();
        }

        public override string GetDescription()
        {
            if (Conditions.Count == 0)
                return null;
            if (Conditions.Count == 1)
                return Conditions[0].GetDescription();
            return $"({string.Join(" and ", Conditions.Select(x => x.GetDescription()))})";
        }
    }
}
