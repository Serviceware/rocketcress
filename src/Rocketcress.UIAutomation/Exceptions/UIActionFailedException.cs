using Rocketcress.UIAutomation.Controls;

namespace Rocketcress.UIAutomation.Exceptions;

public class UIActionFailedException : UIAutomationControlException
{
    public UIActionFailedException()
    {
    }

    public UIActionFailedException(string message)
        : base(message)
    {
    }

    public UIActionFailedException(string message, IUITestControl element)
        : base(message, element)
    {
    }

    public UIActionFailedException(string message, IUITestControl parent, By locationKey)
        : base(message, parent, locationKey)
    {
    }

    public UIActionFailedException(string message, IUITestControl element, Exception innerException)
        : base(message, element, innerException)
    {
    }

    public UIActionFailedException(string message, IUITestControl parent, By locationKey, Exception innerException)
        : base(message, parent, locationKey, innerException)
    {
    }

    public UIActionFailedException(string message, Application app, AutomationElement parent, By locationKey)
        : base(message, app, parent, locationKey)
    {
    }

    public UIActionFailedException(string message, Application app, AutomationElement parent, By locationKey, Exception innerException)
        : base(message, app, parent, locationKey, innerException)
    {
    }
}
