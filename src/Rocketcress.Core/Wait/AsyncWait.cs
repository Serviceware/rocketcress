using System.Threading.Tasks;

namespace Rocketcress.Core;

internal sealed class AsyncWait<T> : IAsyncWait<T>, IAsyncWaitOnError<T>
{
    private readonly WaitRunner<T> _runner;
    private readonly Func<Task<T?>> _condition;
    private Func<Exception, Task<(WaitRunnerErrorResult Result, T? Value)>>? _exceptionHandler;

    internal AsyncWait(Func<Task<T?>> condition)
    {
        _runner = new WaitRunner<T>();
        _condition = condition;
    }

    public IAsyncWait<T> ThrowOnFailure(string? message)
    {
        _runner.ThrowOnFailure = true;
        _runner.ErrorMessage = message;
        return this;
    }

    public IAsyncWait<T> WithMaxExceptionCount(int? count)
    {
        _runner.MaxExceptionCount = count;
        return this;
    }

    public IAsyncWait<T> WithTimeGap(TimeSpan timeGap)
    {
        _runner.TimeGap = timeGap;
        return this;
    }

    public IAsyncWait<T> WithTimeout(TimeSpan timeout)
    {
        _runner.Timeout = timeout;
        return this;
    }

    public IAsyncWaitOnError<T> OnError()
    {
        return this;
    }

    public IAsyncWait<T> Abort()
    {
        _exceptionHandler = x => Task.FromResult((WaitRunnerErrorResult.Abort, default(T)));
        return this;
    }

    public IAsyncWait<T> Call(Func<Exception, Task> action)
    {
        _exceptionHandler = async x =>
        {
            await action(x);
            return (WaitRunnerErrorResult.None, default(T));
        };
        return this;
    }

    public IAsyncWait<T> Return(Func<Exception, Task<T?>> resultFactory)
    {
        _exceptionHandler = async x => (WaitRunnerErrorResult.Return, await resultFactory(x));
        return this;
    }

    public IAsyncWait<T> Return(T? value)
    {
        _exceptionHandler = x => Task.FromResult((WaitRunnerErrorResult.Return, value));
        return this;
    }

    public async Task<WaitResult<T>> StartAsync()
    {
        return await _runner.RunAsync(this, _condition, _exceptionHandler);
    }
}
