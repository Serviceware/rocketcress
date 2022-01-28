using OpenQA.Selenium.Interactions;

namespace Rocketcress.Selenium.Interactions;

/// <summary>
/// Represents an action that executes a <see cref="Action"/>.
/// </summary>
public class LambdaAction : IAction
{
    private readonly Action _action;

    /// <summary>
    /// Initializes a new instance of the <see cref="LambdaAction"/> class.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    public LambdaAction(Action action)
    {
        Guard.NotNull(action);
        _action = action;
    }

    /// <summary>
    /// Performs this action on the browser.
    /// </summary>
    public void Perform()
    {
        _action();
    }
}
