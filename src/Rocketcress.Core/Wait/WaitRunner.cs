using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Rocketcress.Core
{
    internal sealed class WaitRunner<T>
    {
        private readonly Stopwatch _stopwatch;
        private readonly IDictionary<string, object> _data;
        private readonly List<Exception> _exceptions;

        public TimeSpan Timeout { get; set; }
        public TimeSpan TimeGap { get; set; }
        public int? MaxExceptionCount { get; set; }
        public bool ThrowOnFailure { get; set; }
        public string? ErrorMessage { get; set; }

        public WaitRunner()
        {
            _stopwatch = new Stopwatch();
            _data = new Dictionary<string, object>();
            _exceptions = new List<Exception>();

            Timeout = Wait.Options.DefaultTimeout;
            TimeGap = Wait.Options.DefaultTimeGap;
            MaxExceptionCount = Wait.Options.MaxAcceptedExceptions;
            ThrowOnFailure = false;
            ErrorMessage = null;
        }

        public WaitResult<T> Run(object waitObj, Func<T?> condition, Func<Exception, (WaitRunnerErrorResult Result, T? Value)>? exceptionHandler)
        {
            Start(waitObj);
            try
            {
                while (_stopwatch.Elapsed < Timeout)
                {
                    try
                    {
                        if (TryCreateResult(WaitResult.ValueAvailable, condition(), out var result))
                            return result;
                    }
                    catch (Exception ex)
                    {
                        var hr = exceptionHandler?.Invoke(ex);
                        if (HandleException(waitObj, hr, ex, out var result))
                            return result;
                    }

                    Thread.Sleep(TimeGap);
                }
            }
            finally
            {
                End(waitObj);
            }

            return OnError(WaitResult.Timeout);
        }

        public async Task<WaitResult<T>> RunAsync(object waitObj, Func<Task<T?>> condition, Func<Exception, Task<(WaitRunnerErrorResult Result, T? Value)>>? exceptionHandler)
        {
            Start(waitObj);
            try
            {
                while (_stopwatch.Elapsed < Timeout)
                {
                    try
                    {
                        if (TryCreateResult(WaitResult.ValueAvailable, await condition(), out var result))
                            return result;
                    }
                    catch (Exception ex)
                    {
                        var hr = exceptionHandler == null ? ((WaitRunnerErrorResult, T?)?)null : await exceptionHandler(ex);
                        if (HandleException(waitObj, hr, ex, out var result))
                            return result;
                    }

                    await Task.Delay(TimeGap);
                }
            }
            finally
            {
                End(waitObj);
            }

            return OnError(WaitResult.Timeout);
        }

        private void Start(object waitObj)
        {
            Wait.OnStarting(waitObj, _data);
            _exceptions.Clear();
            _stopwatch.Start();
        }

        private void End(object waitObj)
        {
            _stopwatch.Stop();
            Wait.OnFinished(waitObj, _data);
        }

        private bool TryCreateResult(WaitResult resultStatus, T? resultValue, [NotNullWhen(true)] out WaitResult<T>? result)
        {
            if (!Equals(resultValue, default(T)))
            {
                result = new WaitResult<T>(resultStatus, resultValue, _stopwatch.Elapsed, _exceptions.ToArray());
                return true;
            }

            result = null;
            return false;
        }

        private WaitResult<T> OnError(WaitResult status)
        {
            Logger.LogWarning(GetErrorMessage(status, true));
            if (ThrowOnFailure)
                AssertEx.Instance.Fail(GetErrorMessage(status, false));
            return new WaitResult<T>(status, default, _stopwatch.Elapsed, _exceptions.ToArray());
        }

        private string GetErrorMessage(WaitResult status, bool ignoreCustomMessage)
        {
            if (!ignoreCustomMessage && ErrorMessage is not null)
                return ErrorMessage;
            return status switch
            {
                WaitResult.CallerAborted => $"The caller aborted this wait operation due to an exception.",
                WaitResult.TooManyExceptions => $"{_exceptions.Count} exceptions occurred during the wait operation, which exceeds the maximum allowed number of exceptions of {MaxExceptionCount}.",
                _ => $"The wait operation timed out after {Timeout.TotalSeconds:0.###} seconds.",
            };
        }

        private bool HandleException(object waitObj, (WaitRunnerErrorResult Result, T? Value)? handlerResult, Exception exception, [NotNullWhen(true)] out WaitResult<T>? result)
        {
            _exceptions.Add(exception);
            if (Wait.Options.TraceExceptions)
                Logger.LogDebug("Exception while waiting: " + exception.ToString());

            try
            {
                Wait.OnExceptionOccurred(waitObj, exception);
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

            if (MaxExceptionCount.HasValue && _exceptions.Count > MaxExceptionCount)
            {
                result = OnError(WaitResult.TooManyExceptions);
                return true;
            }

            result = null;
            return false;
        }
    }

    internal enum WaitRunnerErrorResult
    {
        None,
        Return,
        Abort,
    }
}
