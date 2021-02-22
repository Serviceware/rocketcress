namespace Rocketcress.UIAutomation.Controls
{
    public interface IUITestToggleButtonControl : IUITestControl
    {
        string DisplayText { get; }
        bool Indeterminate { get; }
        bool Pressed { get; set; }
    }
}