using Rocketcress.UIAutomation.Controls;
using System;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Exceptions
{
    public class UIAutomationControlException : UIAutomationException
    {
        public UITestControlDescriptor ControlDescriptor { get; set; }

        public UIAutomationControlException() : base() { }
        public UIAutomationControlException(string message) : base(message) { }

        public UIAutomationControlException(string message, IUITestControl parent, By locationKey) : this(message, parent, locationKey, null) { }
        public UIAutomationControlException(string message, IUITestControl parent, By locationKey, Exception innerException) 
            : this(message, new UITestControl(locationKey, parent), innerException) { }
        public UIAutomationControlException(string message, AutomationElement parent, By locationKey) : this(message, parent, locationKey, null) { }
        public UIAutomationControlException(string message, AutomationElement parent, By locationKey, Exception innerException) 
            : this(message, new UITestControl(locationKey, parent), innerException) { }
        
        public UIAutomationControlException(string message, IUITestControl element) : this(message, element, (Exception)null) { }
        public UIAutomationControlException(string message, IUITestControl element, Exception innerException)
            : base(message.TrimEnd('.') + $": {element.GetSearchDescription(true)}", innerException)
        {
            ControlDescriptor = new UITestControlDescriptor(element);
        }
    }

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
}
