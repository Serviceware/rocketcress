using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable SA1402 // File may only contain a single type

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
    /// Builds a <see cref="WaitResult{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    public sealed class WaitResultBuilder<T>
    {
        private readonly List<Exception> _exceptions = new();
        private WaitResult _result;
        private T? _value;

        /// <summary>
        /// Gets the exceptions collected until now.
        /// </summary>
        public IReadOnlyList<Exception> Exceptions => new ReadOnlyCollection<Exception>(_exceptions);

        /// <summary>
        /// Gets the current result status.
        /// </summary>
        public WaitResult Result => _result;

        /// <summary>
        /// Gets the current value.
        /// </summary>
        public T? Value => _value;

        internal WaitResultBuilder()
        {
        }

        /// <summary>
        /// Defines the value for the resulting <see cref="WaitResult{T}"/>. Status is automatically set to <see cref="WaitResult.ValueAvailable"/> if not default value of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value">The value to use.</param>
        /// <returns>The same builder this method has been called on.</returns>
        public WaitResultBuilder<T> WithValue(T? value)
        {
            if (Equals(value, default))
                _result = WaitResult.ValueAvailable;
            _value = value;
            return this;
        }

        /// <summary>
        /// Defines the result status for the resulting <see cref="WaitResult{T}"/>. Value is automatically set to <c>default</c> if the result is not <see cref="WaitResult.ValueAvailable"/>.
        /// </summary>
        /// <param name="result">The result status to use.</param>
        /// <returns>The same builder this method has been called on.</returns>
        public WaitResultBuilder<T> WithResult(WaitResult result)
        {
            if (result != WaitResult.ValueAvailable)
                _value = default;
            _result = result;
            return this;
        }

        /// <summary>
        /// Adds an exception to the resulting <see cref="WaitResult{T}"/>.
        /// </summary>
        /// <param name="exception">The exception to add.</param>
        /// <returns>The same builder this method has been called on.</returns>
        public WaitResultBuilder<T> WithException(Exception exception)
        {
            _exceptions.Add(exception);
            return this;
        }

        /// <summary>
        /// Builds the <see cref="WaitResult{T}"/>.
        /// </summary>
        /// <param name="duration">The duration for the result.</param>
        /// <returns>The built <see cref="WaitResult{T}"/>.</returns>
        public WaitResult<T> Build(TimeSpan duration)
        {
            return new WaitResult<T>(_result, _value, duration, _exceptions.ToArray());
        }
    }

    /// <summary>
    /// Specifies the result of a <see cref="WaitResult{T}"/>.
    /// </summary>
    public enum WaitResult
    {
        /// <summary>
        /// No result.
        /// </summary>
        None,

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
        /// The wait operation has been aborted by the caller.
        /// </summary>
        CallerAborted,

        /// <summary>
        /// The condition has been checked too many times.
        /// </summary>
        TooManyRetries,
    }
}
