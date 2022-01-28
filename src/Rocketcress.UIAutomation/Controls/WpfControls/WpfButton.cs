namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation button control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestButtonControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfButton : WpfControl, IUITestButtonControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Button);

    /// <summary>
    /// Gets the invoke pattern.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public InvokePattern InvokePattern => GetPattern<InvokePattern>();

    /// <inheritdoc/>
    public virtual string DisplayText => Name;

    /// <inheritdoc/>
    public virtual void Invoke() => InvokePattern.Invoke();
}
