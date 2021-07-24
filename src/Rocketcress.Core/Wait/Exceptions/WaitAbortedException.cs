using System;

#pragma warning disable SA1402 // File may only contain a single type

namespace Rocketcress.Core
{
    /// <summary>
    /// Represents errors that occur during wait operations to abort them.
    /// </summary>
    public class WaitAbortedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WaitAbortedException"/> class.
        /// </summary>
        public WaitAbortedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitAbortedException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public WaitAbortedException(string? message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitAbortedException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public WaitAbortedException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Represents errors that occur during wait operations to abort them.
    /// </summary>
    /// <typeparam name="T">The type of value to return.</typeparam>
    public class WaitAbortedException<T> : WaitAbortedException
    {
        /// <summary>
        /// Gets the value to use as return value for the wait operation.
        /// </summary>
        public T? Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitAbortedException{T}"/> class.
        /// </summary>
        /// <param name="value">The value to use as return value for the wait operation.</param>
        public WaitAbortedException(T? value)
        {
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitAbortedException{T}"/> class.
        /// </summary>
        /// <param name="value">The value to use as return value for the wait operation.</param>
        /// <param name="message">The message that describes the error.</param>
        public WaitAbortedException(T? value, string? message)
            : base(message)
        {
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitAbortedException{T}"/> class.
        /// </summary>
        /// <param name="value">The value to use as return value for the wait operation.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public WaitAbortedException(T? value, string? message, Exception? innerException)
            : base(message, innerException)
        {
            Value = value;
        }
    }
}
