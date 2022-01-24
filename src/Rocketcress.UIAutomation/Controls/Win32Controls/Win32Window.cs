using Rocketcress.Core;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Windows;

namespace Rocketcress.UIAutomation.Controls.Win32Controls;

/// <summary>
/// Represents a Win32 window control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.Win32Controls.Win32Control" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestWindowControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class Win32Window : Win32Control, IUITestWindowControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Window);

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Base Location Key should be on top.")]
    private WindowControlSupport _windowControlSupport;

    /// <summary>
    /// Initializes a new instance of the <see cref="Win32Window"/> class.
    /// </summary>
    /// <param name="app">The application this window is part of.</param>
    public Win32Window(Application app)
        : this(app, By.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Win32Window"/> class.
    /// </summary>
    /// <param name="app">The application this window is part of.</param>
    /// <param name="locationKey">The location key that is used to.</param>
    public Win32Window(Application app, By locationKey)
        : base(app, locationKey)
    {
        Guard.NotNull(app);
        LocationKey.Append(By.ProcessId(app.Process.Id), false, false);
    }

    /// <summary>
    /// Gets the window pattern.
    /// </summary>
    public WindowPattern WindowPattern => GetPattern<WindowPattern>();

    /// <inheritdoc/>
    public virtual bool Maximized
    {
        get => WindowPattern.Current.WindowVisualState == WindowVisualState.Maximized;
        set => WindowPattern.SetWindowVisualState(value ? WindowVisualState.Maximized : WindowVisualState.Normal);
    }

    /// <inheritdoc/>
    public override bool Exists => base.Exists && _windowControlSupport.IsWindow();

    /// <inheritdoc/>
    public override bool Displayed => Exists && _windowControlSupport.IsWindowVisible();

    /// <inheritdoc/>
    public virtual void SetWindowSize(Size windowSize) => _windowControlSupport.SetWindowSize(windowSize, true);

    /// <inheritdoc/>
    public virtual void SetWindowSize(Size windowSize, bool moveCenter) => _windowControlSupport.SetWindowSize(windowSize, moveCenter);

    /// <inheritdoc/>
    public virtual void MoveToCenter() => _windowControlSupport.MoveToCenter();

    /// <inheritdoc/>
    public virtual void SetWindowTitle(string titleText) => _windowControlSupport.SetWindowTitle(titleText);

    /// <inheritdoc/>
    public virtual bool Close() => Close(Wait.DefaultOptions.TimeoutMs, true);

    /// <inheritdoc/>
    public virtual bool Close(int timeout) => Close(timeout, true);

    /// <inheritdoc/>
    public virtual bool Close(bool assert) => Close(Wait.DefaultOptions.TimeoutMs, assert);

    /// <inheritdoc/>
    public virtual bool Close(int timeout, bool assert) => _windowControlSupport.Close(timeout, assert);

    /// <inheritdoc/>
    public override void SetFocus() => _windowControlSupport.SetFocus(base.SetFocus);

    partial void OnInitialized()
    {
        _windowControlSupport = new WindowControlSupport(this);
    }
}
