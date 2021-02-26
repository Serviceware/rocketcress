using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using Rocketcress.UIAutomation.Exceptions;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfRadioButton : WpfControl, IUITestRadioButtonControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.RadioButton);

        #region Private Fields
        private SelectionItemControlSupport _selectionItemControlSupport;
        #endregion

        #region Patterns
        public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();
        #endregion

        #region Constructors
        public WpfRadioButton(By locationKey)
            : base(locationKey)
        {
        }

        public WpfRadioButton(IUITestControl parent)
            : base(parent)
        {
        }

        public WpfRadioButton(AutomationElement element)
            : base(element)
        {
        }

        public WpfRadioButton(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WpfRadioButton(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WpfRadioButton()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
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
        #endregion

        #region Public Properties
        public virtual IUITestControl Group
        {
            get
            {
                var element = _selectionItemControlSupport.GetSelectionContainer();
                return element == null ? null : ControlUtility.GetControl(element);
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
        #endregion
    }
}
