using Rocketcress.Core.Extensions;
using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class WinTabList : WinControl, IUITestTabListControl<WinTabPage>
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Tab);

        private ListControlSupport _listControlSupport;

        public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();

        protected virtual IEnumerable<IUITestControl> TabsInternal => _listControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

        public virtual int SelectedIndex
        {
            get => _listControlSupport.GetSelectedIndices().TryFirst(out var index) ? index : -1;
            set => _listControlSupport.SetSelectedIndex(value);
        }

        public IEnumerable<WinTabPage> Tabs => TabsInternal.OfType<WinTabPage>();

        IEnumerable<IUITestControl> IUITestTabListControl.Tabs => Tabs;

        partial void OnInitialized()
        {
            _listControlSupport = new ListControlSupport(this, By.Framework(FrameworkIds.WindowsForms).AndControlType(ControlType.TabItem));
        }
    }
}
