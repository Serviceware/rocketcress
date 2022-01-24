namespace Rocketcress.UIAutomation.Controls.Win32Controls;

/// <summary>
/// Represents a Win32 text control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.Win32Controls.Win32Control" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestTextControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class Win32Text : Win32Control, IUITestTextControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Text);

    /// <inheritdoc/>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public virtual string DisplayText => Name;

    /// <inheritdoc/>
    public virtual string Text => Name;
}
