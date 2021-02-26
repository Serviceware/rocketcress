using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfProgressBar : WpfControl, IUITestProgressBarControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ProgressBar);

        #region Patterns
        public RangeValuePattern RangeValuePattern => GetPattern<RangeValuePattern>();
        #endregion

        #region Constructors
        public WpfProgressBar(By locationKey)
            : base(locationKey)
        {
        }

        public WpfProgressBar(IUITestControl parent)
            : base(parent)
        {
        }

        public WpfProgressBar(AutomationElement element)
            : base(element)
        {
        }

        public WpfProgressBar(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WpfProgressBar(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WpfProgressBar()
        {
        }
        #endregion

        #region Public Properties
        public virtual double MinimumValue => RangeValuePattern.Current.Minimum;
        public virtual double MaximumValue => RangeValuePattern.Current.Maximum;
        public virtual double Position => RangeValuePattern.Current.Value;
        #endregion
    }
}
