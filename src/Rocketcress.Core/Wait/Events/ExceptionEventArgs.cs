namespace Rocketcress.Core;

/// <summary>
/// Represents a method that handles the ExceptionOccured event of the Waiter.
/// </summary>
/// <param name="sender">The sender.</param>
/// <param name="e">The event arguments.</param>
public delegate void ExceptionEventHandler(object? sender, ExceptionEventArgs e);

/// <summary>
/// Represents event arguments for the ExceptionOccured event of the Waiter.
/// </summary>
public class ExceptionEventArgs : EventArgs
{
    /// <summary>
    /// Gets the exception object that has been thrown during the waiting operation.
    /// </summary>
    public Exception ExceptionObject { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionEventArgs"/> class.
    /// </summary>
    /// <param name="exception">The exception object that has been thrown during the waiting operation.</param>
    public ExceptionEventArgs(Exception exception)
    {
        ExceptionObject = exception;
    }
}
