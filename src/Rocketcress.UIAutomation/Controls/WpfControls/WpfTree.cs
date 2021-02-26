using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfTree : WpfControl, IUITestTreeControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Tree);

        #region Private Fields
        private static readonly By ByItem = By.Framework(FrameworkIds.Wpf).AndControlType(ControlType.TreeItem);
        private MenuControlSupport _menuControlSupport;
        #endregion

        #region Patterns
        public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();
        public ScrollPattern ScrollPattern => GetPattern<ScrollPattern>();
        public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();
        #endregion

        #region Construcotrs
        public WpfTree(By locationKey)
            : base(locationKey)
        {
        }

        public WpfTree(IUITestControl parent)
            : base(parent)
        {
        }

        public WpfTree(AutomationElement element)
            : base(element)
        {
        }

        public WpfTree(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WpfTree(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WpfTree()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            _menuControlSupport = new MenuControlSupport(this, ByItem, ControlType.Tree, ControlType.TreeItem);
        }
        #endregion

        #region Public Properties
        public virtual IEnumerable<IUITestControl> Nodes => _menuControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(x));
        #endregion
    }
}
