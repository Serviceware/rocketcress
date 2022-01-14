using Rocketcress.Core;
using Rocketcress.UIAutomation.Exceptions;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfExpander : WpfControl, IUITestExpanderControl
{
    protected override By BaseLocationKey => base.BaseLocationKey
        .AndControlType(ControlType.Group)
        .AndPatternAvailable<ExpandCollapsePattern>();

    public ExpandCollapsePattern ExpandCollapsePattern => GetPattern<ExpandCollapsePattern>();

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
