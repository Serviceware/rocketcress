namespace Rocketcress.UIAutomation.ControlSearch;

/// <summary>
/// Represents a search condition for finding UIAutomation elements.
/// </summary>
/// <seealso cref="System.ICloneable" />
public interface ISearchCondition : ICloneable
{
    /// <summary>
    /// Checks if the specified element matches this <see cref="ISearchCondition"/>.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="treeWalker">The tree walker.</param>
    /// <returns><c>true</c> if <paramref name="element"/> matches this <see cref="ISearchCondition"/>.</returns>
    bool Check(AutomationElement element, TreeWalker treeWalker);

    /// <summary>
    /// Gets the description of this <see cref="ISearchCondition"/>.
    /// </summary>
    /// <returns>The description of this <see cref="ISearchCondition"/>.</returns>
    string GetDescription();
}

/// <summary>
/// Represents a search condition for finding UIAutomation elements composed of multiple <see cref="ISearchCondition"/> instances.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.ISearchCondition" />
public interface ICompositeSearchCondition : ISearchCondition
{
    /// <summary>
    /// Gets the type of the operator.
    /// </summary>
    SearchConditionOperator OperatorType { get; }

    /// <summary>
    /// Gets or sets the conditions.
    /// </summary>
    IList<ISearchCondition> Conditions { get; set; }
}

/// <summary>
/// The operator used in <see cref="ICompositeSearchCondition"/>.
/// </summary>
public enum SearchConditionOperator
{
    /// <summary>
    /// All conditions need to be met to match a control.
    /// </summary>
    And,

    /// <summary>
    /// At least one condition needs to be met to match a control.
    /// </summary>
    Or,
}
