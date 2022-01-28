namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a UIAutomation control that is invokable.
/// </summary>
public interface IUITestInvokableControl
{
    /// <summary>
    /// Invokes the <see cref="InvokePattern"/> of this control.
    /// </summary>
    void Invoke();
}
