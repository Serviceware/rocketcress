using System;

namespace Rocketcress.UIAutomation.Exceptions
{
    public class UIAutomationException : Exception
    {
        public UIAutomationException() { }
        public UIAutomationException(string message) : base(message) { }
        public UIAutomationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
