namespace Rocketcress.UIAutomation.Controls.WinFormsControls;

/// <summary>
/// Represents a Windows Forms title bar control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WinFormsControls.WinControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestTitleBarControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WinTitleBar : WinControl, IUITestTitleBarControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.TitleBar);

    /// <summary>
    /// Gets the value pattern.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public ValuePattern ValuePattern => GetPattern<ValuePattern>();

    /// <inheritdoc/>
    public virtual string DisplayText => ValuePattern.Current.Value;
}
