using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Windows;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class WinCheckBox : WinControl, IUITestCheckBoxControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.CheckBox);

        private ToggleControlSupport _toggleControlSupport;

        public TogglePattern TogglePattern => GetPattern<TogglePattern>();

        [UIMapControl(IdStyle = IdStyle.Disabled)]
        protected virtual UITestControl ContentControl { get; set; }

        public override Point ClickablePoint => ContentControl.Exists ? ContentControl.ClickablePoint : base.ClickablePoint;

        public virtual bool Checked
        {
            get => _toggleControlSupport.GetChecked();
            set => _toggleControlSupport.SetChecked(value);
        }

        partial void OnInitialized()
        {
            _toggleControlSupport = new ToggleControlSupport(this);
        }
    }
}
