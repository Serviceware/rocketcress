namespace Rocketcress.UIAutomation.Controls;

public interface IUITestListItemControl : IUITestControl
{
    bool Selected { get; set; }
    string DisplayText { get; }

    void Select();
}
