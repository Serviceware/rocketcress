namespace Rocketcress.Core
{
    /// <summary>
    /// Provides options for the <see cref="Wait"/> class.
    /// </summary>
    public sealed class WaitOptions
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
        /// Gets or sets the default timeout for wait operations.
        /// </summary>
        public TimeSpan DefaultTimeout { get; set; }

        /// <summary>
        /// Gets or sets the default timeout for wait operations in milliseconds.
        /// </summary>
        public int DefaultTimeoutMs
        {
            get => (int)DefaultTimeout.TotalMilliseconds;
            set => DefaultTimeout = TimeSpan.FromMilliseconds(value);
        }

        /// <summary>
        /// Gets or sets the time to wait between checking the condition during a wait operation.
        /// </summary>
        public TimeSpan DefaultTimeGap { get; set; }

        /// <summary>
        /// Gets or sets the time in milliseconds to wait between checking the condition during a wait operation.
        /// </summary>
        public int DefaultTimeGapMs
        {
            get => (int)DefaultTimeGap.TotalMilliseconds;
            set => DefaultTimeGap = TimeSpan.FromMilliseconds(value);
        }

        internal WaitOptions()
        {
            TraceExceptions = true;
            MaxAcceptedExceptions = null;
            DefaultTimeout = TimeSpan.FromSeconds(10);
            DefaultTimeGap = TimeSpan.FromMilliseconds(100);
        }
    }
}
