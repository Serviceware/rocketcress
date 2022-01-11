namespace Rocketcress.UIAutomation.Controls
{
    public interface IUITestComboBoxControl : IUITestControl
    {
        string SelectedItem { get; set; }
        int SelectedIndex { get; set; }
        string Text { get; set; }
        bool Expanded { get; set; }
        IEnumerable<IUITestControl> Items { get; }
    }
}
