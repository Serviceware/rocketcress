using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls;

/// <summary>
/// Represents a Windows Forms edit control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WinFormsControls.WinControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestEditControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WinEdit : WinControl, IUITestEditControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Edit);

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Base Location Key should be on top.")]
    private ValueControlSupport _valueControlSupport;

    /// <summary>
    /// Gets the text pattern.
    /// </summary>
    public TextPattern TextPattern => GetPattern<TextPattern>();

    /// <summary>
    /// Gets the value pattern.
    /// </summary>
    public ValuePattern ValuePattern => GetPattern<ValuePattern>();

    /// <inheritdoc/>
    public virtual string Text
    {
        get => ValuePattern.Current.Value;
        set => _valueControlSupport.SetValue(value);
    }

    /// <inheritdoc/>
    public virtual bool ReadOnly => ValuePattern.Current.IsReadOnly;

    partial void OnInitialized()
    {
        _valueControlSupport = new ValueControlSupport(this);
    }
}
