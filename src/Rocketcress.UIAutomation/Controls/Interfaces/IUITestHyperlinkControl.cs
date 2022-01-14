namespace Rocketcress.UIAutomation.Controls;

public interface IUITestHyperlinkControl : IUITestControl
{
    string Alt { get; }
    void Invoke();
}
