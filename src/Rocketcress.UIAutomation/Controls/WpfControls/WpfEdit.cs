using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfEdit : WpfControl, IUITestEditControl
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
        public WpfEdit(By locationKey) : base(locationKey) { }
        public WpfEdit(IUITestControl parent) : base(parent) { }
        public WpfEdit(AutomationElement element) : base(element) { }
        public WpfEdit(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WpfEdit(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WpfEdit() { }
        
        protected override void Initialize()
        {
            base.Initialize();
            _valueControlSupport = new ValueControlSupport(this);
        }

        public void SetValue(object value)
        {
            Text = (string)value;
        }
        public object GetValue()
        {
            return Text;
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
