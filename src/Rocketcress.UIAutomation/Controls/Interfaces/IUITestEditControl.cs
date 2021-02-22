namespace Rocketcress.UIAutomation.Controls
{
    public interface IUITestEditControl : IUITestControl
    {
        string Text { get; set; }
        bool ReadOnly { get; }
    }
}
