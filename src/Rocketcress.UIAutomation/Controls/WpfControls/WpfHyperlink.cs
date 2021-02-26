using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfHyperlink : WpfControl, IUITestHyperlinkControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Hyperlink);

        #region Patterns
        public InvokePattern InvokePattern => GetPattern<InvokePattern>();
        #endregion

        #region Constructors
        public WpfHyperlink(By locationKey)
            : base(locationKey)
        {
        }

        public WpfHyperlink(IUITestControl parent)
            : base(parent)
        {
        }

        public WpfHyperlink(AutomationElement element)
            : base(element)
        {
        }

        public WpfHyperlink(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WpfHyperlink(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WpfHyperlink()
        {
        }
        #endregion

        #region Public Properties
        public virtual string Alt => Name;
        #endregion

        #region Public Methods
        public virtual void Invoke() => InvokePattern.Invoke();
        #endregion
    }
}
