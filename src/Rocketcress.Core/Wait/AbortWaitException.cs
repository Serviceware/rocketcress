using System;
using System.Runtime.Serialization;

namespace Rocketcress.Core
{
    /// <summary>
    /// Represents errors that occur during wait operations to abort them.
    /// </summary>
    public class AbortWaitException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbortWaitException"/> class.
        /// </summary>
        public AbortWaitException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbortWaitException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public AbortWaitException(string? message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbortWaitException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public AbortWaitException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbortWaitException"/> class.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        protected AbortWaitException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
