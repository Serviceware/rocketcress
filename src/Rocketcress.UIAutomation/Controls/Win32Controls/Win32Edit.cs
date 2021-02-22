using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.Win32Controls
{
    [AutoDetectControl]
    public class Win32Edit : Win32Control, IUITestEditControl
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
        public Win32Edit(By locationKey) : base(locationKey) { }
        public Win32Edit(IUITestControl parent) : base(parent) { }
        public Win32Edit(AutomationElement element) : base(element) { }
        public Win32Edit(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public Win32Edit(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected Win32Edit() { }
        
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
