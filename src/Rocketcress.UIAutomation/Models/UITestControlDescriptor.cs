using Rocketcress.UIAutomation.Controls;

namespace Rocketcress.UIAutomation.Models;

public class UITestControlDescriptor
{
    public UITestControlDescriptor Parent { get; set; }
    public By LocationKey { get; set; }
    public AutomationElement AutomationElement { get; set; }

    public UITestControlDescriptor(IUITestControl control)
    {
        Parent = control.SearchContext == null ? null : new UITestControlDescriptor(control.SearchContext);
        LocationKey = control.LocationKey;
        AutomationElement = (control as UITestControl)?.GetCachedAutomationElement();
    }
}
