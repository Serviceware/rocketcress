using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rocketcress.Core
{
    /// <summary>
    /// Provides functionalities to create wait operations.
    /// </summary>
    public static class Wait
    {
        private const string SyncWaitName = "wait operation";
        private const string AsyncWaitName = "async wait operation";

        private static readonly IObsoleteWaitOptions _options = new WaitOptions
        {
            TraceExceptions = true,
            MaxAcceptedExceptions = null,
            MaxRetryCount = null,
            Timeout = TimeSpan.FromSeconds(10),
            TimeGap = TimeSpan.FromMilliseconds(100),
        };

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
        /// Gets the default options for wait operations.
        /// </summary>
        public static IWaitOptions DefaultOptions => _options;

        /// <summary>
        /// Gets the global options for wait operations.
        /// </summary>
        [Obsolete("Use DefaultOptions property instead.")]
        public static IObsoleteWaitOptions Options => _options;

        /// <summary>
        /// Creates a <see cref="IWait{T}"/> object with a specified condition.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of <typeparamref name="T"/>.</param>
        /// <returns>An instance of <see cref="IWait{T}"/> that can be used to wait or configure additional options.</returns>
        public static IWait<T> Until<T>(Func<T?> condition)
        {
            return CreateWait((WaitContext<T> ctx) => condition());
        }

        /// <summary>
        /// Creates a <see cref="IWait{T}"/> object with a specified condition.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of <typeparamref name="T"/>.</param>
        /// <returns>An instance of <see cref="IWait{T}"/> that can be used to wait or configure additional options.</returns>
        public static IWait<T> Until<T>(Func<WaitContext<T>, T?> condition)
        {
            return CreateWait(condition);
        }

        /// <summary>
        /// Creates an <see cref="IAsyncWait{T}"/> object with a specified condition.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of <typeparamref name="T"/>.</param>
        /// <returns>An instance of <see cref="IAsyncWait{T}"/> that can be used to wait or configure additional options.</returns>
        public static IAsyncWait<T> Until<T>(Func<Task<T?>> condition)
        {
            return CreateAsyncWait(async (WaitContext<T> ctx) => await condition());
        }

        /// <summary>
        /// Creates an <see cref="IAsyncWait{T}"/> object with a specified condition.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of <typeparamref name="T"/>.</param>
        /// <returns>An instance of <see cref="IAsyncWait{T}"/> that can be used to wait or configure additional options.</returns>
        public static IAsyncWait<T> Until<T>(Func<WaitContext<T>, Task<T?>> condition)
        {
            return CreateAsyncWait(condition);
        }

        internal static void OnStarting(object? sender, WaitContext ctx)
        {
            WhenStarting?.Invoke(sender, new WaitingEventArgs(ctx));
        }

        internal static void OnFinished(object? sender, WaitContext ctx)
        {
            WhenFinished?.Invoke(sender, new WaitingEventArgs(ctx));
        }

        internal static void OnExceptionOccurred(object? sender, WaitContext ctx, Exception exception)
        {
            WhenExceptionOccurred?.Invoke(sender, new ExceptionEventArgs(ctx, exception));
        }

        private static IWait<T> CreateWait<T>(Func<WaitContext<T>, T?> condition)
        {
            return new Wait<T>(
                condition,
                DefaultOptions,
                SyncWaitName,
                OnStarting,
                OnFinished,
                OnExceptionOccurred);
        }

        private static IAsyncWait<T> CreateAsyncWait<T>(Func<WaitContext<T>, Task<T?>> condition)
        {
            return new AsyncWait<T>(
                condition,
                DefaultOptions,
                AsyncWaitName,
                OnStarting,
                OnFinished,
                OnExceptionOccurred);
        }
    }
}
