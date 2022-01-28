using System.Threading.Tasks;

namespace Rocketcress.Core;

/// <summary>
/// Configuration for what should happen when an exception occurs during an asynchronous wait operation.
/// </summary>
/// <typeparam name="T">The type of result of the wait operation.</typeparam>
public interface IAsyncWaitOnError<T>
{
    /// <summary>
    /// Configures the wait operation to call the specified action when an exception occurs.
    /// If the action throws an <see cref="WaitAbortedException"/> the operation is aborted.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>The configured wait operation.</returns>
    IAsyncWait<T> Call(Func<Exception, Task> action);

    /// <summary>
    /// Configures the wait operation to call the specified result factory when an exception occurs.
    /// If the result factory returns the default value of <typeparamref name="T"/> the wait operation continues.
    /// If the action throws an <see cref="WaitAbortedException"/> the operation is aborted.
    /// </summary>
    /// <param name="resultFactory">The result factory to execute.</param>
    /// <returns>The configured wait operation.</returns>
    IAsyncWait<T> Return(Func<Exception, Task<T?>> resultFactory);

    /// <summary>
    /// Configures the wait operation to return the specified value when an exception occurs.
    /// If the result is the default value of <typeparamref name="T"/> the wait operation continues.
    /// </summary>
    /// <param name="value">The value to return.</param>
    /// <returns>The configured wait operation.</returns>
    IAsyncWait<T> Return(T? value);

    /// <summary>
    /// Configures the wait operation to abort when an exception occurs.
    /// If the wait operation is configured to throw on failure, this will also lead to an exception.
    /// </summary>
    /// <returns>The configured wait operation.</returns>
    IAsyncWait<T> Abort();
}
