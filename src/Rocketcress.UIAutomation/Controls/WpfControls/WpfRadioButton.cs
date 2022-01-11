using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using Rocketcress.UIAutomation.Exceptions;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    [GenerateUIMapParts]
    public partial class WpfRadioButton : WpfControl, IUITestRadioButtonControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.RadioButton);

        private SelectionItemControlSupport _selectionItemControlSupport;

        public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();

        public virtual IUITestControl Group
        {
            get
            {
                var element = _selectionItemControlSupport.GetSelectionContainer();
                return element == null ? null : ControlUtility.GetControl(Application, element);
            }
        }

        public virtual bool Selected
        {
            get => _selectionItemControlSupport.GetSelected();
            set
            {
                if (!value && Selected)
                    throw new UIActionNotSupportedException("RadioButtons cannot be unchecked.", this);
                _selectionItemControlSupport.SetSelected(value);
            }
        }

        partial void OnInitialized()
        {
            _selectionItemControlSupport = new SelectionItemControlSupport(this);
        }

        public void SetValue(object value)
        {
            if (!(bool)value)
                throw new System.NotImplementedException();
            if (!Selected)
                Selected = true;
        }

        public object GetValue()
        {
            return Selected;
        }
    }
}
