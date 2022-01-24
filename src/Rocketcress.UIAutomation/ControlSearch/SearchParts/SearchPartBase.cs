using Rocketcress.Core.Extensions;
using PropertyCondition = Rocketcress.UIAutomation.ControlSearch.Conditions.PropertyCondition;

namespace Rocketcress.UIAutomation.ControlSearch.SearchParts;

/// <summary>
/// Represents a search part for finding UIAutomation elements.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.ISearchPart" />
public abstract class SearchPartBase : ISearchPart
{
    /// <inheritdoc/>
    public virtual ISearchCondition Condition { get; set; }

    /// <summary>
    /// Gets or sets the skip count.
    /// </summary>
    public virtual int? SkipCount { get; set; }

    /// <summary>
    /// Gets or sets the take count.
    /// </summary>
    public virtual int? TakeCount { get; set; }

    /// <inheritdoc/>
    public IEnumerable<AutomationElement> FindElements(AutomationElement element, TreeWalker treeWalker)
    {
        var result = FindElementsInternal(element, treeWalker);
        if (SkipCount.HasValue && SkipCount > 0)
            result = result.Skip(SkipCount.Value);
        if (TakeCount.HasValue && TakeCount >= 0)
            result = result.Take(TakeCount.Value);
        return result;
    }

    /// <inheritdoc/>
    public object Clone()
    {
        var clone = CloneInternal();
        try
        {
            clone.Condition = (ISearchCondition)Condition?.Clone();
        }
        catch
        {
        }

        try
        {
            clone.SkipCount = SkipCount;
        }
        catch
        {
        }

        try
        {
            clone.TakeCount = TakeCount;
        }
        catch
        {
        }

        return clone;
    }

    /// <inheritdoc/>
    public abstract string GetDescription();

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>A new object that is a copy of this instance.</returns>
    protected abstract SearchPartBase CloneInternal();

    /// <summary>
    /// Finds the elements matching the <see cref="Condition"/>.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="treeWalker">The tree walker.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that iterates through all elements matching the <see cref="Condition"/>.</returns>
    protected abstract IEnumerable<AutomationElement> FindElementsInternal(AutomationElement element, TreeWalker treeWalker);

    /// <summary>
    /// Gets the skip take description.
    /// </summary>
    /// <returns>The description.</returns>
    protected string GetSkipTakeDescription()
    {
        if (SkipCount.HasValue || TakeCount.HasValue)
            return $"[{(SkipCount ?? 0) + 1}{(TakeCount.HasValue ? $":{TakeCount}" : string.Empty)}]";
        return null;
    }

    /// <summary>
    /// Gets the condition description.
    /// </summary>
    /// <returns>The description.</returns>
    protected string GetConditionDescription()
    {
        if (Condition == null)
            return string.Empty;

        var prefix = string.Empty;
        var condition = Condition;
        if (Condition is ICompositeSearchCondition compositeCondition &&
            compositeCondition.Conditions.OfType<PropertyCondition>().TryFirst(x => x.Property == AutomationElement.ControlTypeProperty, out var tempCtp))
        {
            var clone = (ICompositeSearchCondition)Condition.Clone();
            clone.Conditions.Remove(tempCtp);

            prefix = ((ControlType)tempCtp.Value).ProgrammaticName.Split('.').Last();
            condition = clone.Conditions.Count == 0 ? null : clone;
        }
        else if (Condition is PropertyCondition tempPc && tempPc.Property == AutomationElement.ControlTypeProperty)
        {
            prefix = ((ControlType)tempPc.Value).ProgrammaticName.Split('.').Last();
            condition = null;
        }

        string conditionStr = null;
        if (condition != null)
        {
            conditionStr = condition.GetDescription();
            if (conditionStr.StartsWith("(") && conditionStr.EndsWith(")"))
                conditionStr = conditionStr.Substring(1, conditionStr.Length - 2);
            conditionStr = $"[{conditionStr}]";
        }

        return $"{prefix}{conditionStr}";
    }
}
