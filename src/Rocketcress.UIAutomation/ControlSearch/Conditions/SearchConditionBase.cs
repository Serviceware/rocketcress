namespace Rocketcress.UIAutomation.ControlSearch.Conditions;

/// <summary>
/// Represents a search condition for finding UIAutomation elements.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.ISearchCondition" />
public abstract class SearchConditionBase : ISearchCondition
{
    /// <inheritdoc />
    public abstract bool Check(AutomationElement element, TreeWalker treeWalker);

    /// <inheritdoc />
    public virtual object Clone()
    {
        var clone = CloneInternal();
        return clone;
    }

    /// <inheritdoc />
    public abstract string GetDescription();

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>A new object that is a copy of this instance.</returns>
    protected abstract SearchConditionBase CloneInternal();
}
