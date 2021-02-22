namespace Rocketcress.UIAutomation.Controls
{
    public interface IUITestHyperlink : IUITestControl
    {
        string Alt { get; }
        void Invoke();
    }
}
