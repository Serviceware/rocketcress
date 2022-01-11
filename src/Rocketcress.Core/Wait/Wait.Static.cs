using System.Threading.Tasks;

namespace Rocketcress.Core
{
    /// <summary>
    /// Provides functionalities to create wait operations.
    /// </summary>
    public static class Wait
    {
        /// <summary>
        /// Raised when a wait operation is starting.
        /// </summary>
        public static event WaitingEventHandler? WhenStarting;

        /// <summary>
        /// Raised when a wait operation finished.
        /// </summary>
        public static event WaitingEventHandler? WhenFinished;

        /// <summary>
        /// Raised when an exception has been thrown during a wait operation.
        /// </summary>
        public static event ExceptionEventHandler? WhenExceptionOccurred;

        /// <summary>
        /// Gets the global options for wait operations.
        /// </summary>
        public static WaitOptions Options { get; } = new WaitOptions();

        /// <summary>
        /// Creates a <see cref="IWait{T}"/> object with a specified condition.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of <typeparamref name="T"/>.</param>
        /// <returns>An instance of <see cref="IWait{T}"/> that can be used to wait or configure additional options.</returns>
        public static IWait<T> Until<T>(Func<T?> condition)
        {
            return new Wait<T>(condition);
        }

        /// <summary>
        /// Creates an <see cref="IAsyncWait{T}"/> object with a specified condition.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of <typeparamref name="T"/>.</param>
        /// <returns>An instance of <see cref="IAsyncWait{T}"/> that can be used to wait or configure additional options.</returns>
        public static IAsyncWait<T> Until<T>(Func<Task<T?>> condition)
        {
            return new AsyncWait<T>(condition);
        }

        internal static void OnStarting(object? sender, IDictionary<string, object> data)
        {
            WhenStarting?.Invoke(sender, new WaitingEventArgs(data));
        }

        internal static void OnFinished(object? sender, IDictionary<string, object> data)
        {
            WhenFinished?.Invoke(sender, new WaitingEventArgs(data));
        }

        internal static void OnExceptionOccurred(object? sender, Exception exception)
        {
            WhenExceptionOccurred?.Invoke(sender, new ExceptionEventArgs(exception));
        }
    }
}
