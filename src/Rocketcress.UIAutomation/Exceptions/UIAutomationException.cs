namespace Rocketcress.UIAutomation.Exceptions;

/// <summary>
/// Represents errors that occur during some action in UIAutomation actions.
/// </summary>
/// <seealso cref="System.Exception" />
public class UIAutomationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UIAutomationException"/> class.
    /// </summary>
    public UIAutomationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIAutomationException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public UIAutomationException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIAutomationException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
    public UIAutomationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
