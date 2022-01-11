namespace Rocketcress.UIAutomation.Controls
{
    public interface IUITestTabListControl : IUITestControl
    {
        int SelectedIndex { get; set; }
        IEnumerable<IUITestControl> Tabs { get; }
    }

    public interface IUITestTabListControl<TPage> : IUITestTabListControl
        where TPage : IUITestTabPageControl
    {
        new IEnumerable<TPage> Tabs { get; }
    }
}