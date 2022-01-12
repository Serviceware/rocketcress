using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WinEdit : WinControl, IUITestEditControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Edit);

    private ValueControlSupport _valueControlSupport;

    public TextPattern TextPattern => GetPattern<TextPattern>();
    public ValuePattern ValuePattern => GetPattern<ValuePattern>();

    public virtual string Text
    {
        get => ValuePattern.Current.Value;
        set => _valueControlSupport.SetValue(value);
    }

    public virtual bool ReadOnly => ValuePattern.Current.IsReadOnly;

    partial void OnInitialized()
    {
        _valueControlSupport = new ValueControlSupport(this);
    }
}
