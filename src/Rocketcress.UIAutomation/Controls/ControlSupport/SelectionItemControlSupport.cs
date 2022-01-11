using Rocketcress.Core;
using Rocketcress.UIAutomation.Exceptions;

namespace Rocketcress.UIAutomation.Controls.ControlSupport
{
    public class SelectionItemControlSupport
    {
        private readonly UITestControl _control;

        public SelectionItemControlSupport(UITestControl control)
        {
            _control = control;
        }

        public bool GetSelected() => GetSelected(GetSelectionItemPattern());
        public bool GetSelected(SelectionItemPattern selectionItemPattern)
        {
            return selectionItemPattern.Current.IsSelected;
        }

        public void SetSelected(bool value)
        {
            var selectionItemPattern = GetSelectionItemPattern();

            if (value != GetSelected(selectionItemPattern))
            {
                _control.Click();
                if (!Wait.Until(() => value == GetSelected(selectionItemPattern)).WithTimeout(UITestControl.ShortControlActionTimeout).WithTimeGap(0).Start().Value)
                {
                    _control.LogWarning("Selected could not be set via control click. Setting property via pattern now.");
                    selectionItemPattern.Select();
                    if (!value)
                        selectionItemPattern.RemoveFromSelection();
                    if (value != GetSelected(selectionItemPattern))
                        throw new UIAutomationControlException("Selected has not been set correctly.", _control);
                }
            }
        }

        public AutomationElement GetSelectionContainer()
        {
            return GetSelectionItemPattern().Current.SelectionContainer;
        }

        private SelectionItemPattern GetSelectionItemPattern()
        {
            if (!_control.TryGetPattern<SelectionItemPattern>(out var selectionItemPattern))
                throw new UIActionNotSupportedException("The control needs to support the SelectionItemPattern.", _control);
            return selectionItemPattern;
        }
    }
}
