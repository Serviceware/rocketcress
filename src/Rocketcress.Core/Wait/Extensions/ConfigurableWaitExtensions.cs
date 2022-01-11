namespace Rocketcress.Core
{
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
            var timeout = timeoutMilliseconds.HasValue
                ? TimeSpan.FromMilliseconds(timeoutMilliseconds.Value)
                : Wait.Options.DefaultTimeout;
            return wait.WithTimeout(timeout);
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
            var timeGap = timeGapMilliseconds.HasValue
                ? TimeSpan.FromMilliseconds(timeGapMilliseconds.Value)
                : Wait.Options.DefaultTimeGap;
            return wait.WithTimeGap(timeGap);
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
            if (@throw)
                return wait.ThrowOnFailure(null);
            return (TWait)wait;
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
            if (@throw)
                return wait.ThrowOnFailure(message);
            return (TWait)wait;
        }
    }
}
