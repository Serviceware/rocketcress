using System.Threading.Tasks;

namespace Rocketcress.Core;

/// <summary>
/// Represents a base class for a wait operation entry point.
/// </summary>
public abstract class WaitEntry : IWaitEntry
{
    /// <inheritdoc/>
    public event WaitingEventHandler? WhenStarting
    {
        add => Wait.WhenStarting += value;
        remove => Wait.WhenStarting -= value;
    }

    /// <inheritdoc/>
    public event WaitingEventHandler? WhenFinished
    {
        add => Wait.WhenFinished += value;
        remove => Wait.WhenFinished -= value;
    }

    /// <inheritdoc/>
    public event ExceptionEventHandler? WhenExceptionOccurred
    {
        add => Wait.WhenExceptionOccurred += value;
        remove => Wait.WhenExceptionOccurred -= value;
    }

    /// <inheritdoc/>
    public IWaitOptions DefaultOptions => Wait.DefaultOptions;

    /// <inheritdoc/>
    [Obsolete("Use DefaultOptions property instead.")]
    public IObsoleteWaitOptions Options => Wait.Options;

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