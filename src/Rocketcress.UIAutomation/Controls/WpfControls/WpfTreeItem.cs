using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Windows;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class WpfTreeItem : WpfControl, IUITestTreeItemControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.TreeItem);

        private static readonly By ByItem = By.Framework(FrameworkIds.Wpf).AndControlType(ControlType.TreeItem);
        private MenuControlSupport _menuControlSupport;
        private SelectionItemControlSupport _selectionItemControlSupport;

        public ExpandCollapsePattern ExpandCollapsePattern => GetPattern<ExpandCollapsePattern>();
        public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();
        public ScrollItemPattern ScrollItemPattern => GetPattern<ScrollItemPattern>();
        public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();

        [UIMapControl(IdStyle = IdStyle.Disabled)]
        protected WpfText HeaderControl { get; set; }

        public override Point ClickablePoint => HeaderControl.Exists ? HeaderControl.ClickablePoint : base.ClickablePoint;

        public virtual bool Expanded
        {
            get => ExpandCollapsePattern.Current.ExpandCollapseState == ExpandCollapseState.Expanded;
            set => (value ? (Action)ExpandCollapsePattern.Expand : ExpandCollapsePattern.Collapse)();
        }

        public virtual bool HasChildNodes => ExpandCollapsePattern.Current.ExpandCollapseState != ExpandCollapseState.LeafNode;
        public virtual string Header => HeaderControl.DisplayText;
        public virtual IEnumerable<IUITestControl> Nodes => _menuControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));
        public virtual IUITestControl ParentNode => FindElements(By.Scope(TreeScope.Ancestors).Append(ByItem, false, false)).FirstOrDefault();
        public virtual bool Selected
        {
            get => _selectionItemControlSupport.GetSelected();
            set => _selectionItemControlSupport.SetSelected(value);
        }

        partial void OnInitialized()
        {
            _menuControlSupport = new MenuControlSupport(this, ByItem, ControlType.Tree, ControlType.TreeItem);
            _selectionItemControlSupport = new SelectionItemControlSupport(this);
        }
    }
}
