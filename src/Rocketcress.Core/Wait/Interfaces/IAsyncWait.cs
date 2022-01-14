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
}
