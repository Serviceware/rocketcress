using System;

namespace Rocketcress.Core
{
    /// <summary>
    /// Represents the result of a wait operation.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    public sealed class WaitResult<T>
    {
        /// <summary>
        /// Gets the result status of the wait operation.
        /// </summary>
        public WaitResult Result { get; }

        /// <summary>
        /// Gets the result value.
        /// </summary>
        public T? Value { get; }

        /// <summary>
        /// Gets the time the wait operation waited.
        /// </summary>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Gets all exception that were through during the wait operation.
        /// </summary>
        public Exception[] Exceptions { get; }

        internal WaitResult(WaitResult status, T? value, TimeSpan duration, Exception[] exceptions)
        {
            Result = status;
            Value = value;
            Duration = duration;
            Exceptions = exceptions;
        }
    }

    /// <summary>
    /// Specifies the result of a <see cref="WaitResult{T}"/>.
    /// </summary>
    public enum WaitResult
    {
        /// <summary>
        /// The condition function returned a value other than default in time.
        /// </summary>
        ValueAvailable,

        /// <summary>
        /// The wait operation ran into a timeout.
        /// </summary>
        Timeout,

        /// <summary>
        /// Too many exceptions have been thrown during the wait operation.
        /// </summary>
        TooManyExceptions,

        /// <summary>
        /// A result has been defined by the caller.
        /// </summary>
        CallerResult,

        /// <summary>
        /// The wait operation has been aborted by the caller.
        /// </summary>
        CallerAborted,
    }
}
