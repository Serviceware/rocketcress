#nullable disable

namespace Rocketcress.Core
{
    /// <summary>
    /// Provides methods for waiting until something happens.
    /// </summary>
    public static class Waiter
    {
        /// <summary>
        /// Event that occurres when a waiting operation is starting.
        /// </summary>
        [Obsolete("Use Wait.WhenStarting instead.")]
        public static event WaitingEventHandler WaitingStarting
        {
            add => Wait.WhenStarting += value;
            remove => Wait.WhenStarting -= value;
        }

        /// <summary>
        /// Event that occurres when a waiting operation has ended.
        /// </summary>
        [Obsolete("Use Wait.WhenFinished instead.")]
        public static event WaitingEventHandler WaitingEnded
        {
            add => Wait.WhenFinished += value;
            remove => Wait.WhenFinished -= value;
        }

        /// <summary>
        /// Event that occurres when an exception has been occurres during a waiting operation.
        /// </summary>
        [Obsolete("Use Wait.WhenExceptionOccurred instead.")]
        public static event ExceptionEventHandler ExceptionOccured
        {
            add => Wait.WhenExceptionOccurred += value;
            remove => Wait.WhenExceptionOccurred -= value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether exceptions during waiting operations should be traced.
        /// </summary>
        [Obsolete("Use Wait.Options.TraceExceptions instead.")]
        public static bool TraceExceptions
        {
            get => Wait.Options.TraceExceptions;
            set => Wait.Options.TraceExceptions = value;
        }

        /// <summary>
        /// Gets or sets the maximal accepted exceptions during waiting operations. If more that the specified exceptions are thrown the waiting operation fails.
        /// If the value is set to null, the accepted exceptions are infinite.
        /// </summary>
        [Obsolete("Use Wait.Options.MaxAcceptedExceptions instead.")]
        public static int? MaxAcceptedExceptions
        {
            get => Wait.Options.MaxAcceptedExceptions;
            set => Wait.Options.MaxAcceptedExceptions = value;
        }

        /// <summary>
        /// Gets or sets the default timeout for waiting operations.
        /// </summary>
        [Obsolete("Use Wait.Options.DefaultTimeout instead.")]
        public static TimeSpan DefaultTimeout
        {
            get => Wait.Options.DefaultTimeout;
            set => Wait.Options.DefaultTimeout = value;
        }

        /// <summary>
        /// Gets or sets the default timeout for waiting operations in miliseconds.
        /// </summary>
        [Obsolete("Use Wait.Options.DefaultTimeout instead.")]
        public static int DefaultTimeoutMs
        {
            get => (int)Wait.Options.DefaultTimeout.TotalMilliseconds;
            set => Wait.Options.DefaultTimeout = TimeSpan.FromMilliseconds(value);
        }

        /// <summary>
        /// Gets or sets the time to wait between checking during a waiting operation.
        /// </summary>
        [Obsolete("Use Wait.Options.DefaultTimeGap instead.")]
        public static int DefaultWaitBetweenChecks
        {
            get => (int)Wait.Options.DefaultTimeGap.TotalMilliseconds;
            set => Wait.Options.DefaultTimeGap = TimeSpan.FromMilliseconds(value);
        }

        #region WaitUntil overloads

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).Start() instead. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, bool assert = false, string assertMsg = null)
        {
            return WaitUntil(condition, null, DefaultTimeout, DefaultWaitBetweenChecks, assert, assertMsg);
        }

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="duration">The duration the waiting operation took.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).Start() instead and get the duration from the result. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, out TimeSpan duration, bool assert = false, string assertMsg = null)
        {
            return WaitUntil(condition, null, DefaultTimeout, DefaultWaitBetweenChecks, out duration, assert, assertMsg);
        }

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="timeout">The timeout for the waiting operation in miliseconds.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).WithTimeout(timeout).Start() instead. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, int timeout, bool assert = false, string assertMsg = null)
        {
            return WaitUntil(condition, null, TimeSpan.FromMilliseconds(timeout), DefaultWaitBetweenChecks, assert, assertMsg);
        }

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="timeout">The timeout for the waiting operation in miliseconds.</param>
        /// <param name="duration">The duration the waiting operation took.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).WithTimeout(timeout).Start() instead and get the duration from the result. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, int timeout, out TimeSpan duration, bool assert = false, string assertMsg = null)
        {
            return WaitUntil(condition, null, TimeSpan.FromMilliseconds(timeout), DefaultWaitBetweenChecks, out duration, assert, assertMsg);
        }

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="timeout">The timeout for the waiting operation in miliseconds.</param>
        /// <param name="waitBetweenChecks">The time to wait between checking during the waiting operation.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).WithTimeout(timeout).WithTimeGap(waitBetweenChecks).Start() instead. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, int timeout, int waitBetweenChecks, bool assert = false, string assertMsg = null)
        {
            return WaitUntil(condition, null, TimeSpan.FromMilliseconds(timeout), waitBetweenChecks, assert, assertMsg);
        }

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="timeout">The timeout for the waiting operation in miliseconds.</param>
        /// <param name="waitBetweenChecks">The time to wait between checking during the waiting operation.</param>
        /// <param name="duration">The duration the waiting operation took.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).WithTimeout(timeout).WithTimeGap(waitBetweenChecks).Start() instead and get the duration from the result. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, int timeout, int waitBetweenChecks, out TimeSpan duration, bool assert = false, string assertMsg = null)
        {
            return WaitUntil(condition, null, TimeSpan.FromMilliseconds(timeout), waitBetweenChecks, out duration, assert, assertMsg);
        }

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="timeout">The timeout for the waiting operation.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).WithTimeout(timeout).Start() instead. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, TimeSpan timeout, bool assert = false, string assertMsg = null)
        {
            return WaitUntil(condition, null, timeout, DefaultWaitBetweenChecks, assert, assertMsg);
        }

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="timeout">The timeout for the waiting operation.</param>
        /// <param name="duration">The duration the waiting operation took.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).WithTimeout(timeout).Start() instead and get the duration from the result. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, TimeSpan timeout, out TimeSpan duration, bool assert = false, string assertMsg = null)
        {
            return WaitUntil(condition, null, timeout, DefaultWaitBetweenChecks, out duration, assert, assertMsg);
        }

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="timeout">The timeout for the waiting operation.</param>
        /// <param name="waitBetweenChecks">The time to wait between checking during the waiting operation.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).WithTimeout(timeout).WithTimeGap(waitBetweenChecks).Start() instead. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, TimeSpan timeout, int waitBetweenChecks, bool assert = false, string assertMsg = null)
        {
            return WaitUntil(condition, null, timeout, waitBetweenChecks, assert, assertMsg);
        }

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="timeout">The timeout for the waiting operation.</param>
        /// <param name="waitBetweenChecks">The time to wait between checking during the waiting operation.</param>
        /// <param name="duration">The duration the waiting operation took.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).WithTimeout(timeout).WithTimeGap(waitBetweenChecks).Start() instead and get the duration from the result. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, TimeSpan timeout, int waitBetweenChecks, out TimeSpan duration, bool assert = false, string assertMsg = null)
        {
            return WaitUntil(condition, null, timeout, waitBetweenChecks, out duration, assert, assertMsg);
        }

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="onError">Function that is executed when an exception occurres during the waiting operation.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).OnError().Call(onError).Start() instead. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, Action<Exception> onError, bool assert = false, string assertMsg = null)
        {
            return WaitUntil(condition, onError, DefaultTimeout, DefaultWaitBetweenChecks, assert, assertMsg);
        }

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="onError">Function that is executed when an exception occurres during the waiting operation.</param>
        /// <param name="duration">The duration the waiting operation took.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).OnError().Call(onError).Start() instead and get the duration from the result. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, Action<Exception> onError, out TimeSpan duration, bool assert = false, string assertMsg = null)
        {
            return WaitUntil(condition, onError, DefaultTimeout, DefaultWaitBetweenChecks, out duration, assert, assertMsg);
        }

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="onError">Function that is executed when an exception occurres during the waiting operation.</param>
        /// <param name="timeout">The timeout for the waiting operation in miliseconds.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).OnError().Call(onError).WithTimeout(timeout).Start() instead. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, Action<Exception> onError, int timeout, bool assert = false, string assertMsg = null)
        {
            return WaitUntil(condition, onError, TimeSpan.FromMilliseconds(timeout), DefaultWaitBetweenChecks, assert, assertMsg);
        }

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="onError">Function that is executed when an exception occurres during the waiting operation.</param>
        /// <param name="timeout">The timeout for the waiting operation in miliseconds.</param>
        /// <param name="duration">The duration the waiting operation took.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).OnError().Call(onError).WithTimeout(timeout).Start() instead and get the duration from the result. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, Action<Exception> onError, int timeout, out TimeSpan duration, bool assert = false, string assertMsg = null)
        {
            return WaitUntil(condition, onError, TimeSpan.FromMilliseconds(timeout), DefaultWaitBetweenChecks, out duration, assert, assertMsg);
        }

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="onError">Function that is executed when an exception occurres during the waiting operation.</param>
        /// <param name="timeout">The timeout for the waiting operation in miliseconds.</param>
        /// <param name="waitBetweenChecks">The time to wait between checking during the waiting operation.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).OnError().Call(onError).WithTimeout(timeout).WithTimeGap(waitBetweenChecks).Start() instead. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, Action<Exception> onError, int timeout, int waitBetweenChecks, bool assert = false, string assertMsg = null)
        {
            return WaitUntil(condition, onError, TimeSpan.FromMilliseconds(timeout), waitBetweenChecks, assert, assertMsg);
        }

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="onError">Function that is executed when an exception occurres during the waiting operation.</param>
        /// <param name="timeout">The timeout for the waiting operation in miliseconds.</param>
        /// <param name="waitBetweenChecks">The time to wait between checking during the waiting operation.</param>
        /// <param name="duration">The duration the waiting operation took.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).OnError().Call(onError).WithTimeout(timeout).WithTimeGap(waitBetweenChecks).Start() instead and get the duration from the result. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, Action<Exception> onError, int timeout, int waitBetweenChecks, out TimeSpan duration, bool assert = false, string assertMsg = null)
        {
            return WaitUntil(condition, onError, TimeSpan.FromMilliseconds(timeout), waitBetweenChecks, out duration, assert, assertMsg);
        }

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="onError">Function that is executed when an exception occurres during the waiting operation.</param>
        /// <param name="timeout">The timeout for the waiting operation.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).OnError().Call(onError).WithTimeout(timeout).Start() instead. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, Action<Exception> onError, TimeSpan timeout, bool assert = false, string assertMsg = null)
        {
            return WaitUntil(condition, onError, timeout, DefaultWaitBetweenChecks, assert, assertMsg);
        }

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="onError">Function that is executed when an exception occurres during the waiting operation.</param>
        /// <param name="timeout">The timeout for the waiting operation.</param>
        /// <param name="duration">The duration the waiting operation took.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).OnError().Call(onError).WithTimeout(timeout).Start() instead and get the duration from the result. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, Action<Exception> onError, TimeSpan timeout, out TimeSpan duration, bool assert = false, string assertMsg = null)
        {
            return WaitUntil(condition, onError, timeout, DefaultWaitBetweenChecks, out duration, assert, assertMsg);
        }
        #endregion

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="onError">Function that is executed when an exception occurres during the waiting operation.</param>
        /// <param name="timeout">The timeout for the waiting operation.</param>
        /// <param name="waitBetweenChecks">The time to wait between checking during the waiting operation.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).OnError().Call(onError).WithTimeout(timeout).WithTimeGap(waitBetweenChecks).Start() instead. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, Action<Exception> onError, TimeSpan timeout, int waitBetweenChecks, bool assert = false, string assertMsg = null)
        {
            return WaitUntil(condition, onError, timeout, waitBetweenChecks, out TimeSpan _, assert, assertMsg);
        }

        /// <summary>
        /// Waits until a specific condition is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the condition result.</typeparam>
        /// <param name="condition">The condition function. The waiting operation ends when the returned value equals the default value of the specified type.</param>
        /// <param name="onError">Function that is executed when an exception occurres during the waiting operation.</param>
        /// <param name="timeout">The timeout for the waiting operation.</param>
        /// <param name="waitBetweenChecks">The time to wait between checking during the waiting operation.</param>
        /// <param name="duration">The duration the waiting operation took.</param>
        /// <param name="assert">Determines whether an assertion should happen when a timeout occurres.</param>
        /// <param name="assertMsg">The message that should be displayed when asserting.</param>
        /// <returns>Returns the last non-default value of the condition function when a timeout has not been reached; otherwise the default value of the specified type.</returns>
        [Obsolete("Use Wait.Until(condition).OnError().Call(onError).WithTimeout(timeout).WithTimeGap(waitBetweenChecks).Start() instead and get the duration from the result. [for asserting add .ThrowOnFailure(assertMsg) before .Start()]")]
        public static T WaitUntil<T>(Func<T> condition, Action<Exception> onError, TimeSpan timeout, int waitBetweenChecks, out TimeSpan duration, bool assert = false, string assertMsg = null)
        {
            var wait = Wait
                .Until(condition)
                .WithTimeout(timeout)
                .WithTimeGap(TimeSpan.FromMilliseconds(waitBetweenChecks));

            if (onError != null)
                wait.OnError().Call(onError);
            if (assert)
                wait.ThrowOnFailure(assertMsg);

            var result = wait.Start();
            duration = result.Duration;
            return result.Value;
        }
    }
}
