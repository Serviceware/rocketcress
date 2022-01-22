namespace Rocketcress.Core;

/// <summary>
/// Used to indicate failure for a wait or retry operation.
/// </summary>
public class WaitFailedException : Exception
{
    /// <summary>
    /// Gets the result of the wait operation.
    /// </summary>
    public WaitResult WaitResult { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WaitFailedException"/> class.
    /// </summary>
    /// <param name="waitResult">The result of the wait operation.</param>
    public WaitFailedException(WaitResult waitResult)
    {
        WaitResult = waitResult;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WaitFailedException"/> class.
    /// </summary>
    /// <param name="waitResult">The result of the wait operation.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public WaitFailedException(WaitResult waitResult, string? message)
        : base(message)
    {
        WaitResult = waitResult;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WaitFailedException"/> class.
    /// </summary>
    /// <param name="waitResult">The result of the wait operation.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
    public WaitFailedException(WaitResult waitResult, string? message, Exception? innerException)
        : base(message, innerException)
    {
        WaitResult = waitResult;
    }
}
