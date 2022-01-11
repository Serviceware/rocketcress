using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    [GenerateUIMapParts(IdStyle = IdStyle.Disabled)]
    public partial class WpfCell : WpfControl, IUITestCellControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey
            .AndControlType(ControlType.Custom)
            .AndProperty(AutomationElement.IsGridItemPatternAvailableProperty, true);

        private static readonly By ByItem = By.Scope(TreeScope.Children).AndFramework(FrameworkIds.Wpf).AndControlType(ControlType.ListItem);

        private ValueControlSupport _valueControlSupport;
        private ToggleControlSupport _toggleControlSupport;
        private ListControlSupport _listControlSupport;

        [UIMapControl]
        private WpfCheckBox CheckBox { get; set; }

        [UIMapControl]
        private WpfComboBox ComboBox { get; set; }

        public GridItemPattern GridItemPattern => GetPattern<GridItemPattern>();
        public InvokePattern InvokePattern => GetPattern<InvokePattern>();
        public ScrollItemPattern ScrollItemPattern => GetPattern<ScrollItemPattern>();
        public TableItemPattern TableItemPattern => GetPattern<TableItemPattern>();
        public ValuePattern ValuePattern => GetPattern<ValuePattern>();
        public TogglePattern TogglePattern => GetPattern<TogglePattern>();

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
                ComboBox.Find();
                _listControlSupport.SetSelectedItem(value);
            }
        }

        partial void OnInitialized()
        {
            _valueControlSupport = new ValueControlSupport(this);
            _toggleControlSupport = new ToggleControlSupport(CheckBox);
            _listControlSupport = new ListControlSupport(ComboBox, ByItem);
        }
    }
}
