namespace Rocketcress.UIAutomation.Controls
{
    public interface IUITestButtonControl : IUITestControl
    {
        string DisplayText { get; }

        void Invoke();
    }
}
