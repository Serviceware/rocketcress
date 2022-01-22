namespace Rocketcress.Core;

/// <summary>
/// Executes an action when an instance of this class gets disposed.
/// </summary>
public sealed class ActionOnDispose : IDisposable
{
    private readonly Stopwatch? _stopwatch;
    private readonly Action<int>? _actionWithTime;
    private readonly Action? _actionWithoutTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionOnDispose"/> class.
    /// </summary>
    /// <param name="actionWithTime">The action to execute when this instance gets disposed. Has a parameter that is given the time in miliseconds that took from initialization to disposing.</param>
    public ActionOnDispose(Action<int> actionWithTime)
    {
        _actionWithTime = actionWithTime;
        _stopwatch = new Stopwatch();
        _stopwatch.Start();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionOnDispose"/> class.
    /// </summary>
    /// <param name="actionWithoutTime">The action to execute when this instance gets disposed.</param>
    public ActionOnDispose(Action actionWithoutTime)
    {
        _actionWithoutTime = actionWithoutTime;
    }

    private ActionOnDispose()
    {
    }

    /// <summary>
    /// Gets an empty <see cref="ActionOnDispose"/> object.
    /// </summary>
    public static ActionOnDispose Empty { get; } = new();

    /// <summary>
    /// Executes the underlying action.
    /// </summary>
    public void Dispose()
    {
        if (_stopwatch != null)
            _stopwatch.Stop();
        if (_actionWithTime != null && _stopwatch != null)
            _actionWithTime((int)_stopwatch.ElapsedMilliseconds);
        _actionWithoutTime?.Invoke();
    }
}
