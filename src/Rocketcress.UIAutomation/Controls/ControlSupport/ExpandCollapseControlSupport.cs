using Rocketcress.Core;
using Rocketcress.UIAutomation.Exceptions;
using System;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.ControlSupport
{
    public class ExpandCollapseControlSupport
    {
        private readonly UITestControl _control;

        public ExpandCollapseControlSupport(UITestControl control)
        {
            _control = control;
        }

        public bool GetExpanded()
        {
            if (!_control.TryGetPattern<ExpandCollapsePattern>(out var expandCollapsePattern))
                throw new UIActionNotSupportedException("This control does not support expanding/collapsing (ExpandCollapsePattern not available)", _control);
            return GetExpanded(expandCollapsePattern);
        }

        private bool GetExpanded(ExpandCollapsePattern expandCollapsePattern)
        {
            return expandCollapsePattern.Current.ExpandCollapseState == ExpandCollapseState.Expanded;
        }

        public void SetExpanded(bool value)
        {
            if (!_control.TryGetPattern<ExpandCollapsePattern>(out var expandCollapsePattern))
                throw new UIActionNotSupportedException("This control does not support expanding/collapsing (ExpandCollapsePattern not available)", _control);
            if (GetExpanded(expandCollapsePattern) != value)
            {
                _control.Click();
                if (!Waiter.WaitUntil(() => value == GetExpanded(expandCollapsePattern), UITestControl.ShortControlActionTimeout, 0))
                {
                    _control.LogWarning($"Expanded was not set from click on the control. State is now set with the automation pattern.");
                    (value ? (Action)expandCollapsePattern.Expand : expandCollapsePattern.Collapse).Invoke();
                    if (value != GetExpanded(expandCollapsePattern))
                        throw new Exception("GetExpanded has not been correctly changed.");
                }
            }
        }
    }
}
