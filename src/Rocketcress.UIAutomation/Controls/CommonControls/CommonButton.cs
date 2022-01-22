using Rocketcress.Core.Extensions;
using System.Windows;

namespace Rocketcress.UIAutomation.Controls.CommonControls;

/// <summary>
/// Represents a common button control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.UITestControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestButtonControl" />
[AutoDetectControl(Priority = -50)]
[GenerateUIMapParts]
public partial class CommonButton : UITestControl, IUITestButtonControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Button);

    /// <summary>
    /// Gets the invoke pattern.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public InvokePattern InvokePattern => GetPattern<InvokePattern>();

    /// <inheritdoc/>
    public override Point ClickablePoint => BoundingRectangle.GetAbsoluteCenter();

    /// <inheritdoc/>
    public string DisplayText => Name;

    /// <inheritdoc/>
    public void Invoke() => InvokePattern.Invoke();
}
