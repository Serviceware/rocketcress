using System;

namespace Rocketcress.Core
{
    /// <summary>
    /// Wait options for a wait operation.
    /// </summary>
    public interface IWaitOptions : ICloneable
    {
        /// <summary>
        /// Gets or sets a value indicating whether exceptions during wait operations should be traced.
        /// </summary>
        public bool TraceExceptions { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of accepted exceptions during wait operations.
        /// If more than the specified exceptions are thrown during a wait operation, it will fail.
        /// If set to <c>null</c>, the accepted exceptions are infinite.
        /// </summary>
        public int? MaxAcceptedExceptions { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of time the condition should be retried during wait operations.
        /// If the condition does not succeed after the number of calls during a wait operation, it will fail.
        /// If set to <c>null</c>, the condition is retried infinitely (until the timeout occurs).
        /// </summary>
        public int? MaxRetryCount { get; set; }

        /// <summary>
        /// Gets or sets the timeout for wait operations.
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// Gets or sets the timeout for wait operations in milliseconds.
        /// </summary>
        public int TimeoutMs { get; set; }

        /// <summary>
        /// Gets or sets the time to wait between checking the condition during wait operations.
        /// </summary>
        public TimeSpan TimeGap { get; set; }

        /// <summary>
        /// Gets or sets the time in milliseconds to wait between checking the condition during wait operations.
        /// </summary>
        public int TimeGapMs { get; set; }
    }

    /// <summary>
    /// Read-only wait options for a wait operation.
    /// </summary>
    public interface IReadOnlyWaitOptions
    {
        /// <summary>
        /// Gets a value indicating whether exceptions during wait operations should be traced.
        /// </summary>
        public bool TraceExceptions { get; }

        /// <summary>
        /// Gets the maximum number of accepted exceptions during wait operations.
        /// If more than the specified exceptions are thrown during a wait operation, it will fail.
        /// If set to <c>null</c>, the accepted exceptions are infinite.
        /// </summary>
        public int? MaxAcceptedExceptions { get; }

        /// <summary>
        /// Gets the maximum number of time the condition should be retried during wait operations.
        /// If the condition does not succeed after the number of calls during a wait operation, it will fail.
        /// If set to <c>null</c>, the condition is retried infinitely (until the timeout occurs).
        /// </summary>
        public int? MaxRetryCount { get; }

        /// <summary>
        /// Gets the timeout for wait operations.
        /// </summary>
        public TimeSpan Timeout { get; }

        /// <summary>
        /// Gets the timeout for wait operations in milliseconds.
        /// </summary>
        public int TimeoutMs { get; }

        /// <summary>
        /// Gets the time to wait between checking the condition during wait operations.
        /// </summary>
        public TimeSpan TimeGap { get; }

        /// <summary>
        /// Gets the time in milliseconds to wait between checking the condition during wait operations.
        /// </summary>
        public int TimeGapMs { get; }
    }

    /// <summary>
    /// Obsolete options for a wait operation.
    /// </summary>
    public interface IObsoleteWaitOptions : IWaitOptions
    {
        /// <summary>
        /// Gets or sets the default timeout for wait operations.
        /// </summary>
        [Obsolete("Use Timeout property instead.")]
        public TimeSpan DefaultTimeout { get; set; }

        /// <summary>
        /// Gets or sets the default timeout for wait operations in milliseconds.
        /// </summary>
        [Obsolete("Use TimeoutMs property instead.")]
        public int DefaultTimeoutMs { get; set; }

        /// <summary>
        /// Gets or sets the time to wait between checking the condition during wait operations.
        /// </summary>
        [Obsolete("Use Timeout property instead.")]
        public TimeSpan DefaultTimeGap { get; set; }

        /// <summary>
        /// Gets or sets the time in milliseconds to wait between checking the condition during wait operations.
        /// </summary>
        [Obsolete("Use TimeGapMs property instead.")]
        public int DefaultTimeGapMs { get; set; }
    }
}
