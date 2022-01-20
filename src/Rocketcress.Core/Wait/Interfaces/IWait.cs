namespace Rocketcress.Core;

/// <summary>
/// Wait operation that can be configured or started.
/// </summary>
/// <typeparam name="T">The type of result of the wait operation.</typeparam>
public interface IWait<T> : IStartableWait<T>, IConfigurableWait<T, IWait<T>>
{
    /// <summary>
    /// Configures what should happen when an exception occurs during this wait operation.
    /// </summary>
    /// <returns>Configuration object with which the exception handling can be configured.</returns>
    IWaitOnError<T> OnError();

    /// <summary>
    /// Adds an action to be executed before the wait operation starts.
    /// </summary>
    /// <param name="action">The action to execute before the wait operation starts.</param>
    /// <returns>The configured wait operation.</returns>
    IWait<T> PrecedeWith(Action<WaitContext<T>> action);

    /// <summary>
    /// Adds an action to be executed after the wait operation starts.
    /// </summary>
    /// <param name="action">The action to execute before the wait operation starts.</param>
    /// <returns>The configured wait operation.</returns>
    IWait<T> ContinueWith(Action<WaitContext<T>> action);
}
