using Rocketcress.UIAutomation.Controls;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Exceptions
{
    public class NoSuchElementException : UIAutomationControlException
    {
        private const string DefaultMessage = "No Element was found with the given search properties";

        public NoSuchElementException(string message) : base(message) { }
        public NoSuchElementException(string message, UITestControl element) : base(message, element) { }
        public NoSuchElementException(string message, UITestControl parent, By locationKey) : base(message, parent, locationKey) { }
        public NoSuchElementException(string message, AutomationElement parent, By locationKey) : base(message, parent, locationKey) { }

        public NoSuchElementException(UITestControl element) : base(DefaultMessage, element) { }
        public NoSuchElementException(UITestControl parent, By locationKey) : base(DefaultMessage, parent, locationKey) { }
        public NoSuchElementException(AutomationElement parent, By locationKey) : base(DefaultMessage, parent, locationKey) { }
    }
}
