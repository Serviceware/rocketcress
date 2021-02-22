using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfScrollBar : WpfControl, IUITestScrollBarControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ScrollBar);

        #region Patterns
        public RangeValuePattern RangeValuePattern => GetPattern<RangeValuePattern>();
        #endregion

        #region Constructors
        public WpfScrollBar(By locationKey) : base(locationKey) { }
        public WpfScrollBar(IUITestControl parent) : base(parent) { }
        public WpfScrollBar(AutomationElement element) : base(element) { }
        public WpfScrollBar(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WpfScrollBar(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WpfScrollBar() { }
        #endregion

        #region Public Properties
        public virtual double MaximumPosition => RangeValuePattern.Current.Maximum;
        public virtual double MinimumPosition => RangeValuePattern.Current.Minimum;
        public virtual OrientationType Orientation => AutomationElement.Current.Orientation;
        public virtual double Position
        {
            get => RangeValuePattern.Current.Value;
            set => RangeValuePattern.SetValue(value);
        }
        #endregion
    }
}
