namespace Rocketcress.UIAutomation.Controls
{
    public interface IUITestProgressBarControl : IUITestControl
    {
        double MinimumValue { get; }
        double MaximumValue { get; }
        double Position { get; }
    }
}