using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class WpfTree : WpfControl, IUITestTreeControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Tree);

        private static readonly By ByItem = By.Framework(FrameworkIds.Wpf).AndControlType(ControlType.TreeItem);
        private MenuControlSupport _menuControlSupport;

        public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();
        public ScrollPattern ScrollPattern => GetPattern<ScrollPattern>();
        public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();

        public virtual IEnumerable<IUITestControl> Nodes => _menuControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

        partial void OnInitialized()
        {
            _menuControlSupport = new MenuControlSupport(this, ByItem, ControlType.Tree, ControlType.TreeItem);
        }
    }
}
