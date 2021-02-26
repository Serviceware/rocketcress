using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.CommonControls
{
    [AutoDetectControl(Priority = -50)]
    public class CommonTitleBar : UITestControl, IUITestTitleBarControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.TitleBar);

        #region Patterns

        public ValuePattern ValuePattern => GetPattern<ValuePattern>();

        #endregion

        #region Constructors

        public CommonTitleBar(By locationKey)
            : base(locationKey)
        {
        }

        public CommonTitleBar(IUITestControl parent)
            : base(parent)
        {
        }

        public CommonTitleBar(AutomationElement element)
            : base(element)
        {
        }

        public CommonTitleBar(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public CommonTitleBar(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected CommonTitleBar()
        {
        }

        #endregion

        #region Public Properties

        public virtual string DisplayText => ValuePattern.Current.Value;

        #endregion
    }
}
