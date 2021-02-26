using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    public class WinTitleBar : WinControl, IUITestTitleBarControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.TitleBar);

        #region Patterns
        public ValuePattern ValuePattern => GetPattern<ValuePattern>();
        #endregion

        #region Construcotrs
        public WinTitleBar(By locationKey)
            : base(locationKey)
        {
        }

        public WinTitleBar(IUITestControl parent)
            : base(parent)
        {
        }

        public WinTitleBar(AutomationElement element)
            : base(element)
        {
        }

        public WinTitleBar(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WinTitleBar(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WinTitleBar()
        {
        }
        #endregion

        #region Public Properties
        public virtual string DisplayText => ValuePattern.Current.Value;
        #endregion
    }
}
