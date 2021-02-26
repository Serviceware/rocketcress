using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfTabPage : WpfControl, IUITestTabPageControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.TabItem);

        #region Patterns
        public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();
        #endregion

        #region Constructors
        public WpfTabPage(By locationKey)
            : base(locationKey)
        {
        }

        public WpfTabPage(IUITestControl parent)
            : base(parent)
        {
        }

        public WpfTabPage(AutomationElement element)
            : base(element)
        {
        }

        public WpfTabPage(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WpfTabPage(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WpfTabPage()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            Header = new UITestControl(By.Skip(0), this);
        }
        #endregion

        #region Public Properties
        public virtual IUITestControl Header { get; protected set; }
        #endregion
    }
}
