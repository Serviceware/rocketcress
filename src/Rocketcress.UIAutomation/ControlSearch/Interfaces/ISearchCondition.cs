using System;
using System.Collections.Generic;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.ControlSearch
{
    public interface ISearchCondition : ICloneable
    {
        bool Check(AutomationElement element, TreeWalker treeWalker);
        string GetDescription();
    }

    public interface ICompositeSearchCondition : ISearchCondition
    {
        SearchConditionOperator OperatorType { get; }
        IList<ISearchCondition> Conditions { get; set; }
    }

    public enum SearchConditionOperator
    {
        And,
        Or,
    }
}
