using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using Rocketcress.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfTabList : WpfControl, IUITestTabListControl<WpfTabPage>
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Tab);

        #region Private Fields
        private ListControlSupport _listControlSupport;
        #endregion

        #region Patterns
        public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();
        public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();
        #endregion

        #region Constructors
        public WpfTabList(By locationKey) : base(locationKey) { }
        public WpfTabList(IUITestControl parent) : base(parent) { }
        public WpfTabList(AutomationElement element) : base(element) { }
        public WpfTabList(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WpfTabList(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WpfTabList() { }

        protected override void Initialize()
        {
            base.Initialize();
            _listControlSupport = new ListControlSupport(this, By.Framework(FrameworkIds.Wpf).AndControlType(ControlType.TabItem));
        }
        #endregion

        #region Protected Members
        protected virtual IEnumerable<IUITestControl> TabsInternal => _listControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(x));
        #endregion

        #region Public Properties
        public virtual int SelectedIndex
        {
            get => _listControlSupport.GetSelectedIndices().TryFirst(out var index) ? index : -1;
            set => _listControlSupport.SetSelectedIndex(value);
        }
        public IEnumerable<WpfTabPage> Tabs => TabsInternal.OfType<WpfTabPage>();
        #endregion

        #region IUITestTabListControl Members
        IEnumerable<IUITestControl> IUITestTabListControl.Tabs => TabsInternal;
        #endregion
    }
}
