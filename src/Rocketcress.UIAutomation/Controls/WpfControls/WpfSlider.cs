using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfSlider : WpfControl, IUITestSliderControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Slider);

        #region Patterns
        public RangeValuePattern RangeValuePattern => GetPattern<RangeValuePattern>();
        #endregion

        #region Construcotrs
        public WpfSlider(By locationKey) : base(locationKey) { }
        public WpfSlider(IUITestControl parent) : base(parent) { }
        public WpfSlider(AutomationElement element) : base(element) { }
        public WpfSlider(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WpfSlider(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WpfSlider() { }
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
        public virtual double SmallChange => RangeValuePattern.Current.SmallChange;
        public virtual double LargeChange => RangeValuePattern.Current.LargeChange;
        #endregion
    }
}
