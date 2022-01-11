namespace Rocketcress.UIAutomation.Controls.Win32Controls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class Win32Text : Win32Control, IUITestTextControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Text);

        public virtual string DisplayText => Name;
        public virtual string Text => Name;
    }
}
