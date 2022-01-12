using Rocketcress.Core;
using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.CommonControls;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using Rocketcress.UIAutomation.Exceptions;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WinComboBox : WinControl, IUITestComboBoxControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ComboBox);

    private static readonly By ByItem = By.Scope(TreeScope.Children).AndControlType(ControlType.ListItem);
    private ValueControlSupport _valueControlSupport;
    private ListControlSupport _listControlSupport;

    [UIMapControl]
    private UITestControl ListControl { get; set; } = InitUsing<UITestControl>(() => By.ControlType(ControlType.List));

    [UIMapControl(IdStyle = IdStyle.Disabled)]
    private CommonButton ExpandButton { get; set; }

    public ExpandCollapsePattern ExpandCollapsePattern => GetPattern<ExpandCollapsePattern>();
    public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();
    public ScrollPattern ScrollPattern => GetPattern<ScrollPattern>();
    public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();
    public ValuePattern ValuePattern => GetPattern<ValuePattern>();

    public virtual string SelectedItem
    {
        get => _listControlSupport.GetSelectedItems().FirstOrDefault();
        set => _listControlSupport.SetSelectedItem(value);
    }

    public virtual int SelectedIndex
    {
        get
        {
            Expanded = true;
            var result = _listControlSupport.GetSelectedIndices().FirstOrDefault();
            Expanded = false;
            return result;
        }
        set
        {
            Expanded = true;
            var element = new UITestControl(Application, _listControlSupport.EnumerateItems().ElementAt(value));
            element.Click();
        }
    }

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

    public virtual bool Expanded
    {
        get => ListControl.Displayed;
        set
        {
            if (ListControl.Displayed != value)
            {
                ExpandButton.Click();
                Wait.Until(() => ListControl.Displayed == value).WithTimeout(2000).Start();
            }
        }
    }

    public virtual IEnumerable<IUITestControl> Items => _listControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

    partial void OnInitialized()
    {
        _valueControlSupport = new ValueControlSupport(this);
        _listControlSupport = new ListControlSupport(ListControl, ByItem);
    }
}
