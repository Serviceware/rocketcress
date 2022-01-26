namespace Rocketcress.Core;

internal sealed class Wait<T> : IWait<T>, IWaitOnError<T>, IWaitDefaultOptions
{
    private readonly WaitRunner<T> _runner;

    internal Wait(
        Func<WaitContext<T>, T?> condition,
        IWaitOptions options,
        string name,
        Action<object?, WaitContext> onStartingCallback,
        Action<object?, WaitContext> onFinishedCallback,
        Action<object?, WaitContext, Exception> onExceptionCallback)
    {
        DefaultOptions = options;
        _runner = new WaitRunner<T>(
            condition,
            (IWaitOptions)options.Clone(),
            name,
            x => onStartingCallback(this, x),
            x => onFinishedCallback(this, x),
            (x, y) => onExceptionCallback(this, x, y));
    }

    public IWaitOptions DefaultOptions { get; }

    #region IWait<T> Members

    public IWaitOnError<T> OnError()
    {
        return this;
    }

    #endregion

    #region IConfigurableWait<T, IWait<T>> Members

    public IWait<T> ThrowOnFailure(string? message)
    {
        _runner.ThrowOnFailure = true;
        _runner.ErrorMessage = message;
        return this;
    }

    public IWait<T> NotThrowOnFailure()
    {
        _runner.ThrowOnFailure = false;
        _runner.ErrorMessage = null;
        return this;
    }

    public IWait<T> WithDefaultErrorMessage(string? message)
    {
        _runner.DefaultErrorMessage = message;
        return this;
    }

    public IWait<T> WithMaxExceptionCount(int? count)
    {
        _runner.Options.MaxAcceptedExceptions = count;
        return this;
    }

    public IWait<T> WithTimeGap(TimeSpan timeGap)
    {
        _runner.Options.TimeGap = timeGap;
        return this;
    }

    public IWait<T> WithTimeout(TimeSpan timeout)
    {
        _runner.Options.Timeout = timeout;
        return this;
    }

    public IWait<T> WithMaxRetryCount(int? count)
    {
        _runner.Options.MaxRetryCount = count;
        return this;
    }

    public IWait<T> Configure(Action<IWaitOptions> configurationFunction)
    {
        configurationFunction(_runner.Options);
        return this;
    }

    public IWait<T> PrecedeWith(Action<WaitContext<T>> action)
    {
        _runner.PrecedeWith(action);
        return this;
    }

    public IWait<T> ContinueWith(Action<WaitContext<T>> action)
    {
        _runner.ContinueWith(action);
        return this;
    }

    #endregion

    #region IStartableWait<T> Members

    public IWait<T> Abort()
    {
        _runner.ExceptionHandler = x => throw new WaitAbortedException();
        return this;
    }

    public IWait<T> Call(Action<Exception> action)
    {
        _runner.ExceptionHandler = action;
        return this;
    }

    public IWait<T> Return(Func<Exception, T?> resultFactory)
    {
        _runner.ExceptionHandler = x =>
        {
            var value = resultFactory(x);
            if (!Equals(value, default))
                throw new WaitAbortedException<T>(value);
        };
        return this;
    }

    public IWait<T> Return(T? value)
    {
        _runner.ExceptionHandler = x =>
        {
            if (!Equals(value, default))
                throw new WaitAbortedException<T>(value);
        };
        return this;
    }

    #endregion

    #region IStartableWait<T> Members

    public WaitResult<T> Start()
    {
        return _runner.Run();
    }

    #endregion
}
