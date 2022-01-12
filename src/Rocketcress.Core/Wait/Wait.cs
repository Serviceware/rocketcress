namespace Rocketcress.Core;

internal sealed class Wait<T> : IWait<T>, IWaitOnError<T>
{
    private readonly WaitRunner<T> _runner;
    private readonly Func<T?> _condition;
    private Func<Exception, (WaitRunnerErrorResult Result, T? Value)>? _exceptionHandler;

    internal Wait(Func<T?> condition)
    {
        _runner = new WaitRunner<T>();
        _condition = condition;
    }

    public IWait<T> ThrowOnFailure(string? message)
    {
        _runner.ThrowOnFailure = true;
        _runner.ErrorMessage = message;
        return this;
    }

    public IWait<T> WithMaxExceptionCount(int? count)
    {
        _runner.MaxExceptionCount = count;
        return this;
    }

    public IWait<T> WithTimeGap(TimeSpan timeGap)
    {
        _runner.TimeGap = timeGap;
        return this;
    }

    public IWait<T> WithTimeout(TimeSpan timeout)
    {
        _runner.Timeout = timeout;
        return this;
    }

    public IWaitOnError<T> OnError()
    {
        return this;
    }

    public IWait<T> Abort()
    {
        _exceptionHandler = x => (WaitRunnerErrorResult.Abort, default);
        return this;
    }

    public IWait<T> Call(Action<Exception> action)
    {
        _exceptionHandler = x =>
        {
            action(x);
            return (WaitRunnerErrorResult.None, default);
        };
        return this;
    }

    public IWait<T> Return(Func<Exception, T?> resultFactory)
    {
        _exceptionHandler = x => (WaitRunnerErrorResult.Return, resultFactory(x));
        return this;
    }

    public IWait<T> Return(T? value)
    {
        _exceptionHandler = x => (WaitRunnerErrorResult.Return, value);
        return this;
    }

    public WaitResult<T> Start()
    {
        return _runner.Run(this, _condition, _exceptionHandler);
    }
}
