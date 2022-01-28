namespace Rocketcress.Core;

/// <summary>
/// Wait operation that can be configured.
/// </summary>
/// <typeparam name="TResult">The type of result of the wait operation.</typeparam>
/// <typeparam name="TWait">The type of wait operation.</typeparam>
public interface IConfigurableWait<TResult, TWait>
    where TWait : IConfigurableWait<TResult, TWait>
{
    /// <summary>
    /// Defines the timeout for this wait operation.
    /// </summary>
    /// <param name="timeout">The timeout to use.</param>
    /// <returns>The configured wait operation.</returns>
    TWait WithTimeout(TimeSpan timeout);

    /// <summary>
    /// Defines the time to wait between checking the condition for this wait operation.
    /// </summary>
    /// <param name="timeGap">The time gap to use.</param>
    /// <returns>The configured wait operation.</returns>
    TWait WithTimeGap(TimeSpan timeGap);

    /// <summary>
    /// Defines that the wait operation should throw an exception when it fails.
    /// This occurs when it times out, is aborted or exceeds the maximum exception count.
    /// </summary>
    /// <param name="message">The message that will be shown in the exception.</param>
    /// <returns>The configured wait operation.</returns>
    TWait ThrowOnFailure(string? message);

    /// <summary>
    /// Defines that the wait operation should not throw an exception when it fails.
    /// This occurs when it times out, is aborted or exceeds the maximum exception count.
    /// </summary>
    /// <returns>The configured wait operation.</returns>
    TWait NotThrowOnFailure();

    /// <summary>
    /// Defines the default error message that should be used when the wait operation is configured to throw an error on failure.
    /// </summary>
    /// <param name="message">The message to use.</param>
    /// <returns>The configured wait operation.</returns>
    TWait WithDefaultErrorMessage(string? message);

    /// <summary>
    /// Defines the maximum amount of exception that are allowed during the wait operation.
    /// If set to <c>null</c>, the amount of exceptions is not limited.
    /// </summary>
    /// <param name="count">The maximum number of allowed exception.</param>
    /// <returns>The configured wait operation.</returns>
    TWait WithMaxExceptionCount(int? count);

    /// <summary>
    /// Defines the maximum number of time the condition should be retried during the wait operation.
    /// If set to <c>null</c>, the condition is retried infinitely (until the timeout occurs).
    /// </summary>
    /// <param name="count">The maximum number of retries.</param>
    /// <returns>The configured wait operation.</returns>
    TWait WithMaxRetryCount(int? count);

    /// <summary>
    /// Configures the wait operation.
    /// </summary>
    /// <param name="configurationFunction">The function that is used to configure the wait operation.</param>
    /// <returns>The configured wait operation.</returns>
    TWait Configure(Action<IWaitOptions> configurationFunction);

    /// <summary>
    /// Precedes the wait operation with a specified action.
    /// </summary>
    /// <param name="precededAction">The preceded action.</param>
    /// <returns>The configured wait operation.</returns>
    TWait PrecedeWith(Action<WaitContext<TResult>> precededAction);

    /// <summary>
    /// Continues the wait operation with a specified action.
    /// </summary>
    /// <param name="continuedAction">The continued action.</param>
    /// <returns>The configured wait operation.</returns>
    TWait ContinueWith(Action<WaitContext<TResult>> continuedAction);
}