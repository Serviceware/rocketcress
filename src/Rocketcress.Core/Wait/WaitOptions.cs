using System;

namespace Rocketcress.Core
{
    /// <summary>
    /// Provides options for the <see cref="Wait"/> class.
    /// </summary>
    public sealed class WaitOptions : IWaitOptions, IObsoleteWaitOptions, IReadOnlyWaitOptions
    {
        /// <inheritdoc/>
        public bool TraceExceptions { get; set; }

        /// <inheritdoc/>
        public int? MaxAcceptedExceptions { get; set; }

        /// <inheritdoc/>
        public int? MaxRetryCount { get; set; }

        /// <inheritdoc/>
        public TimeSpan Timeout { get; set; }

        /// <inheritdoc/>
        public int TimeoutMs
        {
            get => GetMilliseconds(Timeout);
            set => Timeout = TimeSpan.FromMilliseconds(value);
        }

        /// <inheritdoc/>
        public TimeSpan TimeGap { get; set; }

        /// <inheritdoc/>
        public int TimeGapMs
        {
            get => GetMilliseconds(TimeGap);
            set => TimeGap = TimeSpan.FromMilliseconds(value);
        }

        #region Obsolete

        /// <inheritdoc/>
        public TimeSpan DefaultTimeout { get; set; }

        /// <inheritdoc/>
        public int DefaultTimeoutMs
        {
            get => TimeoutMs;
            set => TimeoutMs = value;
        }

        /// <inheritdoc/>
        public TimeSpan DefaultTimeGap { get; set; }

        /// <inheritdoc/>
        public int DefaultTimeGapMs
        {
            get => TimeGapMs;
            set => TimeGapMs = value;
        }

        #endregion

        internal WaitOptions()
        {
        }

        /// <inheritdoc/>
        public object Clone()
        {
            return new WaitOptions
            {
                TraceExceptions = TraceExceptions,
                MaxAcceptedExceptions = MaxAcceptedExceptions,
                MaxRetryCount = MaxRetryCount,
                Timeout = Timeout,
                TimeGap = TimeGap,
            };
        }

        private static int GetMilliseconds(TimeSpan timeSpan)
        {
            unchecked
            {
                return Math.Max((int)timeSpan.TotalMilliseconds, int.MaxValue);
            }
        }
    }
}
