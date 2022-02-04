using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Rocketcress.Core;

/// <summary>
/// Provides options for the <see cref="Wait"/> class.
/// </summary>
internal sealed class WaitOptions : IWaitOptions, IObsoleteWaitOptions
{
    private bool _traceExceptions;
    private int? _maxAcceptedExceptions;
    private int? _maxRetryCount;
    private TimeSpan _timeout;
    private TimeSpan _timeGap;

    internal WaitOptions()
    {
    }

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc/>
    public bool TraceExceptions
    {
        get => _traceExceptions;
        set => SetProperty(value, ref _traceExceptions);
    }

    /// <inheritdoc/>
    public int? MaxAcceptedExceptions
    {
        get => _maxAcceptedExceptions;
        set => SetProperty(value, ref _maxAcceptedExceptions);
    }

    /// <inheritdoc/>
    public int? MaxRetryCount
    {
        get => _maxRetryCount;
        set => SetProperty(value, ref _maxRetryCount);
    }

    /// <inheritdoc/>
    public TimeSpan Timeout
    {
        get => _timeout;
        set
        {
            if (SetProperty(value, ref _timeout))
                OnPropertyChanged(nameof(TimeoutMs));
        }
    }

    /// <inheritdoc/>
    public int TimeoutMs
    {
        get => (int)Timeout.TotalMilliseconds;
        set => Timeout = TimeSpan.FromMilliseconds(value);
    }

    /// <inheritdoc/>
    public TimeSpan TimeGap
    {
        get => _timeGap;
        set
        {
            if (SetProperty(value, ref _timeGap))
                OnPropertyChanged(nameof(TimeGapMs));
        }
    }

    /// <inheritdoc/>
    public int TimeGapMs
    {
        get => (int)TimeGap.TotalMilliseconds;
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

    private bool SetProperty<T>(T value, ref T @field, [CallerMemberName] string propertyName = "")
    {
        bool notify = !Equals(value, @field);
        field = value;
        if (notify)
            OnPropertyChanged(propertyName);
        return notify;
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private class ReadOnly : IReadOnlyWaitOptions
    {
        private readonly IWaitOptions _options;

        public ReadOnly(IWaitOptions options)
        {
            _options = options;
        }

        public bool TraceExceptions => _options.TraceExceptions;
        public int? MaxAcceptedExceptions => _options.MaxAcceptedExceptions;
        public int? MaxRetryCount => _options.MaxRetryCount;
        public TimeSpan Timeout => _options.Timeout;
        public int TimeoutMs => _options.TimeoutMs;
        public TimeSpan TimeGap => _options.TimeGap;
        public int TimeGapMs => _options.TimeGapMs;
    }
}
