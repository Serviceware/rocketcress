namespace Rocketcress.UIAutomation.Controls;

public interface IUITestTreeControl : IUITestControl
{
    IEnumerable<IUITestControl> Nodes { get; }
}
