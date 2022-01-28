using Rocketcress.Core.Extensions;
using System.Windows;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls;

/// <summary>
/// Represents a Windows Forms button control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WinFormsControls.WinControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestButtonControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WinButton : WinControl, IUITestButtonControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Button);

    /// <summary>
    /// Gets the invoke pattern.
    /// </summary>
    /// <value>
    /// The invoke pattern.
    /// </value>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public InvokePattern InvokePattern => GetPattern<InvokePattern>();

    /// <inheritdoc/>
    public override Point ClickablePoint => BoundingRectangle.GetAbsoluteCenter();

    /// <inheritdoc/>
    public string DisplayText => Name;

    /// <inheritdoc/>
    public void Invoke() => InvokePattern.Invoke();
}
