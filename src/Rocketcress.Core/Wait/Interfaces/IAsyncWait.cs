using System.Threading.Tasks;

namespace Rocketcress.Core;

/// <summary>
/// Asynchronous wait operation that can be configured or started.
/// </summary>
/// <typeparam name="T">The type of result of the wait operation.</typeparam>
public interface IAsyncWait<T> : IAsyncStartableWait<T>, IConfigurableWait<T, IAsyncWait<T>>
{
    /// <summary>
    /// Configures what should happen when an exception occurs during this wait operation.
    /// </summary>
    /// <returns>Configuration object with which the exception handling can be configured.</returns>
    IAsyncWaitOnError<T> OnError();

    /// <summary>
    /// Adds an action to be executed before the wait operation starts.
    /// </summary>
    /// <param name="action">The action to execute before the wait operation starts.</param>
    /// <returns>The configured wait operation.</returns>
    IAsyncWait<T> PrecedeWith(Func<WaitContext<T>, Task> action);

    /// <summary>
    /// Adds an action to be executed after the wait operation starts.
    /// </summary>
    /// <param name="action">The action to execute before the wait operation starts.</param>
    /// <returns>The configured wait operation.</returns>
    IAsyncWait<T> ContinueWith(Func<WaitContext<T>, Task> action);
}
