using Rocketcress.Core.Extensions;
using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    public class WinTabList : WinControl, IUITestTabListControl<WinTabPage>
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Tab);

        #region Private Fields
        private ListControlSupport _listControlSupport;
        #endregion

        #region Patterns
        public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();
        #endregion

        #region Constructors
        public WinTabList(By locationKey)
            : base(locationKey)
        {
        }

        public WinTabList(IUITestControl parent)
            : base(parent)
        {
        }

        public WinTabList(AutomationElement element)
            : base(element)
        {
        }

        public WinTabList(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WinTabList(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WinTabList()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            _listControlSupport = new ListControlSupport(this, By.Framework(FrameworkIds.WindowsForms).AndControlType(ControlType.TabItem));
        }
        #endregion

        #region Proptected Members
        protected virtual IEnumerable<IUITestControl> TabsInternal => _listControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(x));
        #endregion

        #region Public Properties
        public virtual int SelectedIndex
        {
            get => _listControlSupport.GetSelectedIndices().TryFirst(out var index) ? index : -1;
            set => _listControlSupport.SetSelectedIndex(value);
        }

        public IEnumerable<WinTabPage> Tabs => TabsInternal.OfType<WinTabPage>();
        #endregion

        #region IUITestTabListControl Members
        IEnumerable<IUITestControl> IUITestTabListControl.Tabs => Tabs;
        #endregion
    }
}
