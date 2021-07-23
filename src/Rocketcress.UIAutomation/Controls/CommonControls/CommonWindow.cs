using Rocketcress.Core;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Windows;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.CommonControls
{
    [AutoDetectControl(Priority = -50)]
    public class CommonWindow : UITestControl, IUITestWindowControl
    {
        private Application _application;

        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Window);
        public override Application Application => _application ?? UIAutomationTestContext.CurrentContext.ActiveApplication;

        #region Private Fields

        private WindowControlSupport _windowControlSupport;

        #endregion

        #region Patterns

        public WindowPattern WindowPattern => GetPattern<WindowPattern>();

        #endregion

        #region Constructors

        public CommonWindow(AutomationElement element)
            : base(element)
        {
        }

        public CommonWindow(Application app)
            : this(By.Empty, app)
        {
        }

        public CommonWindow(By locationKey)
            : base(locationKey)
        {
        }

        public CommonWindow(IUITestControl parent)
            : base(parent)
        {
        }

        public CommonWindow(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public CommonWindow(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        public CommonWindow(By locationKey, Application app)
            : base(locationKey)
        {
            _application = app;
            if (app != null)
                LocationKey.Append(By.ProcessId(app.Process.Id), false, false);
        }

        protected CommonWindow()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            _windowControlSupport = new WindowControlSupport(this);
        }

        #endregion

        #region Public Properties

        public virtual bool Maximized
        {
            get => WindowPattern.Current.WindowVisualState == WindowVisualState.Maximized;
            set => WindowPattern.SetWindowVisualState(value ? WindowVisualState.Maximized : WindowVisualState.Normal);
        }

        public override bool Exists => base.Exists && _windowControlSupport.IsWindow();
        public override bool Displayed => Exists && _windowControlSupport.IsWindowVisible();

        #endregion

        #region Public Methods

        public virtual bool SetWindowSize(Size windowSize) => SetWindowSize(windowSize, true, true);
        public virtual bool SetWindowSize(Size windowSize, bool moveCenter) => SetWindowSize(windowSize, moveCenter, true);
        public virtual bool SetWindowSize(Size windowSize, bool moveCenter, bool assert) => _windowControlSupport.SetWindowSize(windowSize, moveCenter, assert);

        public virtual void MoveToCenter() => _windowControlSupport.MoveToCenter();

        public virtual void SetWindowTitle(string titleText) => _windowControlSupport.SetWindowTitle(titleText);

        public virtual bool Close() => Close(Wait.DefaultOptions.TimeoutMs, true);
        public virtual bool Close(int timeout) => Close(timeout, true);
        public virtual bool Close(bool assert) => Close(Wait.DefaultOptions.TimeoutMs, assert);
        public virtual bool Close(int timeout, bool assert) => _windowControlSupport.Close(timeout, assert);

        public override void SetFocus() => _windowControlSupport.SetFocus(base.SetFocus);

        #endregion
    }
}
