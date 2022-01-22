namespace Rocketcress.UIAutomation.Controls.CommonControls;

/// <summary>
/// Represents a common text control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.UITestControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestTextControl" />
[AutoDetectControl(Priority = -50)]
[GenerateUIMapParts]
public partial class CommonText : UITestControl, IUITestTextControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Text);

    /// <inheritdoc/>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public virtual string DisplayText => Name;

    /// <inheritdoc/>
    public virtual string Text => Name;
}
