using Rocketcress.Core.Base;
using Rocketcress.Core.Extensions;
using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.ControlSearch;
using Rocketcress.UIAutomation.ControlSearch.Conditions;
using Rocketcress.UIAutomation.ControlSearch.DescriptionParsing;
using Rocketcress.UIAutomation.ControlSearch.SearchParts;
using PropertyCondition = Rocketcress.UIAutomation.ControlSearch.Conditions.PropertyCondition;

namespace Rocketcress.UIAutomation;

/// <summary>
/// Options for the <see cref="By"/> conoditions.
/// </summary>
public enum ByOptions
{
    /// <summary>
    /// No options specified.
    /// </summary>
    None = 0x0,

    /// <summary>
    /// Specify to use contains instead of equals for property comparisons.
    /// </summary>
    UseContains = 0x1,

    /// <summary>
    /// Specify to ignore case for property comparisons.
    /// </summary>
    IgnoreCase = 0x2,

    /// <summary>
    /// Specify to inverse the property comparisons (property needs to not match the expected value).
    /// </summary>
    Unequal = 0x4,
}

/// <summary>
/// Represents a location key for finding UIAutomation elements.
/// </summary>
/// <seealso cref="Rocketcress.Core.Base.TestObjectBase" />
public class By : TestObjectBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="By"/> class.
    /// </summary>
    protected By()
    {
        ElementSearchPart = new CompositeSearchPart(new DescendantsSearchPart(1));
        RootSearchPart = new NestedSearchPart(ElementSearchPart);
    }

    /// <summary>
    /// Gets an empty <see cref="By"/> instance.
    /// </summary>
    public static By Empty => new();

    /// <summary>
    /// Gets the root search part.
    /// </summary>
    public NestedSearchPart RootSearchPart { get; private set; }

    /// <summary>
    /// Gets the element search part.
    /// </summary>
    public CompositeSearchPart ElementSearchPart { get; private set; }

    /// <summary>
    /// Gets the property conditions.
    /// </summary>
    public IEnumerable<PropertyCondition> PropertyConditions => ElementSearchPart.GetConditionList().OfType<PropertyCondition>();

    /// <summary>
    /// Create a new location key from a search description in the CPath Syntax (For more info in CPath Syntax see the README).
    /// </summary>
    /// <param name="cpath">The CPath to parse.</param>
    /// <returns><see cref="By"/> object represented by the <paramref name="cpath"/>.</returns>
    public static By CPath(string cpath) => new By().AndCPath(cpath);

    /// <summary>
    /// Creates a locatioon key with the specified function condition.
    /// </summary>
    /// <param name="condition">The condition.</param>
    /// <param name="conditionName">Name of the condition.</param>
    /// <returns>The created location key.</returns>
    public static By Condition(FunctionConditionDelegate condition, string conditionName) => new By().AndCondition(condition, conditionName);

    /// <summary>
    /// Creates a location key with a condition that checks whether the element is a child of the element found by a specified location key.
    /// </summary>
    /// <param name="parent">The location key used to find the parent element.</param>
    /// <returns>The created location key.</returns>
    public static By ChildOf(By parent) => new By().AndChildOf(parent);

    /// <summary>
    /// Creates a location key with a condition that checks whether the element has a child element found by a specified location key.
    /// </summary>
    /// <param name="child">The location key used to find the child element.</param>
    /// <returns>The created location key.</returns>
    public static By HasChild(By child) => new By().AndHasChild(child);

    /// <summary>
    /// Creates a location key with a condition that check whether the element is related to another element found by a specified location key.
    /// </summary>
    /// <param name="relative">The location key used to find the related element.</param>
    /// <param name="distance">The distance to the element (negative if the searched element is preceding the related element).</param>
    /// <returns>The created location key.</returns>
    public static By RelativeTo(By relative, int distance) => new By().AndRelativeTo(relative, distance);

    /// <summary>
    /// Creates a location key with a specified scope.
    /// </summary>
    /// <param name="scope">The scope.</param>
    /// <returns>The created location key.</returns>
    public static By Scope(TreeScope scope) => new By().AndScope(scope);

    /// <summary>
    /// Creates a location key with a condition that finds all descendant elements in the default scope.
    /// </summary>
    /// <param name="maxDepth">The maximum search depth.</param>
    /// <returns>The created location key.</returns>
    public static By Descendants(int maxDepth) => new By().AndDescendants(maxDepth);

    /// <summary>
    /// Creates a location key which skips the specified amount of matching elements when searching.
    /// </summary>
    /// <param name="value">The number of skipped matching elements.</param>
    /// <returns>The created location key.</returns>
    public static By Skip(int value) => new By().AndSkip(value);

    /// <summary>
    /// Creates a location key which finds only a specified maximum amount of matching elements when searching.
    /// </summary>
    /// <param name="value">The number of maximum matching elements.</param>
    /// <returns>The created location key.</returns>
    public static By Take(int value) => new By().AndTake(value);

    /// <summary>
    /// Creates a location key with the specified maximum search depth.
    /// </summary>
    /// <param name="value">The maximum search depth.</param>
    /// <returns>The created location key.</returns>
    public static By MaxDepth(int value) => new By().AndMaxDepth(value);

    /// <summary>
    /// Creates a location key with a condition that finds elements that have a specified property value.
    /// </summary>
    /// <param name="property">The property to check.</param>
    /// <param name="value">The value to match.</param>
    /// <returns>The created location key.</returns>
    public static By Property(AutomationProperty property, object value) => new By().AndProperty(property, value);

    /// <summary>
    /// Creates a location key with a condition that finds elements that have a specified property value.
    /// </summary>
    /// <param name="property">The property to check.</param>
    /// <param name="value">The value to match.</param>
    /// <param name="options">The search options.</param>
    /// <returns>The created location key.</returns>
    public static By Property(AutomationProperty property, object value, ByOptions options) => new By().AndProperty(property, value, options);

    /// <summary>
    /// Creates a location key with a condition that finds elements for which a specified pattern is available.
    /// </summary>
    /// <typeparam name="TPattern">The type of the pattern that should be available.</typeparam>
    /// <returns>The created location key.</returns>
    public static By PatternAvailable<TPattern>()
        where TPattern : BasePattern
        => new By().AndPatternAvailable(typeof(TPattern), true);

    /// <summary>
    /// Creates a location key with a condition that finds elements for which a specified pattern is available.
    /// </summary>
    /// <param name="patternType">The type of the pattern that should be available.</param>
    /// <returns>The created location key.</returns>
    public static By PatternAvailable(Type patternType) => new By().AndPatternAvailable(patternType, true);

    /// <summary>
    /// Creates a location key with a condition that finds elements for which a specified pattern is not available.
    /// </summary>
    /// <typeparam name="TPattern">The type of the pattern that should be available.</typeparam>
    /// <returns>The created location key.</returns>
    public static By PatternNotAvailable<TPattern>()
        where TPattern : BasePattern
        => new By().AndPatternAvailable(typeof(TPattern), false);

    /// <summary>
    /// Creates a location key with a condition that finds elements for which a specified pattern is not available.
    /// </summary>
    /// <param name="patternType">The type of the pattern that should be available.</param>
    /// <returns>The created location key.</returns>
    public static By PatternNotAvailable(Type patternType) => new By().AndPatternAvailable(patternType, false);

    /// <summary>
    /// Creates a location key with a condition that finds elements with a specified FrameworkId.
    /// </summary>
    /// <param name="value">The framework id (Find known ids in <see cref="FrameworkIds"/>).</param>
    /// <returns>The created location key.</returns>
    public static By Framework(string value) => new By().AndFramework(value);

    /// <summary>
    /// Creates a location key with a condition that finds elements with a specified FrameworkId.
    /// </summary>
    /// <param name="value">The framework id (Find known ids in <see cref="FrameworkIds"/>).</param>
    /// <param name="options">The search options.</param>
    /// <returns>The created location key.</returns>
    public static By Framework(string value, ByOptions options) => new By().AndFramework(value, options);

    /// <summary>
    /// Creates a location key with a condition that finds elements with a specified ClassName.
    /// </summary>
    /// <param name="value">The class name.</param>
    /// <returns>The created location key.</returns>
    public static By ClassName(string value) => new By().AndClassName(value);

    /// <summary>
    /// Creates a location key with a condition that finds elements with a specified ClassName.
    /// </summary>
    /// <param name="value">The class name.</param>
    /// <param name="options">The search options.</param>
    /// <returns>The created location key.</returns>
    public static By ClassName(string value, ByOptions options) => new By().AndClassName(value, options);

    /// <summary>
    /// Creates a location key with a condition that finds elements with a specified control type.
    /// </summary>
    /// <param name="value">The control type.</param>
    /// <returns>The created location key.</returns>
    public static By ControlType(ControlType value) => Property(AutomationElement.ControlTypeProperty, value);

    /// <summary>
    /// Creates a location key with a condition that finds elements with a specified name.
    /// </summary>
    /// <param name="value">The name.</param>
    /// <returns>The created location key.</returns>
    public static By Name(string value) => new By().AndName(value);

    /// <summary>
    /// Creates a location key with a condition that finds elements with a specified name.
    /// </summary>
    /// <param name="value">The name.</param>
    /// <param name="options">The search options.</param>
    /// <returns>The created location key.</returns>
    public static By Name(string value, ByOptions options) => new By().AndName(value, options);

    /// <summary>
    /// Creates a location key with a condition that finds elements with a specified help text.
    /// </summary>
    /// <param name="value">The help text.</param>
    /// <returns>The created location key.</returns>
    public static By HelpText(string value) => new By().AndHelpText(value);

    /// <summary>
    /// Creates a location key with a condition that finds elements with a specified help text.
    /// </summary>
    /// <param name="value">The help text.</param>
    /// <param name="options">The search options.</param>
    /// <returns>The created location key.</returns>
    public static By HelpText(string value, ByOptions options) => new By().AndHelpText(value, options);

    /// <summary>
    /// Creates a location key with a condition that finds elements hosted by a process with the specified id.
    /// </summary>
    /// <param name="value">The process id.</param>
    /// <returns>The created location key.</returns>
    public static By ProcessId(int value) => Property(AutomationElement.ProcessIdProperty, value);

    /// <summary>
    /// Creates a location key with a condition that finds elements with a specified accelerator key.
    /// </summary>
    /// <param name="value">The accelerator key.</param>
    /// <returns>The created location key.</returns>
    public static By AcceleratorKey(string value) => new By().AndAcceleratorKey(value);

    /// <summary>
    /// Creates a location key with a condition that finds elements with a specified accelerator key.
    /// </summary>
    /// <param name="value">The accelerator key.</param>
    /// <param name="options">The search options.</param>
    /// <returns>The created location key.</returns>
    public static By AcceleratorKey(string value, ByOptions options) => new By().AndAcceleratorKey(value, options);

    /// <summary>
    /// Creates a location key with a condition that finds elements with a specified access key.
    /// </summary>
    /// <param name="value">The access key.</param>
    /// <returns>The created location key.</returns>
    public static By AccessKey(string value) => new By().AndAccessKey(value);

    /// <summary>
    /// Creates a location key with a condition that finds elements with a specified access key.
    /// </summary>
    /// <param name="value">The access key.</param>
    /// <param name="options">The search options.</param>
    /// <returns>The created location key.</returns>
    public static By AccessKey(string value, ByOptions options) => new By().AndAccessKey(value, options);

    /// <summary>
    /// Creates a location key with a condition that finds elements with a specified automation id.
    /// </summary>
    /// <param name="value">The automation id.</param>
    /// <returns>The created location key.</returns>
    public static By AutomationId(string value) => new By().AndAutomationId(value);

    /// <summary>
    /// Creates a location key with a condition that finds elements with a specified automation id.
    /// </summary>
    /// <param name="value">The automation id.</param>
    /// <param name="options">The search options.</param>
    /// <returns>The created location key.</returns>
    public static By AutomationId(string value, ByOptions options) => new By().AndAutomationId(value, options);

    /// <summary>
    /// Creates a location key with a condition that finds elements with a specified item status.
    /// </summary>
    /// <param name="value">The item status.</param>
    /// <returns>The created location key.</returns>
    public static By ItemStatus(string value) => new By().AndItemStatus(value);

    /// <summary>
    /// Creates a location key with a condition that finds elements with a specified item status.
    /// </summary>
    /// <param name="value">The item status.</param>
    /// <param name="options">The search options.</param>
    /// <returns>The created location key.</returns>
    public static By ItemStatus(string value, ByOptions options) => new By().AndItemStatus(value, options);

    /// <summary>
    /// Finds the first element matching this location key.
    /// </summary>
    /// <returns>If an element was found it is returned; otherwise <c>null</c> is returned.</returns>
    public AutomationElement FindFirst() => SearchEngine.FindFirst(this);

    /// <summary>
    /// Finds the first element matching this location key starting the search from a given element.
    /// </summary>
    /// <param name="parent">The element to start the search from.</param>
    /// <returns>If an element was found it is returned; otherwise <c>null</c> is returned.</returns>
    public AutomationElement FindFirst(AutomationElement parent) => SearchEngine.FindFirst(this, parent);

    /// <summary>
    /// Finds all elements matching this location key.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> iterating through all matching elements.</returns>
    public IEnumerable<AutomationElement> FindAll() => SearchEngine.FindAll(this);

    /// <summary>
    /// Finds all elements matching this location key.
    /// </summary>
    /// <param name="parent">The element to start the search from.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> iterating through all matching elements.</returns>
    public IEnumerable<AutomationElement> FindAll(AutomationElement parent) => SearchEngine.FindAll(this, parent);

    /// <summary>
    /// Gets the description of this location key.
    /// </summary>
    /// <returns>The description of this location key.</returns>
    public string GetSearchDescription()
    {
        return RootSearchPart.GetDescription();
    }

    /// <summary>
    /// Appends the specified loocation keys conditions to this location key.
    /// </summary>
    /// <param name="by">The location key to append the conditions.</param>
    /// <returns>Retruns a self-reference to this location key.</returns>
    public virtual By Append(By by) => Append(by, true, true);

    /// <summary>
    /// Appends the specified loocation keys conditions to this location key.
    /// </summary>
    /// <param name="by">The location key to append the conditions.</param>
    /// <param name="overwriteSkipTake">If set to <c>true</c> skip and take properties are not applied.</param>
    /// <param name="overwriteScope">If set to <c>true</c> the scope property is not applied.</param>
    /// <returns>Retruns a self-reference to this location key.</returns>
    public virtual By Append(By by, bool overwriteSkipTake, bool overwriteScope)
    {
        Guard.NotNull(by);

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

    /// <inheritdoc/>
    public override string ToString() => GetSearchDescription();

    /// <summary>
    /// Adds conditions from a search description in the CPath Syntax to this location key (For more info in CPath Syntax see the README).
    /// </summary>
    /// <param name="cpath">The CPath to parse.</param>
    /// <returns>Same <see cref="By"/> object instance with added conditions represented by the <paramref name="cpath"/>.</returns>
    public virtual By AndCPath(string cpath)
    {
        var xpathPart = ControlSearchDescriptionParser.ParseSearchDescription(cpath);

        var xpathElementPart = xpathPart.Parts.Last();
        CompositeSearchPart elementPart;
        if (xpathElementPart is CompositeSearchPart compositeXPathPart)
        {
            elementPart = compositeXPathPart;
        }
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

    /// <summary>
    /// Adds the specified function condition to this location key.
    /// </summary>
    /// <param name="condition">The condition.</param>
    /// <param name="conditionName">Name of the condition.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndCondition(FunctionConditionDelegate condition, string conditionName)
    {
        var c = new FunctionCondition(conditionName, condition);
        return AndCondition(c);
    }

    /// <summary>
    /// Adds a condition that checks whether the element is a child of the element found by a specified location key, to this location key.
    /// </summary>
    /// <param name="parent">The location key used to find the parent element.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndChildOf(By parent)
    {
        Guard.NotNull(parent);
        RootSearchPart.Parts.Insert(RootSearchPart.Parts.Count - 1, (ISearchPart)parent.RootSearchPart.Clone());
        return this;
    }

    /// <summary>
    /// Adds a condition that checks whether the element has a child element found by a specified location key, to this location key.
    /// </summary>
    /// <param name="child">The location key used to find the child element.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndHasChild(By child)
    {
        Guard.NotNull(child);
        var c = new HasElementCondition(child.RootSearchPart);
        return AndCondition(c);
    }

    /// <summary>
    /// Adds a condition that check whether the element is related to another element found by a specified location key, to this location key.
    /// </summary>
    /// <param name="relative">The location key used to find the related element.</param>
    /// <param name="distance">The distance to the element (negative if the searched element is preceding the related element).</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndRelativeTo(By relative, int distance)
    {
        Guard.NotNull(relative);
        var c = new RelativeToCondition(distance, relative.ElementSearchPart.Condition);
        return AndCondition(c);
    }

    /// <summary>
    /// Sets the scope of this location key.
    /// </summary>
    /// <param name="scope">The new scope.</param>
    /// <returns>A self-reference to this location key.</returns>
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

    /// <summary>
    /// Adds a condition that finds all descendant elements in the current scope, to this location key.
    /// </summary>
    /// <param name="maxDepth">The maximum depth.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndDescendants(int maxDepth)
    {
        var desc = ElementSearchPart.Parts.OfType<DescendantsSearchPart>().SingleOrDefault();
        if (desc == null)
            ElementSearchPart.Parts.Add(new DescendantsSearchPart(maxDepth));
        else
            desc.MaxDepth = maxDepth;
        return this;
    }

    /// <summary>
    /// Sets the amount of matching elements that are skipped when searching.
    /// </summary>
    /// <param name="value">The number of skipped matching elements.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndSkip(int value)
    {
        ElementSearchPart.SkipCount = value;
        return this;
    }

    /// <summary>
    /// Sets the maximum amount of matching elements that can be found.
    /// </summary>
    /// <param name="value">The number of maximum matching elements.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndTake(int value)
    {
        ElementSearchPart.TakeCount = value;
        return this;
    }

    /// <summary>
    /// Sets the maximum search depth of this location key.
    /// </summary>
    /// <param name="value">The maximum search depth.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndMaxDepth(int value)
    {
        ElementSearchPart.Parts.OfType<IDepthSearchPart>().ForEach(x => x.MaxDepth = value);
        return this;
    }

    /// <summary>
    /// Adds a condition that finds elements that have a specified property value, to this location key.
    /// </summary>
    /// <param name="property">The property to check.</param>
    /// <param name="value">The value to match.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndProperty(AutomationProperty property, object value) => AndProperty(property, value, ByOptions.None);

    /// <summary>
    /// Adds a condition that finds elements that have a specified property value, to this location key.
    /// </summary>
    /// <param name="property">The property to check.</param>
    /// <param name="value">The value to match.</param>
    /// <param name="options">The search options.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndProperty(AutomationProperty property, object value, ByOptions options)
    {
        return AndCondition(new PropertyCondition(property, value, options));
    }

    /// <summary>
    /// Removes all search conditions that check for a specified property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By ClearProperty(AutomationProperty property)
    {
        ElementSearchPart.RemoveCondition<PropertyCondition>(x => x.Property == property);
        return this;
    }

    /// <summary>
    /// Adds a condition that finds elements for which a specified pattern is available, to this location key.
    /// </summary>
    /// <typeparam name="TPattern">The type of the pattern that should be available.</typeparam>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndPatternAvailable<TPattern>()
        where TPattern : BasePattern
        => AndPatternAvailable(typeof(TPattern), true);

    /// <summary>
    /// Adds a condition that finds elements for which a specified pattern is available, to this location key.
    /// </summary>
    /// <param name="patternType">The type of the pattern that should be available.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndPatternAvailable(Type patternType) => AndPatternAvailable(patternType, true);

    /// <summary>
    /// Adds a condition that finds elements for which a specified pattern is not available, to this location key.
    /// </summary>
    /// <typeparam name="TPattern">The type of the pattern that should be available.</typeparam>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndPatternNotAvailable<TPattern>()
        where TPattern : BasePattern
        => AndPatternAvailable(typeof(TPattern), false);

    /// <summary>
    /// Adds a condition that finds elements for which a specified pattern is not available, to this location key.
    /// </summary>
    /// <param name="patternType">The type of the pattern that should be available.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndPatternNotAvailable(Type patternType) => AndPatternAvailable(patternType, false);

    /// <summary>
    /// Adds a condition that finds elements with a specified FrameworkId, to this location key.
    /// </summary>
    /// <param name="value">The framework id (Find known ids in <see cref="FrameworkIds"/>).</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndFramework(string value) => AndProperty(AutomationElement.FrameworkIdProperty, value);

    /// <summary>
    /// Adds a condition that finds elements with a specified FrameworkId, to this location key.
    /// </summary>
    /// <param name="value">The framework id (Find known ids in <see cref="FrameworkIds"/>).</param>
    /// <param name="options">The search options.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndFramework(string value, ByOptions options) => AndProperty(AutomationElement.FrameworkIdProperty, value, options);

    /// <summary>
    /// Adds a condition that finds elements with a specified ClassName, to this location key.
    /// </summary>
    /// <param name="value">The class name.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndClassName(string value) => AndProperty(AutomationElement.ClassNameProperty, value);

    /// <summary>
    /// Adds a condition that finds elements with a specified ClassName, to this location key.
    /// </summary>
    /// <param name="value">The class name.</param>
    /// <param name="options">The search options.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndClassName(string value, ByOptions options) => AndProperty(AutomationElement.ClassNameProperty, value, options);

    /// <summary>
    /// Adds a condition that finds elements with a specified control type, to this location key.
    /// </summary>
    /// <param name="value">The control type.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndControlType(ControlType value) => AndProperty(AutomationElement.ControlTypeProperty, value);

    /// <summary>
    /// Adds a condition that finds elements with a specified name, to this location key.
    /// </summary>
    /// <param name="value">The name.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndName(string value) => AndProperty(AutomationElement.NameProperty, value);

    /// <summary>
    /// Adds a condition that finds elements with a specified name, to this location key.
    /// </summary>
    /// <param name="value">The name.</param>
    /// <param name="options">The search options.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndName(string value, ByOptions options) => AndProperty(AutomationElement.NameProperty, value, options);

    /// <summary>
    /// Adds a condition that finds elements with a specified help text, to this location key.
    /// </summary>
    /// <param name="value">The help text.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndHelpText(string value) => AndProperty(AutomationElement.HelpTextProperty, value);

    /// <summary>
    /// Adds a condition that finds elements with a specified help text, to this location key.
    /// </summary>
    /// <param name="value">The help text.</param>
    /// <param name="options">The search options.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndHelpText(string value, ByOptions options) => AndProperty(AutomationElement.HelpTextProperty, value, options);

    /// <summary>
    /// Adds a condition that finds elements hosted by a process with the specified id, to this location key.
    /// </summary>
    /// <param name="value">The process id.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndProcessId(int value) => AndProperty(AutomationElement.ProcessIdProperty, value);

    /// <summary>
    /// Adds a condition that finds elements with a specified accelerator key, to this location key.
    /// </summary>
    /// <param name="value">The accelerator key.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndAcceleratorKey(string value) => AndProperty(AutomationElement.AcceleratorKeyProperty, value);

    /// <summary>
    /// Adds a condition that finds elements with a specified accelerator key, to this location key.
    /// </summary>
    /// <param name="value">The accelerator key.</param>
    /// <param name="options">The search options.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndAcceleratorKey(string value, ByOptions options) => AndProperty(AutomationElement.AcceleratorKeyProperty, value, options);

    /// <summary>
    /// Adds a condition that finds elements with a specified access key, to this location key.
    /// </summary>
    /// <param name="value">The access key.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndAccessKey(string value) => AndProperty(AutomationElement.AccessKeyProperty, value);

    /// <summary>
    /// Adds a condition that finds elements with a specified access key, to this location key.
    /// </summary>
    /// <param name="value">The access key.</param>
    /// <param name="options">The search options.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndAccessKey(string value, ByOptions options) => AndProperty(AutomationElement.AccessKeyProperty, value, options);

    /// <summary>
    /// Adds a condition that finds elements with a specified automation id, to this location key.
    /// </summary>
    /// <param name="value">The automation id.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndAutomationId(string value) => AndProperty(AutomationElement.AutomationIdProperty, value);

    /// <summary>
    /// Adds a condition that finds elements with a specified automation id, to this location key.
    /// </summary>
    /// <param name="value">The automation id.</param>
    /// <param name="options">The search options.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndAutomationId(string value, ByOptions options) => AndProperty(AutomationElement.AutomationIdProperty, value, options);

    /// <summary>
    /// Adds a condition that finds elements with a specified item status, to this location key.
    /// </summary>
    /// <param name="value">The item status.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndItemStatus(string value) => AndProperty(AutomationElement.ItemStatusProperty, value);

    /// <summary>
    /// Adds a condition that finds elements with a specified item status, to this location key.
    /// </summary>
    /// <param name="value">The item status.</param>
    /// <param name="options">The search options.</param>
    /// <returns>A self-reference to this location key.</returns>
    public virtual By AndItemStatus(string value, ByOptions options) => AndProperty(AutomationElement.ItemStatusProperty, value, options);

    /// <summary>
    /// Adds a condition that finds elements for which a specified pattern is available or not, to this location key.
    /// </summary>
    /// <param name="patternType">The type of the pattern that should be available or not.</param>
    /// <param name="shouldBeAvailable">If set to <c>true</c> the pattern is expected to be available.</param>
    /// <returns>A self-reference to this location key.</returns>
    protected virtual By AndPatternAvailable(Type patternType, bool shouldBeAvailable)
    {
        if (patternType == null)
            throw new ArgumentNullException(nameof(patternType));
        if (!typeof(BasePattern).IsAssignableFrom(patternType))
            throw new ArgumentException($"Only types that are derived from \"{typeof(BasePattern).FullName}\" are supported.", nameof(patternType));
        var c = new PropertyCondition(PatternUtility.GetIsPatternAvailableProperty(patternType), shouldBeAvailable, ByOptions.None);
        return AndCondition(c);
    }

    private By AndCondition(ISearchCondition condition)
    {
        ElementSearchPart.AppendCondition(condition, SearchConditionOperator.And);
        return this;
    }
}
