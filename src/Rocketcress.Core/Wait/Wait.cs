using System;
using System.Collections.Generic;

namespace Rocketcress.Core
{
    internal sealed class Wait<T> : IWait<T>, IWaitOnError<T>, IWaitDefaultOptions
    {
        private readonly WaitRunner<T> _runner;
        private readonly Func<int, T?> _condition;
        private Func<Exception, (WaitRunnerErrorResult Result, T? Value)>? _exceptionHandler;

        public IWaitOptions DefaultOptions { get; }

        internal Wait(
            Func<int, T?> condition,
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
            return _runner.Run(_condition, _exceptionHandler);
        }
    }
}
