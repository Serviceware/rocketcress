using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.CommonControls
{
    [AutoDetectControl(Priority = -50)]
    public class CommonMenu : UITestControl, IUITestMenuControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Menu);

        #region Private Fields

        private static readonly By ByItem = By.ControlType(ControlType.MenuItem);
        public MenuControlSupport MenuControlSupport { get; private set; }

        #endregion

        #region Constructors

        public CommonMenu(By locationKey)
            : base(locationKey)
        {
        }

        public CommonMenu(IUITestControl parent)
            : base(parent)
        {
        }

        public CommonMenu(AutomationElement element)
            : base(element)
        {
        }

        public CommonMenu(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public CommonMenu(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected CommonMenu()
        {
        }

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
