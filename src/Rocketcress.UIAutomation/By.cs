using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.ControlSearch;
using Rocketcress.UIAutomation.ControlSearch.Conditions;
using Rocketcress.UIAutomation.ControlSearch.SearchParts;
using Rocketcress.UIAutomation.ControlSearch.DescriptionParsing;
using Rocketcress.Core.Base;
using Rocketcress.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using PropertyCondition = Rocketcress.UIAutomation.ControlSearch.Conditions.PropertyCondition;

namespace Rocketcress.UIAutomation
{
    public enum ByOptions
    {
        None = 0x0,
        UseContains = 0x1,
        IgnoreCase = 0x2,
        Unequal = 0x4
    }

    public class By : TestObjectBase
    {
        public static By Empty => new By();
        
        public NestedSearchPart RootSearchPart { get; private set; }
        public CompositeSearchPart ElementSearchPart { get; private set; }

        public IEnumerable<PropertyCondition> PropertyConditions => ElementSearchPart.GetConditionList().OfType<PropertyCondition>();

        protected By()
        {
            ElementSearchPart = new CompositeSearchPart(new DescendantsSearchPart(1));
            RootSearchPart = new NestedSearchPart(ElementSearchPart);
        }

        #region Public Methods
        public AutomationElement FindFirst() => SearchEngine.FindFirst(this);
        public AutomationElement FindFirst(AutomationElement parent) => SearchEngine.FindFirst(this, parent);

        public IEnumerable<AutomationElement> FindAll() => SearchEngine.FindAll(this);
        public IEnumerable<AutomationElement> FindAll(AutomationElement parent) => SearchEngine.FindAll(this, parent);

        public string GetSearchDescription()
        {
            return RootSearchPart.GetDescription();
        }

        public virtual By Append(By by) => Append(by, true, true);
        public virtual By Append(By by, bool overwriteSkipTake, bool overwriteScope)
        {
            foreach (var part in by.RootSearchPart.Parts.Take(by.RootSearchPart.Parts.Count - 1))
                RootSearchPart.Parts.Insert(RootSearchPart.Parts.Count - 1, (ISearchPart)part.Clone());

            if (by.ElementSearchPart.Condition != null)
                ElementSearchPart.AppendCondition((ISearchCondition)by.ElementSearchPart.Condition.Clone(), SearchConditionOperator.And);
            if (overwriteScope)
            {
                ElementSearchPart.Parts.Clear();
                ElementSearchPart.Parts.AddRange(by.ElementSearchPart.Parts.Select(x => (ISearchPart)x.Clone()));
            }

            if (overwriteSkipTake)
            {
                if (by.ElementSearchPart.SkipCount.HasValue)
                    ElementSearchPart.SkipCount = by.ElementSearchPart.SkipCount;
                if (by.ElementSearchPart.TakeCount.HasValue)
                    ElementSearchPart.TakeCount = by.ElementSearchPart.TakeCount;
            }

            return this;
        }

        public override string ToString() => GetSearchDescription();
        #endregion
        
        private By AndCondition(ISearchCondition condition)
        {
            ElementSearchPart.AppendCondition(condition, SearchConditionOperator.And);
            return this;
        }

        #region Condition Functions
        /// <summary>
        /// Create a new location key from a search description in the CPath Syntax. (For more info in CPath Syntax see the cpath-syntax.txt in Rocketcress.UIAutomation project)
        /// </summary>
        /// <param name="cpath">The CPath to parse.</param>
        public static By CPath(string cpath) => new By().AndCPath(cpath);
        /// <summary>
        /// Adds conditions from a search description in the CPath Syntax to this location key. (For more info in CPath Syntax see the cpath-syntax.txt in Rocketcress.UIAutomation project)
        /// </summary>
        /// <param name="cpath">The CPath to parse.</param>
        public virtual By AndCPath(string cpath)
        {
            var xpathPart = ControlSearchDescriptionParser.ParseSearchDescription(cpath);

            var xpathElementPart = xpathPart.Parts.Last();
            CompositeSearchPart elementPart;
            if (xpathElementPart is CompositeSearchPart compositeXPathPart)
                elementPart = compositeXPathPart;
            else
            {
                elementPart = new CompositeSearchPart(xpathElementPart);
                elementPart.AppendCondition(xpathElementPart.Condition, SearchConditionOperator.And);
                elementPart.SkipCount = (xpathElementPart as SearchPartBase)?.SkipCount;
                elementPart.TakeCount = (xpathElementPart as SearchPartBase)?.TakeCount;
                xpathElementPart.Condition = null;
                if (xpathElementPart is SearchPartBase xpathElementPartBase)
                {
                    xpathElementPartBase.SkipCount = null;
                    xpathElementPartBase.TakeCount = null;
                }
                xpathPart.Parts[xpathPart.Parts.Count - 1] = elementPart;
            }

            var xpathElementCondition = elementPart.Condition;
            elementPart.Condition = ElementSearchPart.Condition;
            elementPart.AppendCondition(xpathElementCondition, SearchConditionOperator.And);

            RootSearchPart = xpathPart;
            ElementSearchPart = elementPart;
            return this;
        }

        public static By Condition(FunctionConditionDelegate condition, string conditionName) => new By().AndCondition(condition, conditionName);
        public virtual By AndCondition(FunctionConditionDelegate condition, string conditionName)
        {
            var c = new FunctionCondition(conditionName, condition);
            return AndCondition(c);
        }

        public static By ChildOf(By parent) => new By().AndChildOf(parent);
        public virtual By AndChildOf(By parent)
        {
            RootSearchPart.Parts.Insert(RootSearchPart.Parts.Count - 1, (ISearchPart)parent.RootSearchPart.Clone());
            return this;
        }

        public static By HasChild(By child) => new By().AndHasChild(child);
        public virtual By AndHasChild(By child)
        {
            var c = new HasElementCondition(child.RootSearchPart);
            return AndCondition(c);
        }

        public static By RelativeTo(By relative, int distance) => new By().AndRelativeTo(relative, distance);
        public virtual By AndRelativeTo(By relative, int distance)
        {
            var c = new RelativeToCondition(distance, relative.ElementSearchPart.Condition);
            return AndCondition(c);
        }

        public static By Scope(TreeScope scope) => new By().AndScope(scope);
        public virtual By AndScope(TreeScope scope)
        {
            ElementSearchPart.Parts.Clear();
            if (scope.HasFlag(TreeScope.Element) || scope.HasFlag(TreeScope.Subtree))
                ElementSearchPart.Parts.Add(new IdentitySearchPart());
            if (scope.HasFlag(TreeScope.Ancestors) || scope.HasFlag(TreeScope.Parent))
                ElementSearchPart.Parts.Add(new AncestorsSearchPart(scope.HasFlag(TreeScope.Ancestors) ? -1 : 1));
            if (scope.HasFlag(TreeScope.Descendants) || scope.HasFlag(TreeScope.Children) || scope.HasFlag(TreeScope.Subtree))
                ElementSearchPart.Parts.Add(new DescendantsSearchPart(scope.HasFlag(TreeScope.Descendants) || scope.HasFlag(TreeScope.Subtree) ? -1 : 1));
            return this;
        }

        public static By Descendants(int maxDepth) => new By().AndDescendants(maxDepth);
        public virtual By AndDescendants(int maxDepth)
        {
            var desc = ElementSearchPart.Parts.OfType<DescendantsSearchPart>().SingleOrDefault();
            if (desc == null)
                ElementSearchPart.Parts.Add(new DescendantsSearchPart(maxDepth));
            else
                desc.MaxDepth = maxDepth;
            return this;
        }

        public static By Skip(int value) => new By().AndSkip(value);
        public virtual By AndSkip(int value)
        {
            ElementSearchPart.SkipCount = value;
            return this;
        }

        public static By Take(int value) => new By().AndTake(value);
        public virtual By AndTake(int value)
        {
            ElementSearchPart.TakeCount = value;
            return this;
        }

        public static By MaxDepth(int value) => new By().AndMaxDepth(value);
        public virtual By AndMaxDepth(int value)
        {
            ElementSearchPart.Parts.OfType<IDepthSearchPart>().ForEach(x => x.MaxDepth = value);
            return this;
        }

        public static By Property(AutomationProperty property, object value) => new By().AndProperty(property, value);
        public static By Property(AutomationProperty property, object value, ByOptions options) => new By().AndProperty(property, value, options);
        public virtual By AndProperty(AutomationProperty property, object value) => AndProperty(property, value, ByOptions.None);
        public virtual By AndProperty(AutomationProperty property, object value, ByOptions options)
        {
            return AndCondition(new PropertyCondition(property, value, options));
        }

        public virtual By ClearProperty(AutomationProperty property)
        {
            ElementSearchPart.RemoveCondition<PropertyCondition>(x => x.Property == property);
            return this;
        }

        public static By PatternAvailable<TPattern>() where TPattern : BasePattern => new By().AndPatternAvailable(typeof(TPattern), true);
        public static By PatternAvailable(Type patternType) => new By().AndPatternAvailable(patternType, true);
        public static By PatternNotAvailable<TPattern>() where TPattern : BasePattern => new By().AndPatternAvailable(typeof(TPattern), false);
        public static By PatternNotAvailable(Type patternType) => new By().AndPatternAvailable(patternType, false);
        public virtual By AndPatternAvailable<TPattern>() where TPattern : BasePattern => AndPatternAvailable(typeof(TPattern), true);
        public virtual By AndPatternAvailable(Type patternType) => AndPatternAvailable(patternType, true);
        public virtual By AndPatternNotAvailable<TPattern>() where TPattern : BasePattern => AndPatternAvailable(typeof(TPattern), false);
        public virtual By AndPatternNotAvailable(Type patternType) => AndPatternAvailable(patternType, false);
        protected virtual By AndPatternAvailable(Type patternType, bool shouldBeAvailable)
        {
            if (patternType == null)
                throw new ArgumentNullException(nameof(patternType));
            if (!typeof(BasePattern).IsAssignableFrom(patternType))
                throw new ArgumentException($"Only types that are derived from \"{typeof(BasePattern).FullName}\" are supported.", nameof(patternType));
            var c = new PropertyCondition(PatternUtility.GetIsPatternAvailableProperty(patternType), shouldBeAvailable, ByOptions.None);
            return AndCondition(c);
        }

        public static By Framework(string value) => new By().AndFramework(value);
        public static By Framework(string value, ByOptions options) => new By().AndFramework(value, options);
        public virtual By AndFramework(string value) => AndProperty(AutomationElement.FrameworkIdProperty, value);
        public virtual By AndFramework(string value, ByOptions options) => AndProperty(AutomationElement.FrameworkIdProperty, value, options);

        public static By ClassName(string value) => new By().AndClassName(value);
        public static By ClassName(string value, ByOptions options) => new By().AndClassName(value, options);
        public virtual By AndClassName(string value) => AndProperty(AutomationElement.ClassNameProperty, value);
        public virtual By AndClassName(string value, ByOptions options) => AndProperty(AutomationElement.ClassNameProperty, value, options);

        public static By ControlType(ControlType value) => Property(AutomationElement.ControlTypeProperty, value);
        public virtual By AndControlType(ControlType value) => AndProperty(AutomationElement.ControlTypeProperty, value);

        public static By Name(string value) => new By().AndName(value);
        public static By Name(string value, ByOptions options) => new By().AndName(value, options);
        public virtual By AndName(string value) => AndProperty(AutomationElement.NameProperty, value);
        public virtual By AndName(string value, ByOptions options) => AndProperty(AutomationElement.NameProperty, value, options);

        public static By HelpText(string value) => new By().AndHelpText(value);
        public static By HelpText(string value, ByOptions options) => new By().AndHelpText(value, options);
        public virtual By AndHelpText(string value) => AndProperty(AutomationElement.HelpTextProperty, value);
        public virtual By AndHelpText(string value, ByOptions options) => AndProperty(AutomationElement.HelpTextProperty, value, options);

        public static By ProcessId(int value) => Property(AutomationElement.ProcessIdProperty, value);
        public virtual By AndProcessId(int value) => AndProperty(AutomationElement.ProcessIdProperty, value);

        public static By AcceleratorKey(string value) => new By().AndAcceleratorKey(value);
        public static By AcceleratorKey(string value, ByOptions options) => new By().AndAcceleratorKey(value, options);
        public virtual By AndAcceleratorKey(string value) => AndProperty(AutomationElement.AcceleratorKeyProperty, value);
        public virtual By AndAcceleratorKey(string value, ByOptions options) => AndProperty(AutomationElement.AcceleratorKeyProperty, value, options);

        public static By AccessKey(string value) => new By().AndAccessKey(value);
        public static By AccessKey(string value, ByOptions options) => new By().AndAccessKey(value, options);
        public virtual By AndAccessKey(string value) => AndProperty(AutomationElement.AccessKeyProperty, value);
        public virtual By AndAccessKey(string value, ByOptions options) => AndProperty(AutomationElement.AccessKeyProperty, value, options);

        public static By AutomationId(string value) => new By().AndAutomationId(value);
        public static By AutomationId(string value, ByOptions options) => new By().AndAutomationId(value, options);
        public virtual By AndAutomationId(string value) => AndProperty(AutomationElement.AutomationIdProperty, value);
        public virtual By AndAutomationId(string value, ByOptions options) => AndProperty(AutomationElement.AutomationIdProperty, value, options);

        public static By ItemStatus(string value) => new By().AndItemStatus(value);
        public static By ItemStatus(string value, ByOptions options) => new By().AndItemStatus(value, options);
        public virtual By AndItemStatus(string value) => AndProperty(AutomationElement.ItemStatusProperty, value);
        public virtual By AndItemStatus(string value, ByOptions options) => AndProperty(AutomationElement.ItemStatusProperty, value, options);
        #endregion
    }
}
