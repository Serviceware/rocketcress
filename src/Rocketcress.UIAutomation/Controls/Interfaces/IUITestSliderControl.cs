using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls
{
    public interface IUITestSliderControl : IUITestControl
    {
        double MaximumPosition { get; }
        double MinimumPosition { get; }
        OrientationType Orientation { get; }
        double Position { get; set; }
        double SmallChange { get; }
        double LargeChange { get; }
    }
}