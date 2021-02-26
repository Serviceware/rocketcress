using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfMenuItem : WpfControl, IUITestMenuItemControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.MenuItem);

        #region Patterns
        public TogglePattern TogglePattern => GetPattern<TogglePattern>();
        public ExpandCollapsePattern ExpandCollapsePattern => GetPattern<ExpandCollapsePattern>();
        #endregion

        #region Construcotrs
        public WpfMenuItem(By locationKey)
            : base(locationKey)
        {
        }

        public WpfMenuItem(IUITestControl parent)
            : base(parent)
        {
        }

        public WpfMenuItem(AutomationElement element)
            : base(element)
        {
        }

        public WpfMenuItem(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WpfMenuItem(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WpfMenuItem()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            ToggleControlSupport = new ToggleControlSupport(this);
            MenuControlSupport = new MenuControlSupport(this, By.Framework(FrameworkIds.Wpf).AndControlType(ControlType.MenuItem), ControlType.Menu, ControlType.MenuItem);
            ExpandCollapseControlSupport = new ExpandCollapseControlSupport(this);
            HeaderControl = new WpfText(By.Empty, this);
        }
        #endregion

        #region Properties
        public ToggleControlSupport ToggleControlSupport { get; private set; }
        public MenuControlSupport MenuControlSupport { get; private set; }
        public ExpandCollapseControlSupport ExpandCollapseControlSupport { get; private set; }
        protected virtual WpfText HeaderControl { get; set; }

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
