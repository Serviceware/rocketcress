namespace Rocketcress.UIAutomation.Controls.ControlSupport;

/// <summary>
/// Contains supporting code for menu controls.
/// </summary>
public class MenuControlSupport
{
    private readonly UITestControl _control;
    private readonly By _itemLocationKey;
    private readonly ListControlSupport _listControlSupport;
    private readonly UITestControl _itemContainer;
    private readonly ControlType _menuControlType;
    private readonly ControlType _itemControlType;

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuControlSupport"/> class.
    /// </summary>
    /// <param name="control">The control to use.</param>
    /// <param name="itemLocationKey">The location key that is used to find menu items.</param>
    /// <param name="menuControlType">Type of the menu control.</param>
    /// <param name="itemControlType">Type of the item control.</param>
    public MenuControlSupport(UITestControl control, By itemLocationKey, ControlType menuControlType, ControlType itemControlType)
    {
        _control = Guard.NotNull(control);
        _itemLocationKey = Guard.NotNull(itemLocationKey);
        _menuControlType = Guard.NotNull(menuControlType);
        _itemControlType = Guard.NotNull(itemControlType);

        var byContainer = By.Scope(TreeScope.Subtree)
                        .AndCondition(ItemContainerCondition, "MenuItemContainer")
                        .AndHasChild(itemLocationKey);
        _itemContainer = new UITestControl(control.Application, byContainer, control);
        _listControlSupport = new ListControlSupport(_itemContainer, itemLocationKey);
    }

    /// <summary>
    /// Gets the item container.
    /// </summary>
    public UITestControl ItemContainer => _itemContainer;

    /// <summary>
    /// Gets the location key that is used to find menu items.
    /// </summary>
    public By ItemLocationKey => _itemLocationKey;

    /// <summary>
    /// Enumerates the items in the menu.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="AutomationElement"/>s that represents the items in the menu.</returns>
    public IEnumerable<AutomationElement> EnumerateItems() => _itemContainer.Exists ? _listControlSupport.EnumerateItems() : Array.Empty<AutomationElement>();

    private bool ItemContainerCondition(AutomationElement element, TreeWalker walker)
    {
        return element == _control.AutomationElement ||
               (element.Current.ControlType != _menuControlType && element.Current.ControlType != _itemControlType);
    }
}
