namespace Rocketcress.UIAutomation.ControlSearch.Conditions
{
    public delegate bool FunctionConditionDelegate(AutomationElement element, TreeWalker treeWalker);

    public class FunctionCondition : SearchConditionBase
    {
        public string ConditionName { get; internal set; }
        public FunctionConditionDelegate Condition { get; internal set; }

        public FunctionCondition(string conditionName, FunctionConditionDelegate condition)
        {
            ConditionName = conditionName ?? throw new ArgumentNullException(nameof(conditionName));
            Condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }

        public override bool Check(AutomationElement element, TreeWalker treeWalker)
        {
            return Condition(element, treeWalker);
        }

        protected override SearchConditionBase CloneInternal()
        {
            return new FunctionCondition(ConditionName, Condition);
        }

        public override string GetDescription()
        {
            return $"func('{ConditionName}')";
        }
    }
}
