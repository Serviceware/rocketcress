using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    public class WinList : WinControl, IUITestListControl<WinListItem>
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.List);

        #region Private Fields
        private static readonly By ByItem = By.Scope(TreeScope.Children).AndFramework(FrameworkIds.WindowsForms).AndControlType(ControlType.ListItem);
        private ListControlSupport _listControlSupport;
        #endregion

        #region Patterns
        public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();
        public ScrollPattern ScrollPattern => GetPattern<ScrollPattern>();
        public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();
        public SynchronizedInputPattern SynchronizedInputPattern => GetPattern<SynchronizedInputPattern>();
        #endregion

        #region Constructors
        public WinList(By locationKey) : base(locationKey) { }
        public WinList(IUITestControl parent) : base(parent) { }
        public WinList(AutomationElement element) : base(element) { }
        public WinList(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WinList(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WinList() { }

        protected override void Initialize()
        {
            base.Initialize();
            _listControlSupport = new ListControlSupport(this, ByItem);
        }
        #endregion

        #region Protected Members
        protected virtual IEnumerable<IUITestControl> ItemsInternal => _listControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(x));
        #endregion

        #region Public Properties
        public virtual string[] SelectedItems
        {
            get => _listControlSupport.GetSelectedItems().ToArray();
            set => _listControlSupport.SetSelectedItems(value);
        }

        public virtual int[] SelectedIndices
        {
            get => _listControlSupport.GetSelectedIndices().ToArray();
            set => _listControlSupport.SetSelectedIndices(value);
        }

        public virtual IEnumerable<WinListItem> Items => ItemsInternal.OfType<WinListItem>();
        public virtual IEnumerable<AutomationElement> NativeItems => _listControlSupport.EnumerateItems();
        public virtual bool IsMultiSelection => SelectionPattern.Current.CanSelectMultiple;
        #endregion

        #region IUITestListControl Members
        IEnumerable<IUITestControl> IUITestListControl.Items => ItemsInternal;
        #endregion
    }
}
