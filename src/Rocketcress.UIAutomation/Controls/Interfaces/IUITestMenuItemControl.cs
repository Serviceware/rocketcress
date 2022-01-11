namespace Rocketcress.UIAutomation.Controls
{
    public interface IUITestMenuItemControl : IUITestControl
    {
        string Header { get; }
        bool Checked { get; set; }
        bool Expanded { get; set; }
        IEnumerable<IUITestControl> Items { get; }
    }
}
