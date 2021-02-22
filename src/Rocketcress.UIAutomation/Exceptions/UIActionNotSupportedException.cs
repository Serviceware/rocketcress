using System;
using System.Windows.Automation;
using Rocketcress.UIAutomation.Controls;

namespace Rocketcress.UIAutomation.Exceptions
{
    public class UIActionNotSupportedException : UIAutomationControlException
    {
        public UIActionNotSupportedException() { }
        public UIActionNotSupportedException(string message) : base(message) { }
        public UIActionNotSupportedException(string message, UITestControl element) : base(message, element) { }
        public UIActionNotSupportedException(string message, UITestControl parent, By locationKey) : base(message, parent, locationKey) { }
        public UIActionNotSupportedException(string message, AutomationElement parent, By locationKey) : base(message, parent, locationKey) { }
        public UIActionNotSupportedException(string message, UITestControl element, Exception innerException) : base(message, element, innerException) { }
        public UIActionNotSupportedException(string message, UITestControl parent, By locationKey, Exception innerException) : base(message, parent, locationKey, innerException) { }
        public UIActionNotSupportedException(string message, AutomationElement parent, By locationKey, Exception innerException) : base(message, parent, locationKey, innerException) { }
    }
}
