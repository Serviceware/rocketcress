using System.Threading.Tasks;

namespace Rocketcress.Core;

/// <summary>
/// Represents a base class for a wait operation entry point.
/// </summary>
public abstract class WaitEntry : IWaitEntry
{
    /// <inheritdoc/>
    public IWait<T> Until<T>(Func<T?> condition)
    {
        return Wait.Until(condition);
    }

    /// <inheritdoc/>
    public IWait<T> Until<T>(Func<WaitContext<T>, T?> condition)
    {
        return Wait.Until(condition);
    }

    /// <inheritdoc/>
    public IAsyncWait<T> Until<T>(Func<Task<T?>> condition)
    {
        return Wait.Until(condition);
    }

    /// <inheritdoc/>
    public IAsyncWait<T> Until<T>(Func<WaitContext<T>, Task<T?>> condition)
    {
        return Wait.Until(condition);
    }
}