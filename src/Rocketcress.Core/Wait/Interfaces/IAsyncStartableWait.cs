using System.Threading.Tasks;

namespace Rocketcress.Core;

/// <summary>
/// Asynchronous wait operation that can be started.
/// </summary>
/// <typeparam name="T">The type of result of the wait operation.</typeparam>
public interface IAsyncStartableWait<T>
{
    /// <summary>
    /// Starts the wait operation asynchronously.
    /// </summary>
    /// <returns>The result of the wait operation.</returns>
    Task<WaitResult<T>> StartAsync();
}
