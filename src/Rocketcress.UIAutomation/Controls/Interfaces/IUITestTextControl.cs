namespace Rocketcress.UIAutomation.Controls
{
    public interface IUITestTextControl : IUITestControl
    {
        string DisplayText { get; }
        string Text { get; }
    }
}