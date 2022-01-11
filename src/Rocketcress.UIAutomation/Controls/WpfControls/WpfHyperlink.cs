namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class WpfHyperlink : WpfControl, IUITestHyperlinkControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Hyperlink);

        public InvokePattern InvokePattern => GetPattern<InvokePattern>();

        public virtual string Alt => Name;

        public virtual void Invoke() => InvokePattern.Invoke();
    }
}
