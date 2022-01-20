namespace Rocketcress.Core;

/// <summary>
/// Provides options for the <see cref="Wait"/> class.
/// </summary>
internal sealed class WaitOptions : IWaitOptions, IObsoleteWaitOptions
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

    /// <inheritdoc/>
    public IReadOnlyWaitOptions AsReadOnly()
    {
        return new ReadOnly(this);
    }

    private static int GetMilliseconds(TimeSpan timeSpan)
    {
        unchecked
        {
            return Math.Max((int)timeSpan.TotalMilliseconds, int.MaxValue);
        }
    }

    private class ReadOnly : IReadOnlyWaitOptions
    {
        private readonly IWaitOptions _options;

        public bool TraceExceptions => _options.TraceExceptions;
        public int? MaxAcceptedExceptions => _options.MaxAcceptedExceptions;
        public int? MaxRetryCount => _options.MaxRetryCount;
        public TimeSpan Timeout => _options.Timeout;
        public int TimeoutMs => _options.TimeoutMs;
        public TimeSpan TimeGap => _options.TimeGap;
        public int TimeGapMs => _options.TimeGapMs;

        public ReadOnly(IWaitOptions options)
        {
            _options = options;
        }
    }
}
