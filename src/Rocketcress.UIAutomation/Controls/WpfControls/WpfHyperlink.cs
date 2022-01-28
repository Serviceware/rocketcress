namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation hyperlink control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestHyperlinkControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfHyperlink : WpfControl, IUITestHyperlinkControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Hyperlink);

    /// <summary>
    /// Gets the invoke pattern.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public InvokePattern InvokePattern => GetPattern<InvokePattern>();

    /// <inheritdoc/>
    public virtual string Alt => Name;

    /// <inheritdoc/>
    public virtual void Invoke() => InvokePattern.Invoke();
}
