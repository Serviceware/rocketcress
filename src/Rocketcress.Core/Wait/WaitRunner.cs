using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

namespace Rocketcress.Core
{
    internal class WaitRunnerBase<T>
    {
        private readonly Stopwatch _stopwatch;
        private readonly string _operationName;
        private readonly Action<IDictionary<string, object>> _onStartingCallback;
        private readonly Action<IDictionary<string, object>> _onFinishedCallback;
        private readonly Action<Exception> _onExceptionCallback;

        protected Stopwatch Stopwatch => _stopwatch;
        protected string OperationName => _operationName;

        public IWaitOptions Options { get; }
        public bool ThrowOnFailure { get; set; }
        public string? ErrorMessage { get; set; }

        public WaitRunnerBase(
            IWaitOptions options,
            string operationName,
            Action<IDictionary<string, object>> onStartingCallback,
            Action<IDictionary<string, object>> onFinishedCallback,
            Action<Exception> onExceptionCallback)
        {
            _stopwatch = new Stopwatch();
            _data = new Dictionary<string, object>();
            _exceptions = new List<Exception>();
            _operationName = operationName;

            _onStartingCallback = onStartingCallback;
            _onFinishedCallback = onFinishedCallback;
            _onExceptionCallback = onExceptionCallback;

            Options = options;
            ThrowOnFailure = false;
            ErrorMessage = null;
        }

        protected WaitContext<T> Start()
        {
            var ctx = new WaitContext<T>(Options, )
            _onStartingCallback(_data);
            _exceptions.Clear();
            _stopwatch.Start();
            return new WaitContext<T>
        }

        protected void End()
        {
            _stopwatch.Stop();
            _onFinishedCallback(_data);
        }

        protected bool TryCreateResult(WaitResult resultStatus, T? resultValue, [NotNullWhen(true)] out WaitResult<T>? result)
        {
            if (!Equals(resultValue, default(T)))
            {
                result = new WaitResult<T>(resultStatus, resultValue, _stopwatch.Elapsed, _exceptions.ToArray());
                return true;
            }

            result = null;
            return false;
        }

        protected WaitResult<T> OnError(WaitResult status)
        {
            Logger.LogWarning(GetErrorMessage(status, true));
            if (ThrowOnFailure)
                AssertEx.Instance.Fail(GetErrorMessage(status, false));
            return new WaitResult<T>(status, default, _stopwatch.Elapsed, _exceptions.ToArray());
        }

        protected string GetErrorMessage(WaitResult status, bool ignoreCustomMessage)
        {
            if (!ignoreCustomMessage && ErrorMessage is not null)
                return ErrorMessage;
            return status switch
            {
                WaitResult.CallerAborted => $"The caller aborted this {_operationName} due to an exception.",
                WaitResult.TooManyExceptions => $"{_exceptions.Count} exceptions occurred during the {_operationName}, which exceeds the maximum allowed number of exceptions of {Options.MaxAcceptedExceptions}.",
                WaitResult.TooManyRetries => $"The {_operationName} did not succeed after {Options.MaxRetryCount} retries.",
                _ => $"The {_operationName} timed out after {Options.Timeout.TotalSeconds:0.###} seconds.",
            };
        }

        protected bool HandleException((WaitRunnerErrorResult Result, T? Value)? handlerResult, Exception exception, [NotNullWhen(true)] out WaitResult<T>? result)
        {
            _exceptions.Add(exception);
            if (Options.TraceExceptions)
                Logger.LogDebug("Exception while waiting: " + exception.ToString());

            try
            {
                _onExceptionCallback(exception);
            }
            catch (AbortWaitException ex)
            {
                Logger.LogInfo($"The {_operationName} was aborted while calling event 'ExcpetionOccured': {ex}");
                result = OnError(WaitResult.CallerAborted);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Exception while calling event 'ExcpetionOccured': " + ex);
            }

            if (handlerResult.HasValue)
            {
                if (handlerResult.Value.Result == WaitRunnerErrorResult.Return)
                {
                    return TryCreateResult(WaitResult.CallerResult, handlerResult.Value.Value, out result);
                }
                else if (handlerResult.Value.Result == WaitRunnerErrorResult.Abort)
                {
                    result = OnError(WaitResult.CallerAborted);
                    return true;
                }
            }

            if (Options.MaxAcceptedExceptions.HasValue && _exceptions.Count > Options.MaxAcceptedExceptions)
            {
                result = OnError(WaitResult.TooManyExceptions);
                return true;
            }

            result = null;
            return false;
        }

        protected bool HandleLoopRun(int currentRetryCount, [NotNullWhen(true)] out WaitResult<T>? result)
        {
            if (Options.MaxRetryCount.HasValue && currentRetryCount + 1 > Options.MaxRetryCount)
            {
                result = OnError(WaitResult.TooManyRetries);
                return true;
            }

            result = null;
            return false;
        }
    }

    internal sealed class WaitRunner<T> : WaitRunnerBase<T>
    {
        private readonly List<Func<T?, T?>> _precedeWithActions = new();
        private readonly List<Func<WaitResult<T>, T?>> _continueWithActions = new();

        public WaitRunner(IWaitOptions options, string operationName, Action<IDictionary<string, object>> onStartingCallback, Action<IDictionary<string, object>> onFinishedCallback, Action<Exception> onExceptionCallback)
            : base(options, operationName, onStartingCallback, onFinishedCallback, onExceptionCallback)
        {
        }

        public void PrecedeWith(Func<T?, T?> action) => _precedeWithActions.Add(action);

        public void ContinueWith(Func<WaitResult<T>, T?> action) => _continueWithActions.Add(action);

        public WaitResult<T> Run(Func<int, T?> condition, Func<Exception, (WaitRunnerErrorResult Result, T? Value)>? exceptionHandler)
        {
            Start();
            try
            {
                int count = 0;
                while (Stopwatch.Elapsed < Options.Timeout)
                {
                    try
                    {
                        try
                        {
                            if (TryCreateResult(WaitResult.ValueAvailable, condition(count), out var result))
                                return result;
                        }
                        catch (Exception ex)
                        {
                            var hr = exceptionHandler?.Invoke(ex);
                            if (HandleException(hr, ex, out var result))
                                return result;
                        }
                    }
                    catch (AbortWaitException ex)
                    {
                        Logger.LogInfo($"The {OperationName} was aborted: {ex}");
                        return OnError(WaitResult.CallerAborted);
                    }

                    if (HandleLoopRun(count, out var result2))
                        return result2;

                    Thread.Sleep(Options.TimeGap);
                }
            }
            finally
            {
                End();
            }

            return OnError(WaitResult.Timeout);
        }
    }

    internal sealed class AsyncWaitRunner<T> : WaitRunnerBase<T>
    {
        public AsyncWaitRunner(IWaitOptions options, string operationName, Action<IDictionary<string, object>> onStartingCallback, Action<IDictionary<string, object>> onFinishedCallback, Action<Exception> onExceptionCallback)
            : base(options, operationName, onStartingCallback, onFinishedCallback, onExceptionCallback)
        {
        }

        public async Task<WaitResult<T>> RunAsync(Func<int, Task<T?>> condition, Func<Exception, Task<(WaitRunnerErrorResult Result, T? Value)>>? exceptionHandler)
        {
            Start();
            try
            {
                int count = 0;
                while (Stopwatch.Elapsed < Options.Timeout)
                {
                    try
                    {
                        try
                        {
                            if (TryCreateResult(WaitResult.ValueAvailable, await condition(count), out var result))
                                return result;
                        }
                        catch (Exception ex)
                        {
                            var hr = exceptionHandler == null ? ((WaitRunnerErrorResult, T?)?)null : await exceptionHandler(ex);
                            if (HandleException(hr, ex, out var result))
                                return result;
                        }
                    }
                    catch (AbortWaitException ex)
                    {
                        Logger.LogInfo($"The {OperationName} was aborted: {ex}");
                        return OnError(WaitResult.CallerAborted);
                    }

                    if (HandleLoopRun(count, out var result2))
                        return result2;

                    await Task.Delay(Options.TimeGap);
                }
            }
            finally
            {
                End();
            }

            return OnError(WaitResult.Timeout);
        }
    }

    internal enum WaitRunnerErrorResult
    {
        None,
        Return,
        Abort,
    }
}
