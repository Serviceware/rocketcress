using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls
{
    public interface IUITestScrollBarControl : IUITestControl
    {
        double MaximumPosition { get; }
        double MinimumPosition { get; }
        OrientationType Orientation { get; }
        double Position { get; set; }
    }
}