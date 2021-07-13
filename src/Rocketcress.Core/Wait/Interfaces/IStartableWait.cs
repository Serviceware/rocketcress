namespace Rocketcress.Core
{
    /// <summary>
    /// Wait operation that can be started.
    /// </summary>
    /// <typeparam name="T">The type of result of the wait operation.</typeparam>
    public interface IStartableWait<T>
    {
        /// <summary>
        /// Starts the wait operation.
        /// </summary>
        /// <returns>The result of the wait operation.</returns>
        WaitResult<T> Start();
    }
}
