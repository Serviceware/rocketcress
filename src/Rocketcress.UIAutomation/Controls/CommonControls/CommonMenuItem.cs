using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.CommonControls
{
    [AutoDetectControl(Priority = -50)]
    [GenerateUIMapParts]
    public partial class CommonMenuItem : UITestControl, IUITestMenuItemControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.MenuItem);

        public TogglePattern TogglePattern => GetPattern<TogglePattern>();
        public ExpandCollapsePattern ExpandCollapsePattern => GetPattern<ExpandCollapsePattern>();

        public ToggleControlSupport ToggleControlSupport { get; private set; }
        public MenuControlSupport MenuControlSupport { get; private set; }
        public ExpandCollapseControlSupport ExpandCollapseControlSupport { get; private set; }

        [UIMapControl(IdStyle = IdStyle.Disabled)]
        protected virtual CommonText HeaderControl { get; set; }

        public virtual string Header => HeaderControl.DisplayText;

        public virtual bool Checked
        {
            get => ToggleControlSupport.GetChecked();
            set => ToggleControlSupport.SetChecked(value, true);
        }

        public virtual bool Expanded
        {
            get => ExpandCollapseControlSupport.GetExpanded();
            set => ExpandCollapseControlSupport.SetExpanded(value);
        }

        public virtual IEnumerable<IUITestControl> Items => MenuControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

        partial void OnInitialized()
        {
            ToggleControlSupport = new ToggleControlSupport(this);
            MenuControlSupport = new MenuControlSupport(this, By.ControlType(ControlType.MenuItem), ControlType.Menu, ControlType.MenuItem);
            ExpandCollapseControlSupport = new ExpandCollapseControlSupport(this);
        }
    }
}
