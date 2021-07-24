using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#pragma warning disable SA1402 // File may only contain a single type

namespace Rocketcress.Core
{
    /// <summary>
    /// Represents the result of a wait operation.
    /// </summary>
    public class WaitResult
    {
        /// <summary>
        /// Gets the result status of the wait operation.
        /// </summary>
        public WaitResultStatus Status { get; }

        /// <summary>
        /// Gets the result value.
        /// </summary>
        public object? Value { get; }

        /// <summary>
        /// Gets the time the wait operation waited.
        /// </summary>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Gets all exception that were through during the wait operation.
        /// </summary>
        public Exception[] Exceptions { get; }

        internal WaitResult(WaitResultStatus status, object? value, TimeSpan duration, Exception[] exceptions)
        {
            Status = status;
            Value = value;
            Duration = duration;
            Exceptions = exceptions;
        }
    }

    /// <summary>
    /// Represents the result of a wait operation.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    public sealed class WaitResult<T> : WaitResult
    {
        /// <summary>
        /// Gets the result value.
        /// </summary>
        public new T? Value { get; }

        internal WaitResult(WaitResultStatus status, T? value, TimeSpan duration, Exception[] exceptions)
            : base(status, value, duration, exceptions)
        {
            Value = value;
        }
    }

    /// <summary>
    /// Builds a <see cref="WaitResult"/>.
    /// </summary>
    public abstract class WaitResultBuilder
    {
        private readonly List<Exception> _exceptions = new();
        private WaitResultStatus _status;

        /// <summary>
        /// Gets the exceptions collected until now.
        /// </summary>
        public IReadOnlyList<Exception> Exceptions => new ReadOnlyCollection<Exception>(_exceptions);

        /// <summary>
        /// Gets or sets the current result status.
        /// </summary>
        public WaitResultStatus Status
        {
            get => _status;
            protected set => _status = value;
        }

        /// <summary>
        /// Gets the current value.
        /// </summary>
        public object? Value => OnGetValue();

        /// <summary>
        /// Adds an exception to this builder.
        /// </summary>
        /// <param name="ex">The exception to add.</param>
        protected void AddException(Exception ex)
        {
            _exceptions.Add(ex);
        }

        /// <summary>
        /// This method is called when the <see cref="Value"/> property is called.
        /// </summary>
        /// <returns>The current value.</returns>
        protected abstract object? OnGetValue();
    }

    /// <summary>
    /// Builds a <see cref="WaitResult{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    public sealed class WaitResultBuilder<T> : WaitResultBuilder
    {
        private T? _value;

        /// <summary>
        /// Gets the current value.
        /// </summary>
        public new T? Value => _value;

        internal WaitResultBuilder()
        {
        }

        /// <summary>
        /// Defines the value for the resulting <see cref="WaitResult{T}"/>. Status is automatically set to <see cref="WaitResultStatus.ValueAvailable"/> if not default value of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value">The value to use.</param>
        /// <returns>The same builder this method has been called on.</returns>
        public WaitResultBuilder<T> WithValue(T? value)
        {
            if (Equals(value, default) && !Status.IsValueAvailable())
                Status = WaitResultStatus.ValueAvailable;
            _value = value;
            return this;
        }

        /// <summary>
        /// Defines the result status for the resulting <see cref="WaitResult{T}"/>. Value is automatically set to <c>default</c> if the result is not <see cref="WaitResultStatus.ValueAvailable"/>.
        /// </summary>
        /// <param name="result">The result status to use.</param>
        /// <returns>The same builder this method has been called on.</returns>
        public WaitResultBuilder<T> WithStatus(WaitResultStatus result)
        {
            if (!result.IsValueAvailable())
                _value = default;
            Status = result;
            return this;
        }

        /// <summary>
        /// Adds an exception to the resulting <see cref="WaitResult{T}"/>.
        /// </summary>
        /// <param name="exception">The exception to add.</param>
        /// <returns>The same builder this method has been called on.</returns>
        public WaitResultBuilder<T> WithException(Exception exception)
        {
            AddException(exception);
            return this;
        }

        /// <summary>
        /// Builds the <see cref="WaitResult{T}"/>.
        /// </summary>
        /// <param name="duration">The duration for the result.</param>
        /// <returns>The built <see cref="WaitResult{T}"/>.</returns>
        public WaitResult<T> Build(TimeSpan duration)
        {
            return new WaitResult<T>(Status, _value, duration, Exceptions.ToArray());
        }

        /// <inheritdoc/>
        protected override object? OnGetValue()
        {
            return _value;
        }
    }

    /// <summary>
    /// Specifies the result of a <see cref="WaitResult{T}"/>.
    /// </summary>
    [Flags]
    public enum WaitResultStatus
    {
        /// <summary>
        /// No result.
        /// </summary>
        None = 0,

        /// <summary>
        /// The condition function returned a value other than default in time.
        /// </summary>
        ValueAvailable = 1,

        /// <summary>
        /// The wait operation has been aborted by the caller but a value was not provided.
        /// </summary>
        CallerAbortedWithoutValue = 0b10,

        /// <summary>
        /// The wait operation has been aborted by the caller and a value was provided.
        /// </summary>
        CallerAbortedWithValue = 0b11,

        /// <summary>
        /// The wait operation ran into a timeout.
        /// </summary>
        Timeout = 0b100,

        /// <summary>
        /// Too many exceptions have been thrown during the wait operation.
        /// </summary>
        TooManyExceptions = 0b1000,

        /// <summary>
        /// The condition has been checked too many times.
        /// </summary>
        TooManyRetries = 0b1100,
    }

    /// <summary>
    /// Provdes extension methods for the <see cref="WaitResultStatus"/> enum.
    /// </summary>
    public static class WaitResultStatusExtensions
    {
        private const WaitResultStatus ValueMask = (WaitResultStatus)0b1;
        private const WaitResultStatus AbortedMask = (WaitResultStatus)0b10;

        /// <summary>
        /// Determines whether a given status indicates that a value is available.
        /// </summary>
        /// <param name="status">The status to check.</param>
        /// <returns><c>true</c> if the <paramref name="status"/> indicates that a value is available; otherwise <c>false</c>.</returns>
        public static bool IsValueAvailable(this WaitResultStatus status)
        {
            return status.HasFlag(ValueMask);
        }

        /// <summary>
        /// Determines whether a given status indicates that the operatoin has been aborted.
        /// </summary>
        /// <param name="status">The status to check.</param>
        /// <returns><c>true</c> if the <paramref name="status"/> indicates that the operatoin has been aborted; otherwise <c>false</c>.</returns>
        public static bool IsAbort(this WaitResultStatus status)
        {
            return status.HasFlag(AbortedMask);
        }
    }
}
