namespace Rocketcress.Core;

/// <summary>
/// Represents a method that handles the WaitingStarting and WaitingEnded events of the Waiter.
/// </summary>
/// <param name="sender">The sender.</param>
/// <param name="e">The event arguments.</param>
public delegate void WaitingEventHandler(object? sender, WaitingEventArgs e);

/// <summary>
/// Represents event arguments for the WaitingStarting and WaitingEnded events of the Waiter.
/// </summary>
public class WaitingEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WaitingEventArgs"/> class.
    /// </summary>
    /// <param name="waitContext">The context of the wait operation.</param>
    public WaitingEventArgs(WaitContext waitContext)
    {
        WaitContext = waitContext;
    }

    /// <summary>
    /// Gets a data store you can use on another event on this waiting operation.
    /// </summary>
    [Obsolete("Use WaitContext.Data instead.")]
    public IDictionary<string, object> DataStore => WaitContext.Data;

    /// <summary>
    /// Gets the wait context.
    /// </summary>
    public WaitContext WaitContext { get; }
}

/// <summary>
/// Represents event arguments for the WaitingStarting and WaitingEnded events of the Waiter.
/// </summary>
/// <typeparam name="T">The type of value the wait operation returns.</typeparam>
public class WaitingEventArgs<T> : WaitingEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WaitingEventArgs{T}"/> class.
    /// </summary>
    /// <param name="waitContext">The context of the wait operation.</param>
    public WaitingEventArgs(WaitContext<T> waitContext)
        : base(waitContext)
    {
        WaitContext = waitContext;
    }

    /// <summary>
    /// Gets the wait context.
    /// </summary>
    public new WaitContext<T> WaitContext { get; }
}
