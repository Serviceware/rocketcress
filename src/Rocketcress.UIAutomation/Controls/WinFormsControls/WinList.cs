using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class WinList : WinControl, IUITestListControl<WinListItem>
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.List);

        private static readonly By ByItem = By.Scope(TreeScope.Children).AndFramework(FrameworkIds.WindowsForms).AndControlType(ControlType.ListItem);
        private ListControlSupport _listControlSupport;

        public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();
        public ScrollPattern ScrollPattern => GetPattern<ScrollPattern>();
        public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();
        public SynchronizedInputPattern SynchronizedInputPattern => GetPattern<SynchronizedInputPattern>();

        protected virtual IEnumerable<IUITestControl> ItemsInternal => _listControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

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

        IEnumerable<IUITestControl> IUITestListControl.Items => ItemsInternal;

        partial void OnInitialized()
        {
            _listControlSupport = new ListControlSupport(this, ByItem);
        }
    }
}
