using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rocketcress.Core
{
    internal sealed class AsyncWait<T> : IAsyncWait<T>, IAsyncWaitOnError<T>, IWaitDefaultOptions
    {
        private readonly WaitRunner<T> _runner;
        private readonly Func<int, Task<T?>> _condition;
        private Func<Exception, Task<(WaitRunnerErrorResult Result, T? Value)>>? _exceptionHandler;

        public IWaitOptions DefaultOptions { get; }

        internal AsyncWait(
            Func<int, Task<T?>> condition,
            IWaitOptions options,
            string name,
            Action<object?, IDictionary<string, object>> onStartingCallback,
            Action<object?, IDictionary<string, object>> onFinishedCallback,
            Action<object?, Exception> onExceptionCallback)
        {
            DefaultOptions = options;
            _runner = new WaitRunner<T>(
                (IWaitOptions)options.Clone(),
                name,
                x => onStartingCallback(this, x),
                x => onFinishedCallback(this, x),
                x => onExceptionCallback(this, x));
            _condition = condition;
        }

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
            return await _runner.RunAsync(_condition, _exceptionHandler);
        }
    }
}
