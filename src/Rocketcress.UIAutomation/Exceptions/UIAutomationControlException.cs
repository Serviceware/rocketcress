using Rocketcress.UIAutomation.Controls;
using Rocketcress.UIAutomation.Models;

namespace Rocketcress.UIAutomation.Exceptions;

public class UIAutomationControlException : UIAutomationException
{
    public UITestControlDescriptor ControlDescriptor { get; set; }

    public UIAutomationControlException()
        : base()
    {
    }

    public UIAutomationControlException(string message)
        : base(message)
    {
    }

    public UIAutomationControlException(string message, IUITestControl parent, By locationKey)
        : this(message, parent, locationKey, null)
    {
    }

    public UIAutomationControlException(string message, IUITestControl parent, By locationKey, Exception innerException)
        : this(message, new UITestControl(parent.Application, locationKey, parent), innerException)
    {
    }

    public UIAutomationControlException(string message, Application app, AutomationElement parent, By locationKey)
        : this(message, app, parent, locationKey, null)
    {
    }

    public UIAutomationControlException(string message, Application app, AutomationElement parent, By locationKey, Exception innerException)
        : this(message, new UITestControl(app, locationKey, parent), innerException)
    {
    }

    public UIAutomationControlException(string message, IUITestControl element)
        : this(message, element, (Exception)null)
    {
    }

    public UIAutomationControlException(string message, IUITestControl element, Exception innerException)
        : base(message.TrimEnd('.') + $": {element.GetSearchDescription(true)}", innerException)
    {
        ControlDescriptor = new UITestControlDescriptor(element);
    }
}
