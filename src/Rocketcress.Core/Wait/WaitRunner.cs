using System.Threading;
using System.Threading.Tasks;

namespace Rocketcress.Core;

internal class WaitRunnerBase<T>
{
    private readonly Action<WaitContext> _onStartingCallback;
    private readonly Action<WaitContext> _onFinishedCallback;
    private readonly Action<WaitContext, Exception> _onExceptionCallback;

    public WaitRunnerBase(
        IWaitOptions options,
        string operationName,
        Action<WaitContext> onStartingCallback,
        Action<WaitContext> onFinishedCallback,
        Action<WaitContext, Exception> onExceptionCallback)
    {
        _onStartingCallback = onStartingCallback;
        _onFinishedCallback = onFinishedCallback;
        _onExceptionCallback = onExceptionCallback;

        Options = options;
        ThrowOnFailure = false;
        ErrorMessage = null;

        OperationName = operationName;
    }

    public IWaitOptions Options { get; }
    public bool ThrowOnFailure { get; set; }
    public string? ErrorMessage { get; set; }

    protected string OperationName { get; }

    protected WaitContext<T> Start()
    {
        return new WaitContext<T>(Options.AsReadOnly(), ThrowOnFailure, ErrorMessage);
    }

    protected void StartWait(WaitContext<T> ctx)
    {
        ctx.Start();
        _onStartingCallback(ctx);
    }

    protected void EndWait(WaitContext<T> ctx)
    {
        ctx.Stop();
        _onFinishedCallback(ctx);
    }

    protected WaitResult<T> End(WaitContext<T> ctx)
    {
        var result = ctx.GetResult();
        if (!result.Status.IsValueAvailable())
        {
            Logger.LogWarning(GetErrorMessage(result, true));
            if (ThrowOnFailure)
                throw new WaitFailedException(result, GetErrorMessage(result, false));
        }

        return result;
    }

    protected bool CheckCondition(WaitContext<T> ctx, T? resultValue)
    {
        if (!Equals(resultValue, default(T)))
        {
            ctx.Result.WithStatus(WaitResultStatus.ValueAvailable).WithValue(resultValue);
            return true;
        }

        return false;
    }

    protected string GetErrorMessage(WaitResult<T> result, bool ignoreCustomMessage)
    {
        if (!ignoreCustomMessage && ErrorMessage is not null)
            return ErrorMessage;
        return result.Status switch
        {
            WaitResultStatus.CallerAbortedWithoutValue => $"The caller aborted this {OperationName}.",
            WaitResultStatus.Timeout => $"The {OperationName} timed out after {Options.Timeout.TotalSeconds:0.###} seconds.",
            WaitResultStatus.TooManyRetries => $"The {OperationName} did not succeed after {Options.MaxRetryCount} retries.",
            WaitResultStatus.TooManyExceptions => $"{result.Exceptions.Length} exceptions occurred during the {OperationName}, which exceeds the maximum allowed number of exceptions of {Options.MaxAcceptedExceptions}.",
            _ => $"The {OperationName} failed due to an unknown event (Status = {result.Status})",
        };
    }

    protected bool HandleException(WaitContext<T> ctx, Exception exception)
    {
        ctx.Result.WithException(exception);
        if (exception is WaitAbortedException wae)
        {
            HandleWaitAbortedException(ctx, wae);
            return true;
        }

        if (Options.TraceExceptions)
            Logger.LogDebug("Exception while waiting: " + exception.ToString());

        try
        {
            _onExceptionCallback(ctx, exception);
        }
        catch (WaitAbortedException ex)
        {
            HandleWaitAbortedException(ctx, ex);
            return true;
        }
        catch (Exception ex)
        {
            Logger.LogWarning("Exception while calling event 'ExcpetionOccured': " + ex);
        }

        if (Options.MaxAcceptedExceptions.HasValue && ctx.Result.Exceptions.Count > Options.MaxAcceptedExceptions)
        {
            ctx.Result.WithStatus(WaitResultStatus.TooManyExceptions);
            return true;
        }

        return false;
    }

    private void HandleWaitAbortedException(WaitContext<T> ctx, WaitAbortedException exception)
    {
        Logger.LogInfo($"The {OperationName} was aborted{(string.IsNullOrWhiteSpace(exception.Message) ? "." : $": {exception.Message}")}");

        if (exception is WaitAbortedException<T> wae)
            ctx.Result.WithStatus(WaitResultStatus.CallerAbortedWithValue).WithValue(wae.Value);
        else
            ctx.Result.WithStatus(WaitResultStatus.CallerAbortedWithoutValue);
    }
}

internal sealed class WaitRunner<T> : WaitRunnerBase<T>
{
    private readonly List<Action<WaitContext<T>>> _precedeWithActions = new();
    private readonly List<Action<WaitContext<T>>> _continueWithActions = new();
    private readonly Func<WaitContext<T>, T?> _condition;

    public WaitRunner(
        Func<WaitContext<T>, T?> condition,
        IWaitOptions options,
        string operationName,
        Action<WaitContext> onStartingCallback,
        Action<WaitContext> onFinishedCallback,
        Action<WaitContext, Exception> onExceptionCallback)
        : base(options, operationName, onStartingCallback, onFinishedCallback, onExceptionCallback)
    {
        _condition = condition;
    }

    public Action<Exception>? ExceptionHandler { get; set; }

    public void PrecedeWith(Action<WaitContext<T>> action) => _precedeWithActions.Add(action);

    public void ContinueWith(Action<WaitContext<T>> action) => _continueWithActions.Add(action);

    public WaitResult<T> Run()
    {
        var ctx = Start();

        // Execute precede actions
        try
        {
            foreach (var action in _precedeWithActions)
                action(ctx);
        }
        catch (WaitAbortedException ex)
        {
            HandleException(ctx, ex);
        }

        // Wait/Retry loop
        if (!ctx.Result.Status.IsAbort())
        {
            StartWait(ctx);
            while (ctx.Next())
            {
                try
                {
                    try
                    {
                        if (CheckCondition(ctx, _condition(ctx)))
                            break;
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler?.Invoke(ex);
                        if (HandleException(ctx, ex))
                            break;
                    }
                }
                catch (WaitAbortedException ex)
                {
                    if (HandleException(ctx, ex))
                        break;
                }

                Thread.Sleep(ctx.Options.TimeGap);
            }

            EndWait(ctx);
        }

        // Execute precede actions
        if (!ctx.Result.Status.IsAbort())
        {
            try
            {
                foreach (var action in _continueWithActions)
                    action(ctx);
            }
            catch (WaitAbortedException ex)
            {
                HandleException(ctx, ex);
            }
        }

        return End(ctx);
    }
}

internal sealed class AsyncWaitRunner<T> : WaitRunnerBase<T>
{
    private readonly List<Func<WaitContext<T>, Task>> _precedeWithActions = new();
    private readonly List<Func<WaitContext<T>, Task>> _continueWithActions = new();
    private readonly Func<WaitContext<T>, Task<T?>> _condition;

    public AsyncWaitRunner(
        Func<WaitContext<T>, Task<T?>> condition,
        IWaitOptions options,
        string operationName,
        Action<WaitContext> onStartingCallback,
        Action<WaitContext> onFinishedCallback,
        Action<WaitContext, Exception> onExceptionCallback)
        : base(options, operationName, onStartingCallback, onFinishedCallback, onExceptionCallback)
    {
        _condition = condition;
    }

    public Func<Exception, Task>? ExceptionHandler { get; set; }

    public void PrecedeWith(Func<WaitContext<T>, Task> action) => _precedeWithActions.Add(action);

    public void ContinueWith(Func<WaitContext<T>, Task> action) => _continueWithActions.Add(action);

    public async Task<WaitResult<T>> RunAsync()
    {
        var ctx = Start();

        // Execute precede actions
        try
        {
            foreach (var action in _precedeWithActions)
                await action(ctx);
        }
        catch (WaitAbortedException ex)
        {
            HandleException(ctx, ex);
        }

        // Wait/Retry loop
        if (!ctx.Result.Status.IsAbort())
        {
            StartWait(ctx);
            while (ctx.Next())
            {
                try
                {
                    try
                    {
                        if (CheckCondition(ctx, await _condition(ctx)))
                            break;
                    }
                    catch (Exception ex)
                    {
                        if (ExceptionHandler != null)
                            await ExceptionHandler.Invoke(ex);
                        if (HandleException(ctx, ex))
                            break;
                    }
                }
                catch (WaitAbortedException ex)
                {
                    if (HandleException(ctx, ex))
                        break;
                }

                await Task.Delay(ctx.Options.TimeGap);
            }

            EndWait(ctx);
        }

        // Execute precede actions
        if (!ctx.Result.Status.IsAbort())
        {
            try
            {
                foreach (var action in _continueWithActions)
                    await action(ctx);
            }
            catch (WaitAbortedException ex)
            {
                HandleException(ctx, ex);
            }
        }

        return End(ctx);
    }
}
