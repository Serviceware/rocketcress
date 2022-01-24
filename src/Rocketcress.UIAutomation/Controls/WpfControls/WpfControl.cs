using Rocketcress.UIAutomation.Common;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.UITestControl" />
[AutoDetectControl(Priority = -100)]
[GenerateUIMapParts]
public partial class WpfControl : UITestControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndFramework(FrameworkIds.Wpf);

    /// <summary>
    /// Gets the synchronized input pattern.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public SynchronizedInputPattern SynchronizedInputPattern => GetPattern<SynchronizedInputPattern>();
}
