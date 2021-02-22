using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    public class WinMenu : WinControl, IUITestMenuControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Menu);

        #region Private Fields
        private static readonly By ByItem = By.Framework(FrameworkIds.WindowsForms).AndControlType(ControlType.MenuItem);
        public MenuControlSupport MenuControlSupport { get; private set; }
        #endregion

        #region Constructors
        public WinMenu(By locationKey) : base(locationKey) { }
        public WinMenu(IUITestControl parent) : base(parent) { }
        public WinMenu(AutomationElement element) : base(element) { }
        public WinMenu(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WinMenu(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WinMenu() { }

        protected override void Initialize()
        {
            base.Initialize();
            MenuControlSupport = new MenuControlSupport(this, ByItem, ControlType.Menu, ControlType.MenuItem);
        }
        #endregion

        #region Public Properties
        public virtual IEnumerable<IUITestControl> Items => MenuControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(x));
        #endregion
    }
}
