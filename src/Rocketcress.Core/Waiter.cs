using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Rocketcress.Core
{
    /// <summary>
    /// Represents a method that handles the WaitingStarting and WaitingEnded events of the Waiter.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void WaitingEventHandler(object sender, WaitingEventArgs e);

    /// <summary>
    /// Represents a method that handles the ExceptionOccured event of the Waiter.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ExceptionEventHandler(object sender, ExceptionEventArgs e);

    /// <summary>
    /// Provides methods for waiting until something happens.
    /// </summary>
    public static class Waiter
    {
        private static AssertEx Assert => AssertEx.Instance;

        /// <summary>
        /// Event that occurres when a waiting operation is starting.
        /// </summary>
        public static event WaitingEventHandler WaitingStarting;

        /// <summary>
        /// Event that occurres when a waiting operation has ended.
        /// </summary>
        public static event WaitingEventHandler WaitingEnded;

        /// <summary>
        /// Event that occurres when an exception has been occurres during a waiting operation.
        /// </summary>
        public static event ExceptionEventHandler ExceptionOccured;

        /// <summary>
        /// Gets or sets a value indicating whether exceptions during waiting operations should be traced.
        /// </summary>
        public static bool TraceExceptions { get; set; }

        /// <summary>
        /// Gets or sets the maximal accepted exceptions during waiting operations. If more that the specified exceptions are thrown the waiting operation fails.
        /// If the value is set to null, the accepted exceptions are infinite.
        /// </summary>
        public static int? MaxAcceptedExceptions { get; set; }

        /// <summary>
        /// Gets or sets the default timeout for waiting operations.
        /// </summary>
        public static TimeSpan DefaultTimeout { get; set; }

        /// <summary>
        /// Gets or sets the default timeout for waiting operations in miliseconds.
        /// </summary>
        public static int DefaultTimeoutMs
        {
            get => (int)DefaultTimeout.TotalMilliseconds;
            set => DefaultTimeout = TimeSpan.FromMilliseconds(value);
        }

        /// <summary>
        /// Gets or sets the time to wait between checking during a waiting operation.
        /// </summary>
        public static int DefaultWaitBetweenChecks { get; set; }

        static Waiter()
        {
            TraceExceptions = true;
            MaxAcceptedExceptions = 5;
            DefaultTimeout = TimeSpan.FromSeconds(10);
            DefaultWaitBetweenChecks = 100;
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
        public static T WaitUntil<T>(Func<T> condition, Action<Exception> onError, TimeSpan timeout, int waitBetweenChecks, out TimeSpan duration, bool assert = false, string assertMsg = null)
        {
            var data = new Dictionary<string, object>();
            WaitingStarting?.Invoke(null, new WaitingEventArgs(data));
            var defaultMsg = $"The waiting action timed out after {timeout.TotalSeconds:0.###} seconds";
            try
            {
                var start = DateTime.Now;
                int exceptionCount = 0;
                while (DateTime.Now - start < timeout)
                {
                    try
                    {
                        var result = condition();
                        if (!Equals(result, default(T)))
                        {
                            duration = DateTime.Now - start;
                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!HandleException(ref exceptionCount, onError, ex))
                        {
                            defaultMsg = $"{exceptionCount} exceptions occured while waiting, which exceeds the maximum allowed exception count of {MaxAcceptedExceptions}.";
                            break;
                        }
                    }

                    Thread.Sleep(waitBetweenChecks);
                }

                duration = DateTime.Now - start;
            }
            finally
            {
                WaitingEnded?.Invoke(null, new WaitingEventArgs(data));
            }

            if (assert)
                Assert.Fail(assertMsg ?? defaultMsg);
            else
                Logger.LogWarning(defaultMsg);
            return default;
        }

        private static bool HandleException(ref int exceptionCount, Action<Exception> callback, Exception exception)
        {
            exceptionCount++;
            if (TraceExceptions)
                Logger.LogDebug("Exception while waiting: " + exception.ToString());

            try
            {
                callback?.Invoke(exception);
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Excpetion while calling error-callback: " + ex);
            }

            try
            {
                ExceptionOccured?.Invoke(null, new ExceptionEventArgs(exception));
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Exception while calling event 'ExcpetionOccured': " + ex);
            }

            return !MaxAcceptedExceptions.HasValue || exceptionCount <= MaxAcceptedExceptions;
        }
    }

    /// <summary>
    /// Represents event arguments for the WaitingStarting and WaitingEnded events of the Waiter.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Event arguments")]
    public class WaitingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets a data store you can use on another event on this waiting operation.
        /// </summary>
        public Dictionary<string, object> DataStore { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitingEventArgs"/> class.
        /// </summary>
        /// <param name="dataStore">A data store you can use on another event on this waiting operation.</param>
        public WaitingEventArgs(Dictionary<string, object> dataStore)
        {
            DataStore = dataStore;
        }
    }

    /// <summary>
    /// Represents event arguments for the ExceptionOccured event of the Waiter.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Event arguments")]
    public class ExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the exception object that has been thrown during the waiting operation.
        /// </summary>
        public Exception ExceptionObject { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionEventArgs"/> class.
        /// </summary>
        /// <param name="exception">The exception object that has been thrown during the waiting operation.</param>
        public ExceptionEventArgs(Exception exception)
        {
            ExceptionObject = exception;
        }
    }
}
