namespace Rocketcress.UIAutomation.Controls
{
    public interface IUITestRadioButtonControl : IUITestControl
    {
        IUITestControl Group { get; }
        bool Selected { get; set; }
    }
}