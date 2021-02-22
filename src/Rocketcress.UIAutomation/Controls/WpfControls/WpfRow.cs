using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfRow : WpfControl, IUITestRowControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.DataItem);

        #region Patterns
        public InvokePattern InvokePattern => GetPattern<InvokePattern>();
        public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();
        public ScrollItemPattern ScrollItemPattern => GetPattern<ScrollItemPattern>();
        public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();
        public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();
        #endregion

        #region Construcotrs
        public WpfRow(By locationKey) : base(locationKey) { }
        public WpfRow(IUITestControl parent) : base(parent) { }
        public WpfRow(AutomationElement element) : base(element) { }
        public WpfRow(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WpfRow(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WpfRow() { }
        #endregion

        #region Public Properties
        public virtual bool CanSelectMultiple => SelectionPattern.Current.CanSelectMultiple;
        public virtual IEnumerable<IUITestControl> Cells => FindElements(By.PatternAvailable<GridItemPattern>());
        public virtual IUITestControl Header => FindElement(By.ControlType(ControlType.HeaderItem));
        public virtual int RowIndex => Cells.FirstOrDefault()?.GetPattern<TableItemPattern>().Current.Row ?? -1;
        public virtual bool Selected => SelectionItemPattern.Current.IsSelected;
        public virtual IUITestControl GetCell(int index) => new UITestControl(By.PatternAvailable<GridItemPattern>().AndSkip(index), this);

        public virtual IUITestControl this[int cellIndex] => GetCell(cellIndex);
        #endregion
    }
}
