using System;

#pragma warning disable SA1402 // File may only contain a single type

namespace Rocketcress.Core
{
    /// <summary>
    /// Represents a method that handles the ExceptionOccured event of the Waiter.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ExceptionEventHandler(object? sender, ExceptionEventArgs e);

    /// <summary>
    /// Represents event arguments for the ExceptionOccured event of the Waiter.
    /// </summary>
    public class ExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the exception object that has been thrown during the waiting operation.
        /// </summary>
        public Exception ExceptionObject { get; }

        /// <summary>
        /// Gets the wait context.
        /// </summary>
        public WaitContext WaitContext { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionEventArgs"/> class.
        /// </summary>
        /// <param name="waitContext">The context of the wait operation.</param>
        /// <param name="exception">The exception object that has been thrown during the waiting operation.</param>
        public ExceptionEventArgs(WaitContext waitContext, Exception exception)
        {
            ExceptionObject = exception;
            WaitContext = waitContext;
        }
    }

    /// <summary>
    /// Represents event arguments for the ExceptionOccured event of the Waiter.
    /// </summary>
    /// <typeparam name="T">The type of value the wait operation returns.</typeparam>
    public class ExceptionEventArgs<T> : ExceptionEventArgs
    {
        /// <summary>
        /// Gets the wait context.
        /// </summary>
        public new WaitContext<T> WaitContext { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionEventArgs{T}"/> class.
        /// </summary>
        /// <param name="waitContext">The context of the wait operation.</param>
        /// <param name="exception">The exception object that has been thrown during the waiting operation.</param>
        public ExceptionEventArgs(WaitContext<T> waitContext, Exception exception)
            : base(waitContext, exception)
        {
            WaitContext = waitContext;
        }
    }
}
