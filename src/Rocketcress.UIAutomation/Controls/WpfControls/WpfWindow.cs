using Rocketcress.Core;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Windows;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfWindow : WpfControl, IUITestWindowControl
{
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Window);

    private WindowControlSupport _windowControlSupport;

    public WindowPattern WindowPattern => GetPattern<WindowPattern>();

    public WpfWindow(Application app)
        : this(app, By.Empty)
    {
    }

    public WpfWindow(Application app, By locationKey)
        : base(app, locationKey)
    {
        LocationKey.Append(By.ProcessId(app.Process.Id), false, false);
    }

    public virtual bool Maximized
    {
        get => WindowPattern.Current.WindowVisualState == WindowVisualState.Maximized;
        set => WindowPattern.SetWindowVisualState(value ? WindowVisualState.Maximized : WindowVisualState.Normal);
    }

    public override bool Exists => base.Exists && _windowControlSupport.IsWindow();
    public override bool Displayed => Exists && _windowControlSupport.IsWindowVisible();

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

    partial void OnInitialized()
    {
        _windowControlSupport = new WindowControlSupport(this);
    }
}
