namespace Rocketcress.Core
{
    /// <summary>
    /// Wait operation that can be configured or started.
    /// </summary>
    /// <typeparam name="T">The type of result of the wait operation.</typeparam>
    public interface IWait<T> : IStartableWait<T>, IConfigurableWait<T, IWait<T>>
    {
        /// <summary>
        /// Configures what should happen when an exception occurs during this wait operation.
        /// </summary>
        /// <returns>Configuration object with which the exception handling can be configured.</returns>
        IWaitOnError<T> OnError();
    }
}
