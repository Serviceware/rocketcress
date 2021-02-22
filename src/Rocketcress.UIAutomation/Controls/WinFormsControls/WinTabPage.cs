using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    public class WinTabPage : WinControl, IUITestTabPageControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.TabItem);

        #region Patterns
        public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();
        #endregion

        #region Constructors
        public WinTabPage(By locationKey) : base(locationKey) { }
        public WinTabPage(IUITestControl parent) : base(parent) { }
        public WinTabPage(AutomationElement element) : base(element) { }
        public WinTabPage(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WinTabPage(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WinTabPage() { }

        protected override void Initialize()
        {
            base.Initialize();
        }
        #endregion

        IUITestControl IUITestTabPageControl.Header => throw new System.NotImplementedException();
    }
}
