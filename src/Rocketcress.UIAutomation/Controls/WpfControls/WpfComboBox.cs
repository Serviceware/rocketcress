using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using Rocketcress.UIAutomation.Exceptions;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation combo box control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestComboBoxControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfComboBox : WpfControl, IUITestComboBoxControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ComboBox);

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Base Location Key should be on top.")]
    private static readonly By ByItem = By.Scope(TreeScope.Children).AndFramework(FrameworkIds.Wpf).AndControlType(ControlType.ListItem);

    private ValueControlSupport _valueControlSupport;
    private ListControlSupport _listControlSupport;

    /// <summary>
    /// Gets the expand collapse pattern.
    /// </summary>
    public ExpandCollapsePattern ExpandCollapsePattern => GetPattern<ExpandCollapsePattern>();

    /// <summary>
    /// Gets the item container pattern.
    /// </summary>
    public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();

    /// <summary>
    /// Gets the scroll pattern.
    /// </summary>
    public ScrollPattern ScrollPattern => GetPattern<ScrollPattern>();

    /// <summary>
    /// Gets the selection pattern.
    /// </summary>
    public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();

    /// <summary>
    /// Gets the value pattern.
    /// </summary>
    public ValuePattern ValuePattern => GetPattern<ValuePattern>();

    /// <inheritdoc/>
    public virtual string SelectedItem
    {
        get => _listControlSupport.GetSelectedItems().FirstOrDefault();
        set => _listControlSupport.SetSelectedItem(value);
    }

    /// <inheritdoc/>
    public virtual int SelectedIndex
    {
        get
        {
            var result = _listControlSupport.GetSelectedIndices().FirstOrDefault();
            Expanded = false;
            return result;
        }
        set => _listControlSupport.SetSelectedIndex(value);
    }

    /// <inheritdoc/>
    public virtual string Text
    {
        get => TryGetPattern<ValuePattern>(out var valuePattern) ? valuePattern.Current.Value : SelectedItem;
        set
        {
            if (!IsPatternAvailable<ValuePattern>())
                throw new UIActionNotSupportedException("This combo box does not support direct text input. Use the SelectedItem property instead.", this);
            _valueControlSupport.SetValue(value);
        }
    }

    /// <inheritdoc/>
    public virtual bool Expanded
    {
        get => ExpandCollapsePattern.Current.ExpandCollapseState == ExpandCollapseState.Expanded;
        set => (value ? (Action)ExpandCollapsePattern.Expand : ExpandCollapsePattern.Collapse)();
    }

    /// <inheritdoc/>
    public virtual IEnumerable<IUITestControl> Items => _listControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

    partial void OnInitialized()
    {
        _valueControlSupport = new ValueControlSupport(this);
        _listControlSupport = new ListControlSupport(this, ByItem);
    }
}
