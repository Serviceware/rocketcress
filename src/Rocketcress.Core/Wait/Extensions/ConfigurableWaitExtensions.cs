namespace Rocketcress.Core;

/// <summary>
/// Provdes extension methods for the <see cref="IConfigurableWait{TResult, TWait}"/> interface.
/// </summary>
public static class ConfigurableWaitExtensions
{
    /// <summary>
    /// Defines the timeout for this wait operation.
    /// </summary>
    /// <typeparam name="TResult">The type of result of the wait operation.</typeparam>
    /// <typeparam name="TWait">The type of wait operation.</typeparam>
    /// <param name="wait">The configurable wait operation instance.</param>
    /// <param name="timeout">The timeout to use.</param>
    /// <returns>The configured wait operation.</returns>
    public static TWait WithTimeout<TResult, TWait>(this IConfigurableWait<TResult, TWait> wait, TimeSpan? timeout)
        where TWait : IConfigurableWait<TResult, TWait>
    {
        return timeout.HasValue
            ? wait.WithTimeout(timeout.Value)
            : wait.WithDefaultTimeout();
    }

    /// <summary>
    /// Defines the timeout for this wait operation.
    /// </summary>
    /// <typeparam name="TResult">The type of result of the wait operation.</typeparam>
    /// <typeparam name="TWait">The type of wait operation.</typeparam>
    /// <param name="wait">The configurable wait operation instance.</param>
    /// <param name="timeoutMilliseconds">The timeout in milliseconds to use.</param>
    /// <returns>The configured wait operation.</returns>
    public static TWait WithTimeout<TResult, TWait>(this IConfigurableWait<TResult, TWait> wait, int timeoutMilliseconds)
        where TWait : IConfigurableWait<TResult, TWait>
    {
        return wait.WithTimeout(TimeSpan.FromMilliseconds(timeoutMilliseconds));
    }

    /// <summary>
    /// Defines the timeout for this wait operation.
    /// </summary>
    /// <typeparam name="TResult">The type of result of the wait operation.</typeparam>
    /// <typeparam name="TWait">The type of wait operation.</typeparam>
    /// <param name="wait">The configurable wait operation instance.</param>
    /// <param name="timeoutMilliseconds">The timeout in milliseconds to use. If <c>null</c> the default timeout is used.</param>
    /// <returns>The configured wait operation.</returns>
    public static TWait WithTimeout<TResult, TWait>(this IConfigurableWait<TResult, TWait> wait, int? timeoutMilliseconds)
        where TWait : IConfigurableWait<TResult, TWait>
    {
        return timeoutMilliseconds.HasValue
            ? wait.WithTimeout(TimeSpan.FromMilliseconds(timeoutMilliseconds.Value))
            : wait.WithDefaultTimeout();
    }

    /// <summary>
    /// Defines the timeout for this wait operation as the default timeout.
    /// </summary>
    /// <typeparam name="TResult">The type of result of the wait operation.</typeparam>
    /// <typeparam name="TWait">The type of wait operation.</typeparam>
    /// <param name="wait">The configurable wait operation instance.</param>
    /// <returns>The configured wait operation.</returns>
    public static TWait WithDefaultTimeout<TResult, TWait>(this IConfigurableWait<TResult, TWait> wait)
        where TWait : IConfigurableWait<TResult, TWait>
    {
        var options = wait is IWaitDefaultOptions wdo ? wdo.DefaultOptions : Wait.DefaultOptions;
        return wait.WithTimeout(options.Timeout);
    }

    /// <summary>
    /// Defines the time to wait between checking the condition for this wait operation.
    /// </summary>
    /// <typeparam name="TResult">The type of result of the wait operation.</typeparam>
    /// <typeparam name="TWait">The type of wait operation.</typeparam>
    /// <param name="wait">The configurable wait operation instance.</param>
    /// <param name="timeGap">The time gap to use.</param>
    /// <returns>The configured wait operation.</returns>
    public static TWait WithTimeGap<TResult, TWait>(this IConfigurableWait<TResult, TWait> wait, TimeSpan? timeGap)
        where TWait : IConfigurableWait<TResult, TWait>
    {
        return timeGap.HasValue
            ? wait.WithTimeGap(timeGap.Value)
            : wait.WithDefaultTimeGap();
    }

    /// <summary>
    /// Defines the time to wait between checking the condition for this wait operation.
    /// </summary>
    /// <typeparam name="TResult">The type of result of the wait operation.</typeparam>
    /// <typeparam name="TWait">The type of wait operation.</typeparam>
    /// <param name="wait">The configurable wait operation instance.</param>
    /// <param name="timeGapMilliseconds">The time gap in milliseconds to use.</param>
    /// <returns>The configured wait operation.</returns>
    public static TWait WithTimeGap<TResult, TWait>(this IConfigurableWait<TResult, TWait> wait, int timeGapMilliseconds)
        where TWait : IConfigurableWait<TResult, TWait>
    {
        return wait.WithTimeGap(TimeSpan.FromMilliseconds(timeGapMilliseconds));
    }

    /// <summary>
    /// Defines the time to wait between checking the condition for this wait operation.
    /// </summary>
    /// <typeparam name="TResult">The type of result of the wait operation.</typeparam>
    /// <typeparam name="TWait">The type of wait operation.</typeparam>
    /// <param name="wait">The configurable wait operation instance.</param>
    /// <param name="timeGapMilliseconds">The time gap in milliseconds to use. If <c>null</c> the default time gap is used.</param>
    /// <returns>The configured wait operation.</returns>
    public static TWait WithTimeGap<TResult, TWait>(this IConfigurableWait<TResult, TWait> wait, int? timeGapMilliseconds)
        where TWait : IConfigurableWait<TResult, TWait>
    {
        return timeGapMilliseconds.HasValue
            ? wait.WithTimeGap(TimeSpan.FromMilliseconds(timeGapMilliseconds.Value))
            : wait.WithDefaultTimeGap();
    }

    /// <summary>
    /// Defines the time to wait between checking the condition for this wait operation as the default.
    /// </summary>
    /// <typeparam name="TResult">The type of result of the wait operation.</typeparam>
    /// <typeparam name="TWait">The type of wait operation.</typeparam>
    /// <param name="wait">The configurable wait operation instance.</param>
    /// <returns>The configured wait operation.</returns>
    public static TWait WithDefaultTimeGap<TResult, TWait>(this IConfigurableWait<TResult, TWait> wait)
        where TWait : IConfigurableWait<TResult, TWait>
    {
        var options = wait is IWaitDefaultOptions wdo ? wdo.DefaultOptions : Wait.DefaultOptions;
        return wait.WithTimeGap(options.TimeGap);
    }

    /// <summary>
    /// Defines whether the wait operation should throw an exception when it fails.
    /// This occures when it times out, is aborted or exceeds the maximum exception count.
    /// </summary>
    /// <typeparam name="TResult">The type of result of the wait operation.</typeparam>
    /// <typeparam name="TWait">The type of wait operation.</typeparam>
    /// <param name="wait">The configurable wait operation instance.</param>
    /// <returns>The configured wait operation.</returns>
    public static TWait ThrowOnFailure<TResult, TWait>(this IConfigurableWait<TResult, TWait> wait)
        where TWait : IConfigurableWait<TResult, TWait>
    {
        return wait.ThrowOnFailure(null);
    }

    /// <summary>
    /// Defines whether the wait operation should throw an exception when it fails.
    /// This occures when it times out, is aborted or exceeds the maximum exception count.
    /// </summary>
    /// <typeparam name="TResult">The type of result of the wait operation.</typeparam>
    /// <typeparam name="TWait">The type of wait operation.</typeparam>
    /// <param name="wait">The configurable wait operation instance.</param>
    /// <param name="throw">Determines whether an exception should be thrown.</param>
    /// <returns>The configured wait operation.</returns>
    public static TWait OnFailure<TResult, TWait>(this IConfigurableWait<TResult, TWait> wait, bool @throw)
        where TWait : IConfigurableWait<TResult, TWait>
    {
        return @throw ? wait.ThrowOnFailure(null) : wait.NotThrowOnFailure();
    }

    /// <summary>
    /// Defines whether the wait operation should throw an exception when it fails.
    /// This occures when it times out, is aborted or exceeds the maximum exception count.
    /// </summary>
    /// <typeparam name="TResult">The type of result of the wait operation.</typeparam>
    /// <typeparam name="TWait">The type of wait operation.</typeparam>
    /// <param name="wait">The configurable wait operation instance.</param>
    /// <param name="throw">Determines whether an exception should be thrown.</param>
    /// <param name="message">The message that will be shown in the exception.</param>
    /// <returns>The configured wait operation.</returns>
    public static TWait OnFailure<TResult, TWait>(this IConfigurableWait<TResult, TWait> wait, bool @throw, string? message)
        where TWait : IConfigurableWait<TResult, TWait>
    {
        return @throw ? wait.ThrowOnFailure(message) : wait.NotThrowOnFailure();
    }

    /// <summary>
    /// Defines the maximum amount of exception that are allowed during the wait operation as the default.
    /// </summary>
    /// <typeparam name="TResult">The type of result of the wait operation.</typeparam>
    /// <typeparam name="TWait">The type of wait operation.</typeparam>
    /// <param name="wait">The configurable wait operation instance.</param>
    /// <returns>The configured wait operation.</returns>
    public static TWait WithDefaultMaxExceptionCount<TResult, TWait>(this IConfigurableWait<TResult, TWait> wait)
        where TWait : IConfigurableWait<TResult, TWait>
    {
        var options = wait is IWaitDefaultOptions wdo ? wdo.DefaultOptions : Wait.DefaultOptions;
        return wait.WithMaxExceptionCount(options.MaxAcceptedExceptions);
    }

    /// <summary>
    /// Defines the maximum number of time the condition should be retried during the wait operation as the default.
    /// </summary>
    /// <typeparam name="TResult">The type of result of the wait operation.</typeparam>
    /// <typeparam name="TWait">The type of wait operation.</typeparam>
    /// <param name="wait">The configurable wait operation instance.</param>
    /// <returns>The configured wait operation.</returns>
    public static TWait WithDefaultMaxRetryCount<TResult, TWait>(this IConfigurableWait<TResult, TWait> wait)
        where TWait : IConfigurableWait<TResult, TWait>
    {
        var options = wait is IWaitDefaultOptions wdo ? wdo.DefaultOptions : Wait.DefaultOptions;
        return wait.WithMaxRetryCount(options.MaxRetryCount);
    }
}
