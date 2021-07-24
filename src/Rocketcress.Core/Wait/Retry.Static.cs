using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rocketcress.Core
{
    /// <summary>
    /// Provides functionalities to create retry operations.
    /// </summary>
    public static class Retry
    {
        private const string SyncRetryName = "retry operation";
        private const string AsyncRetryName = "async retry operation";

        private static readonly IObsoleteWaitOptions _options = new WaitOptions
        {
            TraceExceptions = true,
            MaxAcceptedExceptions = null,
            MaxRetryCount = 5,
            Timeout = TimeSpan.MaxValue,
            TimeGap = TimeSpan.Zero,
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
        /// Creates a <see cref="IWait{T}"/> object with a specified condition.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The retry operation ends when the returned value equals the default value of <typeparamref name="T"/>.</param>
        /// <returns>An instance of <see cref="IWait{T}"/> that can be used to retry or configure additional options.</returns>
        public static IWait<T> Until<T>(Func<T?> condition)
        {
            return CreateWait((WaitContext<T> ctx) => condition());
        }

        /// <summary>
        /// Creates a <see cref="IWait{T}"/> object with a specified condition.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The retry operation ends when the returned value equals the default value of <typeparamref name="T"/>.</param>
        /// <returns>An instance of <see cref="IWait{T}"/> that can be used to retry or configure additional options.</returns>
        public static IWait<T> Until<T>(Func<WaitContext<T>, T?> condition)
        {
            return CreateWait(condition);
        }

        /// <summary>
        /// Creates a <see cref="IWait{Boolean}"/> object with a specified condition.
        /// </summary>
        /// <param name="action">The action to retry.</param>
        /// <returns>An instance of <see cref="IWait{T}"/> that can be used to retry or configure additional options.</returns>
        public static IWait<bool> Action(Action action)
        {
            return CreateWait((WaitContext<bool> ctx) =>
            {
                action();
                return true;
            });
        }

        /// <summary>
        /// Creates a <see cref="IWait{Boolean}"/> object with a specified condition.
        /// </summary>
        /// <param name="action">The action to retry.</param>
        /// <returns>An instance of <see cref="IWait{T}"/> that can be used to retry or configure additional options.</returns>
        public static IWait<bool> Action(Action<WaitContext<bool>> action)
        {
            return CreateWait((WaitContext<bool> ctx) =>
            {
                action(ctx);
                return true;
            });
        }

        /// <summary>
        /// Creates a <see cref="IAsyncWait{T}"/> object with a specified condition.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The retry operation ends when the returned value equals the default value of <typeparamref name="T"/>.</param>
        /// <returns>An instance of <see cref="IWait{T}"/> that can be used to retry or configure additional options.</returns>
        public static IAsyncWait<T> Until<T>(Func<Task<T?>> condition)
        {
            return CreateAsyncWait(async (WaitContext<T> ctx) => await condition());
        }

        /// <summary>
        /// Creates a <see cref="IAsyncWait{T}"/> object with a specified condition.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The retry operation ends when the returned value equals the default value of <typeparamref name="T"/>.</param>
        /// <returns>An instance of <see cref="IWait{T}"/> that can be used to retry or configure additional options.</returns>
        public static IAsyncWait<T> Until<T>(Func<WaitContext<T>, Task<T?>> condition)
        {
            return CreateAsyncWait(condition);
        }

        /// <summary>
        /// Creates a <see cref="IAsyncWait{Boolean}"/> object with a specified condition.
        /// </summary>
        /// <param name="action">The action to retry.</param>
        /// <returns>An instance of <see cref="IWait{T}"/> that can be used to retry or configure additional options.</returns>
        public static IAsyncWait<bool> Action(Func<Task> action)
        {
            return CreateAsyncWait(async (WaitContext<bool> ctx) =>
            {
                await action();
                return true;
            });
        }

        /// <summary>
        /// Creates a <see cref="IAsyncWait{Boolean}"/> object with a specified condition.
        /// </summary>
        /// <param name="action">The action to retry.</param>
        /// <returns>An instance of <see cref="IWait{T}"/> that can be used to retry or configure additional options.</returns>
        public static IAsyncWait<bool> Action(Func<WaitContext<bool>, Task> action)
        {
            return CreateAsyncWait(async (WaitContext<bool> ctx) =>
            {
                await action(ctx);
                return true;
            });
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
                SyncRetryName,
                OnStarting,
                OnFinished,
                OnExceptionOccurred);
        }

        private static IAsyncWait<T> CreateAsyncWait<T>(Func<WaitContext<T>, Task<T?>> condition)
        {
            return new AsyncWait<T>(
                condition,
                DefaultOptions,
                AsyncRetryName,
                OnStarting,
                OnFinished,
                OnExceptionOccurred);
        }
    }
}
