using Rocketcress.Core;
using Rocketcress.UIAutomation.Exceptions;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation expander control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestExpanderControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfExpander : WpfControl, IUITestExpanderControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey
        .AndControlType(ControlType.Group)
        .AndPatternAvailable<ExpandCollapsePattern>();

    /// <summary>
    /// Gets the expand collapse pattern.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Base Location Key should be on top.")]
    public ExpandCollapsePattern ExpandCollapsePattern => GetPattern<ExpandCollapsePattern>();

    /// <inheritdoc/>
    public virtual bool Expanded
    {
        get => ExpandCollapsePattern.Current.ExpandCollapseState == ExpandCollapseState.Expanded;
        set
        {
            if (Expanded != value)
            {
                Click();
                if (!Wait.Until(() => value == Expanded).WithTimeout(5000).WithTimeGap(0).Start().Value)
                {
                    LogWarning("Expand state was not set correctly by clicking the control. The state is now set via the pattern.");
                    if (value)
                        ExpandCollapsePattern.Expand();
                    else
                        ExpandCollapsePattern.Collapse();
                    if (value != Expanded)
                        throw new UIAutomationControlException("The expand state has not been set correctly.", this);
                }
            }
        }
    }
}
