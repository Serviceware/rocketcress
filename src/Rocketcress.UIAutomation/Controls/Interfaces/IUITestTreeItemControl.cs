namespace Rocketcress.UIAutomation.Controls
{
    public interface IUITestTreeItemControl : IUITestControl
    {
        bool Expanded { get; set; }
        bool HasChildNodes { get; }
        string Header { get; }
        IEnumerable<IUITestControl> Nodes { get; }
        IUITestControl ParentNode { get; }
        bool Selected { get; set; }
    }
}