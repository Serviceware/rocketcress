namespace Rocketcress.Core
{
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
        /// Defines whether the wait operation should throw an exception when it fails.
        /// This occures when it times out, is aborted or exceeds the maximum exception count.
        /// </summary>
        /// <param name="message">The message that will be shown in the exception.</param>
        /// <returns>The configured wait operation.</returns>
        TWait ThrowOnFailure(string? message);

        /// <summary>
        /// Defines the maximum amount of exception that are allowed during the wait operation.
        /// If set to <c>null</c>, the amount of exceptions is not limited.
        /// </summary>
        /// <param name="count">The maximum number of allowed exception.</param>
        /// <returns>The configured wait operation.</returns>
        TWait WithMaxExceptionCount(int? count);
    }
}
