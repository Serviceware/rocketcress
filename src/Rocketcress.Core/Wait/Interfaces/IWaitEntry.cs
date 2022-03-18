using System.Threading.Tasks;

namespace Rocketcress.Core;

/// <summary>
/// Represents a wait operation entry point.
/// </summary>
public interface IWaitEntry
{
    /// <summary>
    /// Raised when a wait operation is starting.
    /// </summary>
    event WaitingEventHandler? WhenStarting;

    /// <summary>
    /// Raised when a wait operation finished.
    /// </summary>
    event WaitingEventHandler? WhenFinished;

    /// <summary>
    /// Raised when an exception has been thrown during a wait operation.
    /// </summary>
    event ExceptionEventHandler? WhenExceptionOccurred;

    /// <summary>
    /// Gets the default options for wait operations.
    /// </summary>
    IWaitOptions DefaultOptions { get; }

    /// <summary>
    /// Gets the global options for wait operations.
    /// </summary>
    [Obsolete("Use DefaultOptions property instead.")]
    IObsoleteWaitOptions Options { get; }

    /// <summary>
    /// Creates a <see cref="IWait{T}"/> object with a specified condition.
    /// </summary>
    /// <typeparam name="T">The type of the condition result.</typeparam>
    /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of <typeparamref name="T"/>.</param>
    /// <returns>An instance of <see cref="IWait{T}"/> that can be used to wait or configure additional options.</returns>
    IWait<T> Until<T>(Func<T?> condition);

    /// <summary>
    /// Creates a <see cref="IWait{T}"/> object with a specified condition.
    /// </summary>
    /// <typeparam name="T">The type of the condition result.</typeparam>
    /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of <typeparamref name="T"/>.</param>
    /// <returns>An instance of <see cref="IWait{T}"/> that can be used to wait or configure additional options.</returns>
    IWait<T> Until<T>(Func<WaitContext<T>, T?> condition);

    /// <summary>
    /// Creates an <see cref="IAsyncWait{T}"/> object with a specified condition.
    /// </summary>
    /// <typeparam name="T">The type of the condition result.</typeparam>
    /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of <typeparamref name="T"/>.</param>
    /// <returns>An instance of <see cref="IAsyncWait{T}"/> that can be used to wait or configure additional options.</returns>
    IAsyncWait<T> Until<T>(Func<Task<T?>> condition);

    /// <summary>
    /// Creates an <see cref="IAsyncWait{T}"/> object with a specified condition.
    /// </summary>
    /// <typeparam name="T">The type of the condition result.</typeparam>
    /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of <typeparamref name="T"/>.</param>
    /// <returns>An instance of <see cref="IAsyncWait{T}"/> that can be used to wait or configure additional options.</returns>
    IAsyncWait<T> Until<T>(Func<WaitContext<T>, Task<T?>> condition);
}
