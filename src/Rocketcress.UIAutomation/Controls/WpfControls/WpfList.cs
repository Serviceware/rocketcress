using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfList : UITestControl, IUITestListControl<WpfListItem>
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.List);

        #region Private Fields
        private static readonly By ByItem = By.Scope(TreeScope.Children).AndFramework(FrameworkIds.Wpf).AndControlType(ControlType.ListItem);
        private ListControlSupport _listControlSupport;
        #endregion

        #region Patterns
        public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();
        public ScrollPattern ScrollPattern => GetPattern<ScrollPattern>();
        public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();
        public SynchronizedInputPattern SynchronizedInputPattern => GetPattern<SynchronizedInputPattern>();
        #endregion

        #region Constructors
        public WpfList(By locationKey)
            : base(locationKey)
        {
        }

        public WpfList(IUITestControl parent)
            : base(parent)
        {
        }

        public WpfList(AutomationElement element)
            : base(element)
        {
        }

        public WpfList(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WpfList(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WpfList()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            _listControlSupport = new ListControlSupport(this, ByItem);
        }
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

        IEnumerable<IUITestControl> IUITestListControl.Items => Items;
        public virtual IEnumerable<WpfListItem> Items => _listControlSupport.EnumerateItems().Select(x => new WpfListItem(x));
        public virtual bool IsMultiSelection => SelectionPattern.Current.CanSelectMultiple;
        #endregion
    }
}
