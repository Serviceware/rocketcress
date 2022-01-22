using System.Threading.Tasks;

namespace Rocketcress.Core;

internal sealed class AsyncWait<T> : IAsyncWait<T>, IAsyncWaitOnError<T>, IWaitDefaultOptions
{
    private readonly AsyncWaitRunner<T> _runner;

    internal AsyncWait(
        Func<WaitContext<T>, Task<T?>> condition,
        IWaitOptions options,
        string name,
        Action<object?, WaitContext> onStartingCallback,
        Action<object?, WaitContext> onFinishedCallback,
        Action<object?, WaitContext, Exception> onExceptionCallback)
    {
        DefaultOptions = options;
        _runner = new AsyncWaitRunner<T>(
            condition,
            (IWaitOptions)options.Clone(),
            name,
            x => onStartingCallback(this, x),
            x => onFinishedCallback(this, x),
            (x, y) => onExceptionCallback(this, x, y));
    }

    public IWaitOptions DefaultOptions { get; }

    #region IAsyncWait<T> Members

    public IAsyncWaitOnError<T> OnError()
    {
        return this;
    }

    public IAsyncWait<T> PrecedeWith(Func<WaitContext<T>, Task> action)
    {
        _runner.PrecedeWith(action);
        return this;
    }

    public IAsyncWait<T> ContinueWith(Func<WaitContext<T>, Task> action)
    {
        _runner.ContinueWith(action);
        return this;
    }

    #endregion

    #region IConfigurableWait<T, IAsyncWait<T>> Members

    public IAsyncWait<T> ThrowOnFailure(string? message)
    {
        _runner.ThrowOnFailure = true;
        _runner.ErrorMessage = message;
        return this;
    }

    public IAsyncWait<T> NotThrowOnFailure()
    {
        _runner.ThrowOnFailure = false;
        _runner.ErrorMessage = null;
        return this;
    }

    public IAsyncWait<T> WithMaxExceptionCount(int? count)
    {
        _runner.Options.MaxAcceptedExceptions = count;
        return this;
    }

    public IAsyncWait<T> WithTimeGap(TimeSpan timeGap)
    {
        _runner.Options.TimeGap = timeGap;
        return this;
    }

    public IAsyncWait<T> WithTimeout(TimeSpan timeout)
    {
        _runner.Options.Timeout = timeout;
        return this;
    }

    public IAsyncWait<T> WithMaxRetryCount(int? count)
    {
        _runner.Options.MaxRetryCount = count;
        return this;
    }

    public IAsyncWait<T> Configure(Action<IWaitOptions> configurationFunction)
    {
        configurationFunction(_runner.Options);
        return this;
    }

    #endregion

    #region IAsyncWaitOnError<T> Members

    public IAsyncWait<T> Abort()
    {
        _runner.ExceptionHandler = x => throw new WaitAbortedException();
        return this;
    }

    public IAsyncWait<T> Call(Func<Exception, Task> action)
    {
        _runner.ExceptionHandler = action;
        return this;
    }

    public IAsyncWait<T> Return(Func<Exception, Task<T?>> resultFactory)
    {
        _runner.ExceptionHandler = async x =>
        {
            var value = await resultFactory(x);
            if (!Equals(value, default))
                throw new WaitAbortedException<T>(value);
        };
        return this;
    }

    public IAsyncWait<T> Return(T? value)
    {
        _runner.ExceptionHandler = x =>
        {
            if (!Equals(value, default))
                throw new WaitAbortedException<T>(value);
            return Task.CompletedTask;
        };
        return this;
    }

    #endregion

    #region IAsyncStartableWait<T> Members

    public async Task<WaitResult<T>> StartAsync()
    {
        return await _runner.RunAsync();
    }

    #endregion
}
