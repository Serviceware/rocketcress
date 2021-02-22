using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.CommonControls
{
    [AutoDetectControl(Priority = -50)]
    public class CommonMenuItem : UITestControl, IUITestMenuItemControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.MenuItem);

        #region Patterns
        public TogglePattern TogglePattern => GetPattern<TogglePattern>();
        public ExpandCollapsePattern ExpandCollapsePattern => GetPattern<ExpandCollapsePattern>();
        #endregion

        #region Construcotrs
        public CommonMenuItem(By locationKey) : base(locationKey) { }
        public CommonMenuItem(IUITestControl parent) : base(parent) { }
        public CommonMenuItem(AutomationElement element) : base(element) { }
        public CommonMenuItem(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public CommonMenuItem(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected CommonMenuItem() { }

        protected override void Initialize()
        {
            base.Initialize();
            ToggleControlSupport = new ToggleControlSupport(this);
            MenuControlSupport = new MenuControlSupport(this, By.ControlType(ControlType.MenuItem), ControlType.Menu, ControlType.MenuItem);
            ExpandCollapseControlSupport = new ExpandCollapseControlSupport(this);
            HeaderControl = new CommonText(By.Empty, this);
        }
        #endregion

        #region Properties
        public ToggleControlSupport ToggleControlSupport { get; private set; }
        public MenuControlSupport MenuControlSupport { get; private set; }
        public ExpandCollapseControlSupport ExpandCollapseControlSupport { get; private set; }
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

        public virtual IEnumerable<IUITestControl> Items => MenuControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(x));
        #endregion
    }
}
