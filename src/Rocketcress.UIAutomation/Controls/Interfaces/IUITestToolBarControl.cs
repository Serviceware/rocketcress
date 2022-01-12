namespace Rocketcress.UIAutomation.Controls;

public interface IUITestToolBarControl : IUITestControl
{
    IEnumerable<IUITestControl> Items { get; }
}
