using Rocketcress.Core.Extensions;
using System.Windows;

namespace Rocketcress.UIAutomation.Controls.Win32Controls;

/// <summary>
/// Represents a Win32 button.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.Win32Controls.Win32Control" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestButtonControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class Win32Button : Win32Control, IUITestButtonControl
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
