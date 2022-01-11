namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class WpfText : WpfControl, IUITestTextControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Text);

        public virtual string DisplayText => Name;
        public virtual string Text => Name;
    }
}
