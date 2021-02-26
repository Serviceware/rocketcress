using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfCell : WpfControl, IUITestCellControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey
            .AndControlType(ControlType.Custom)
            .AndProperty(AutomationElement.IsGridItemPatternAvailableProperty, true);

        #region Private Fields
        private ValueControlSupport _valueControlSupport;
        private ToggleControlSupport _toggleControlSupport;
        private ListControlSupport _listControlSupport;
        private static readonly By ByItem = By.Scope(TreeScope.Children).AndFramework(FrameworkIds.Wpf).AndControlType(ControlType.ListItem);

        private WpfCheckBox _checkBox;
        private WpfComboBox _comboBox;
        #endregion

        #region Patterns
        public GridItemPattern GridItemPattern => GetPattern<GridItemPattern>();
        public InvokePattern InvokePattern => GetPattern<InvokePattern>();
        public ScrollItemPattern ScrollItemPattern => GetPattern<ScrollItemPattern>();
        public TableItemPattern TableItemPattern => GetPattern<TableItemPattern>();
        public ValuePattern ValuePattern => GetPattern<ValuePattern>();
        public TogglePattern TogglePattern => GetPattern<TogglePattern>();
        #endregion

        #region Construcotrs
        public WpfCell(By locationKey)
            : base(locationKey)
        {
        }

        public WpfCell(IUITestControl parent)
            : base(parent)
        {
        }

        public WpfCell(AutomationElement element)
            : base(element)
        {
        }

        public WpfCell(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WpfCell(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WpfCell()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            _valueControlSupport = new ValueControlSupport(this);
            _checkBox = new WpfCheckBox(this);
            _toggleControlSupport = new ToggleControlSupport(_checkBox);
            _comboBox = new WpfComboBox(this);
            _listControlSupport = new ListControlSupport(_comboBox, ByItem);
        }
        #endregion

        #region Public Properties
        public virtual string Value
        {
            get => ValuePattern.Current.Value;
            set
            {
                _valueControlSupport.SetValue(value);
                SendKeys("{ENTER}");
            }
        }

        public virtual int RowIndex => GridItemPattern.Current.Row;
        public virtual int ColumnIndex => GridItemPattern.Current.Column;
        public virtual bool Checked
        {
            get => _toggleControlSupport.GetChecked();
            set
            {
                SetFocus();
                _toggleControlSupport.SetChecked(value);
            }
        }

        public virtual string SelectedItem
        {
            get => _listControlSupport.GetSelectedItems().FirstOrDefault();
            set
            {
                DoubleClick();
                _comboBox.Find();
                _listControlSupport.SetSelectedItem(value);
            }
        }
        #endregion
    }
}
