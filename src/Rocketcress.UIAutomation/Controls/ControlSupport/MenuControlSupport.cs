namespace Rocketcress.UIAutomation.Controls.ControlSupport;

public class MenuControlSupport
{
    private readonly UITestControl _control;
    private readonly By _itemLocationKey;
    private readonly ListControlSupport _listControlSupport;
    private readonly UITestControl _itemContainer;
    private readonly ControlType _menuControlType;
    private readonly ControlType _itemControlType;

    public UITestControl ItemContainer => _itemContainer;
    public By ItemLocationKey => _itemLocationKey;

    public MenuControlSupport(UITestControl control, By itemLocationKey, ControlType menuControlType, ControlType itemControlType)
    {
        _control = control;
        _itemLocationKey = itemLocationKey;
        _menuControlType = menuControlType;
        _itemControlType = itemControlType;

        var byContainer = By.Scope(TreeScope.Subtree)
                        .AndCondition(ItemContainerCondition, "MenuItemContainer")
                        .AndHasChild(itemLocationKey);
        _itemContainer = new UITestControl(control.Application, byContainer, control);
        _listControlSupport = new ListControlSupport(_itemContainer, itemLocationKey);
    }

    public IEnumerable<AutomationElement> EnumerateItems() => _itemContainer.Exists ? _listControlSupport.EnumerateItems() : Array.Empty<AutomationElement>();

    private bool ItemContainerCondition(AutomationElement element, TreeWalker walker)
    {
        return element == _control.AutomationElement ||
               (element.Current.ControlType != _menuControlType && element.Current.ControlType != _itemControlType);
    }
}
