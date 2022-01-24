using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation edit control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestEditControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfEdit : WpfControl, IUITestEditControl, IValueAccessor
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

    /// <inheritdoc/>
    public void SetValue(object value)
    {
        Text = (string)value;
    }

    /// <inheritdoc/>
    public object GetValue()
    {
        return Text;
    }

    partial void OnInitialized()
    {
        _valueControlSupport = new ValueControlSupport(this);
    }
}
