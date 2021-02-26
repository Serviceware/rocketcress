using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    public class WinEdit : WinControl, IUITestEditControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Edit);

        #region Private Fields
        private ValueControlSupport _valueControlSupport;
        #endregion

        #region Patterns
        public TextPattern TextPattern => GetPattern<TextPattern>();
        public ValuePattern ValuePattern => GetPattern<ValuePattern>();
        #endregion

        #region Constructors
        public WinEdit(By locationKey)
            : base(locationKey)
        {
        }

        public WinEdit(IUITestControl parent)
            : base(parent)
        {
        }

        public WinEdit(AutomationElement element)
            : base(element)
        {
        }

        public WinEdit(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WinEdit(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WinEdit()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            _valueControlSupport = new ValueControlSupport(this);
        }
        #endregion

        #region Public Properties
        public virtual string Text
        {
            get => ValuePattern.Current.Value;
            set => _valueControlSupport.SetValue(value);
        }

        public virtual bool ReadOnly => ValuePattern.Current.IsReadOnly;
        #endregion
    }
}
