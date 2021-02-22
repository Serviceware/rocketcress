using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfTreeItem : WpfControl, IUITestTreeItemControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.TreeItem);

        #region Private Fields
        private static readonly By ByItem = By.Framework(FrameworkIds.Wpf).AndControlType(ControlType.TreeItem);
        private MenuControlSupport _menuControlSupport;
        private SelectionItemControlSupport _selectionItemControlSupport;
        #endregion

        #region Patterns
        public ExpandCollapsePattern ExpandCollapsePattern => GetPattern<ExpandCollapsePattern>();
        public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();
        public ScrollItemPattern ScrollItemPattern => GetPattern<ScrollItemPattern>();
        public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();
        #endregion

        #region Constructors
        public WpfTreeItem(By locationKey) : base(locationKey) { }
        public WpfTreeItem(IUITestControl parent) : base(parent) { }
        public WpfTreeItem(AutomationElement element) : base(element) { }
        public WpfTreeItem(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WpfTreeItem(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WpfTreeItem() { }

        protected override void Initialize()
        {
            base.Initialize();
            _menuControlSupport = new MenuControlSupport(this, ByItem, ControlType.Tree, ControlType.TreeItem);
            _selectionItemControlSupport = new SelectionItemControlSupport(this);
            HeaderControl = new WpfText(By.Empty, this);
        }
        #endregion

        #region Properties
        protected WpfText HeaderControl { get; set; }

        public override Point ClickablePoint => HeaderControl.Exists ? HeaderControl.ClickablePoint : base.ClickablePoint;

        public virtual bool Expanded
        {
            get => ExpandCollapsePattern.Current.ExpandCollapseState == ExpandCollapseState.Expanded;
            set => (value ? (Action)ExpandCollapsePattern.Expand : ExpandCollapsePattern.Collapse)();
        }
        public virtual bool HasChildNodes => ExpandCollapsePattern.Current.ExpandCollapseState != ExpandCollapseState.LeafNode;
        public virtual string Header => HeaderControl.DisplayText;
        public virtual IEnumerable<IUITestControl> Nodes => _menuControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(x));
        public virtual IUITestControl ParentNode => FindElements(By.Scope(TreeScope.Ancestors).Append(ByItem, false, false)).FirstOrDefault();
        public virtual bool Selected
        {
            get => _selectionItemControlSupport.GetSelected();
            set => _selectionItemControlSupport.SetSelected(value);
        }
        #endregion
    }
}
