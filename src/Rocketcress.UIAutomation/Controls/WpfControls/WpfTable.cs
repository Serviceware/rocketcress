using Rocketcress.UIAutomation.Common;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfTable : WpfControl, IUITestTableControl<WpfRow, WpfCell>
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.DataGrid);

        #region Private Fields
        private static readonly By ByRow = By.Framework(FrameworkIds.Wpf).AndControlType(ControlType.DataItem);
        private static readonly By ByCell = By
            .ChildOf(ByRow)
            .AndFramework(FrameworkIds.Wpf)
            .AndPatternAvailable<GridItemPattern>();
        #endregion

        #region Patterns
        public ScrollPattern ScrollPattern => GetPattern<ScrollPattern>();
        public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();
        public TablePattern TablePattern => GetPattern<TablePattern>();
        public GridPattern GridPattern => GetPattern<GridPattern>();
        public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();
        #endregion

        #region Construcotrs
        public WpfTable(By locationKey)
            : base(locationKey)
        {
        }

        public WpfTable(IUITestControl parent)
            : base(parent)
        {
        }

        public WpfTable(AutomationElement element)
            : base(element)
        {
        }

        public WpfTable(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WpfTable(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WpfTable()
        {
        }
        #endregion

        #region Public Properties
        public virtual bool CanSelectMultiple => SelectionPattern.Current.CanSelectMultiple;
        public IEnumerable<WpfCell> Cells => CellsInternal.OfType<WpfCell>();
        public virtual int ColumnCount => TablePattern.Current.ColumnCount;
        public virtual IEnumerable<IUITestControl> ColumnHeaders => TablePattern.Current.GetColumnHeaders().Select(x => ControlUtility.GetControl(x));
        public virtual int RowCount => TablePattern.Current.RowCount;
        public virtual IEnumerable<IUITestControl> RowHeaders => TablePattern.Current.GetRowHeaders().Select(x => ControlUtility.GetControl(x));
        public IEnumerable<WpfRow> Rows => RowsInternal.OfType<WpfRow>();
        #endregion

        #region Protected Members
        protected virtual IEnumerable<IUITestControl> CellsInternal => FindElements(ByCell);
        protected virtual IEnumerable<IUITestControl> RowsInternal => FindElements(ByRow);
        #endregion

        #region Public Methods
        public virtual WpfCell GetCell(int row, int column)
        {
            var element = TablePattern.GetItem(row, column);
            return element == null ? null : new WpfCell(element);
        }

        public virtual string[] GetColumnNames() => ColumnHeaders.Select(x => x.Name).ToArray();
        public virtual WpfRow GetRow(int index) => new WpfRow(ByRow.AndSkip(index), this);
        public virtual WpfCell FindFirstCellWithValue(string value)
        {
            var element = ItemContainerPattern.FindItemByProperty(null, ValuePattern.ValueProperty, value);
            return element == null ? null : new WpfCell(element);
        }

        public virtual WpfRow this[int rowIndex] => GetRow(rowIndex);
        #endregion

        #region IUITestTableControl Members
        IEnumerable<IUITestControl> IUITestTableControl.Cells => CellsInternal;
        IEnumerable<IUITestControl> IUITestTableControl.Rows => RowsInternal;

        IUITestControl IUITestTableControl.GetCell(int row, int column) => GetCell(row, column);
        IUITestControl IUITestTableControl.GetRow(int index) => GetRow(index);
        IUITestControl IUITestTableControl.FindFirstCellWithValue(string value) => FindFirstCellWithValue(value);
        IUITestControl IUITestTableControl.this[int rowIndex] => this[rowIndex];
        #endregion
    }
}
