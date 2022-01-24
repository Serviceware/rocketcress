namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a UIAutomation control that contains multiple items.
/// </summary>
/// <seealso cref="IUITestControl"/>
public interface IUITestContainerControl : IUITestControl
{
    /// <summary>
    /// Gets the available items in this <see cref="IUITestContainerControl"/>.
    /// </summary>
    IEnumerable<IUITestControl> Items { get; }
}

/// <summary>
/// Represents a UIAutomation control that contains multiple items.
/// </summary>
/// <typeparam name="TItem">The type of list items.</typeparam>
/// <seealso cref="IUITestContainerControl"/>
public interface IUITestContainerControl<TItem> : IUITestContainerControl
    where TItem : IUITestControl
{
    /// <summary>
    /// Gets the available items in this <see cref="IUITestContainerControl{TItem}"/>.
    /// </summary>
    new IEnumerable<TItem> Items { get; }
}