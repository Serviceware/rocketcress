using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation cell control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestCellControl" />
[AutoDetectControl]
[GenerateUIMapParts(IdStyle = IdStyle.Disabled)]
public partial class WpfCell : WpfControl, IUITestCellControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey
        .AndControlType(ControlType.Custom)
        .AndProperty(AutomationElement.IsGridItemPatternAvailableProperty, true);

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Base Location Key should be on top.")]
    private static readonly By ByItem = By.Scope(TreeScope.Children).AndFramework(FrameworkIds.Wpf).AndControlType(ControlType.ListItem);

    private ValueControlSupport _valueControlSupport;
    private ToggleControlSupport _toggleControlSupport;
    private ListControlSupport _listControlSupport;

    /// <summary>
    /// Gets the grid item pattern.
    /// </summary>
    public GridItemPattern GridItemPattern => GetPattern<GridItemPattern>();

    /// <summary>
    /// Gets the invoke pattern.
    /// </summary>
    public InvokePattern InvokePattern => GetPattern<InvokePattern>();

    /// <summary>
    /// Gets the scroll item pattern.
    /// </summary>
    public ScrollItemPattern ScrollItemPattern => GetPattern<ScrollItemPattern>();

    /// <summary>
    /// Gets the table item pattern.
    /// </summary>
    public TableItemPattern TableItemPattern => GetPattern<TableItemPattern>();

    /// <summary>
    /// Gets the value pattern.
    /// </summary>
    public ValuePattern ValuePattern => GetPattern<ValuePattern>();

    /// <summary>
    /// Gets the toggle pattern.
    /// </summary>
    public TogglePattern TogglePattern => GetPattern<TogglePattern>();

    /// <inheritdoc/>
    public virtual string Value
    {
        get => ValuePattern.Current.Value;
        set
        {
            _valueControlSupport.SetValue(value);
            SendKeys("{ENTER}");
        }
    }

    /// <inheritdoc/>
    public virtual int RowIndex => GridItemPattern.Current.Row;

    /// <inheritdoc/>
    public virtual int ColumnIndex => GridItemPattern.Current.Column;

    /// <inheritdoc/>
    public virtual bool Checked
    {
        get => _toggleControlSupport.GetChecked();
        set
        {
            SetFocus();
            _toggleControlSupport.SetChecked(value);
        }
    }

    /// <summary>
    /// Gets or sets the selected item.
    /// </summary>
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

    /// <inheritdoc/>
    public bool Indeterminate => TogglePattern.Current.ToggleState == ToggleState.Indeterminate;

    [UIMapControl]
    private WpfCheckBox CheckBox { get; set; }

    [UIMapControl]
    private WpfComboBox ComboBox { get; set; }

    partial void OnInitialized()
    {
        _valueControlSupport = new ValueControlSupport(this);
        _toggleControlSupport = new ToggleControlSupport(CheckBox);
        _listControlSupport = new ListControlSupport(ComboBox, ByItem);
    }
}
