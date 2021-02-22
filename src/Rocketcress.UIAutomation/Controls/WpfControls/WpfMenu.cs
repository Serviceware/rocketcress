using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfMenu : WpfControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Menu);

        #region Private Fields
        private static readonly By ByItem = By.Framework(FrameworkIds.Wpf).AndControlType(ControlType.MenuItem);
        public MenuControlSupport MenuControlSupport { get; private set; }
        #endregion

        #region Constructors
        public WpfMenu(By locationKey) : base(locationKey) { }
        public WpfMenu(IUITestControl parent) : base(parent) { }
        public WpfMenu(AutomationElement element) : base(element) { }
        public WpfMenu(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WpfMenu(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WpfMenu() { }
   
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
