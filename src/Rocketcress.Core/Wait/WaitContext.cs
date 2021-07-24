using System;
using System.Collections.Generic;
using System.Diagnostics;

#pragma warning disable SA1402 // File may only contain a single type

namespace Rocketcress.Core
{
    /// <summary>
    /// Represents the context of a wait operation.
    /// </summary>
    public abstract class WaitContext
    {
        private readonly Stopwatch _stopwatch;

        /// <summary>
        /// Gets the options used for this operation.
        /// </summary>
        public IReadOnlyWaitOptions Options { get; }

        /// <summary>
        /// Gets a dictionary that can be used to store data for this operation.
        /// </summary>
        public IDictionary<string, object> Data { get; }

        /// <summary>
        /// Gets a value indicating whether this operation should throw an exception when it fails.
        /// </summary>
        public bool ThrowOnFailure { get; }

        /// <summary>
        /// Gets the error message to include in the exception that occurs when this operation fails.
        /// </summary>
        public string? ErrorMessage { get; }

        /// <summary>
        /// Gets the current result for this operation.
        /// </summary>
        public WaitResultBuilder Result { get; }

        /// <summary>
        /// Gets the current duration of this wait operation.
        /// Time is only measures during waiting, so in precede functions the duration is always 0 and in continue functions it is always the duration of the wait.
        /// </summary>
        public TimeSpan Duration => _stopwatch.Elapsed;

        /// <summary>
        /// Gets the current number of times the wait condition function has been called.
        /// </summary>
        public int CheckCount { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="throwOnFailure">A value indicating whether this operation should throw an exception when it fails.</param>
        /// <param name="errorMessage">The error message to include in the exception that occurs when this operation fails.</param>
        /// <param name="result">The current result for this operation.</param>
        protected WaitContext(IReadOnlyWaitOptions options, bool throwOnFailure, string? errorMessage, WaitResultBuilder result)
        {
            _stopwatch = new Stopwatch();
            Data = new Dictionary<string, object>();
            Options = options;
            ThrowOnFailure = throwOnFailure;
            ErrorMessage = errorMessage;
            Result = result;
        }

        internal void Start()
        {
            _stopwatch.Restart();
        }

        internal void Stop()
        {
            _stopwatch.Stop();
        }
    }

    /// <summary>
    /// Represents the context of a wait operation.
    /// </summary>
    /// <typeparam name="T">The type of value the wait operation provides.</typeparam>
    public sealed class WaitContext<T> : WaitContext
    {
        /// <summary>
        /// Gets the current result for this operation.
        /// </summary>
        public new WaitResultBuilder<T> Result { get; }

        internal WaitContext(IReadOnlyWaitOptions options, bool throwOnFailure, string? errorMessage)
            : base(options, throwOnFailure, errorMessage, new WaitResultBuilder<T>())
        {
            Result = (WaitResultBuilder<T>)base.Result;
        }

        internal WaitResult<T> GetResult()
        {
            return Result.Build(Duration);
        }

        internal bool Next()
        {
            if (Duration >= Options.Timeout)
            {
                Result.WithStatus(WaitResultStatus.Timeout);
                return false;
            }

            if (CheckCount > 0 && Options.MaxRetryCount.HasValue && CheckCount > Options.MaxRetryCount.Value)
            {
                Result.WithStatus(WaitResultStatus.TooManyRetries);
                return false;
            }

            CheckCount++;
            return true;
        }
    }
}
