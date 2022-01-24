using Rocketcress.Core;
using Rocketcress.UIAutomation.ControlSearch;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace Rocketcress.UIAutomation.Controls.ControlSupport;

/// <summary>
/// Contains supporting code for window controls.
/// </summary>
public class WindowControlSupport
{
    private UITestControl _control;

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowControlSupport"/> class.
    /// </summary>
    /// <param name="control">The control to use.</param>
    public WindowControlSupport(UITestControl control)
    {
        _control = control;
    }

    /// <summary>
    /// Sets the size of the window.
    /// </summary>
    /// <param name="windowSize">Size of the window.</param>
    /// <param name="moveCenter">If set to <c>true</c> the window is moved to the center of the current screen.</param>
    public void SetWindowSize(Size windowSize, bool moveCenter)
    {
        int x = (int)_control.BoundingRectangle.Left;
        int y = (int)_control.BoundingRectangle.Top;
        if (moveCenter)
        {
            var screenSize = Screen.FromHandle(_control.WindowHandle).WorkingArea;
            x = (screenSize.Width - (int)windowSize.Width) / 2;
            y = (screenSize.Height - (int)windowSize.Height) / 2;
        }

        WindowsApiHelper.MoveWindow(_control.WindowHandle, x, y < 0 ? 0 : y, (int)windowSize.Width, (int)windowSize.Height, false);
    }

    /// <summary>
    /// Moves the window to the center of the current screen.
    /// </summary>
    public void MoveToCenter()
    {
        SetWindowSize(new Size(_control.BoundingRectangle.Width, _control.BoundingRectangle.Height), true);
    }

    /// <summary>
    /// Sets the window title.
    /// </summary>
    /// <param name="titleText">The title text.</param>
    public void SetWindowTitle(string titleText)
    {
        var hwnd = _control.WindowHandle;
        WindowsApiHelper.SetWindowText(hwnd, titleText);

        var propertyConditions = _control.LocationKey.ElementSearchPart.GetConditionList()
            .OfType<ControlSearch.Conditions.PropertyCondition>()
            .Where(x => x.Property == AutomationElement.NameProperty);
        foreach (var c in propertyConditions)
        {
            c.Value = titleText;
            c.Options = ByOptions.None;
        }
    }

    /// <summary>
    /// Focuses the window.
    /// </summary>
    /// <param name="baseSetFocus">The action that focuses the window.</param>
    public void SetFocus(Action baseSetFocus)
    {
        Guard.NotNull(baseSetFocus);
        baseSetFocus();
        Thread.Sleep(100);
        if (_control.Application != _control.Application.Context.ActiveApplication)
            _control.Application.Context.ActiveApplication = _control.Application;
    }

    /// <summary>
    /// Closes the window.
    /// </summary>
    /// <param name="timeout">The timeout.</param>
    /// <param name="assert">If set to <c>true</c> an error is thrown when the window does not close.</param>
    /// <returns><c>true</c> if the window closed; otherwise <c>false</c>.</returns>
    public virtual bool Close(int timeout, bool assert)
    {
        WindowsApiHelper.CloseWindow(_control.WindowHandle);
        return _control.WaitUntilNotDisplayed(timeout, assert);
    }

    /// <summary>
    /// Determines whether this instance is an actual window.
    /// </summary>
    /// <returns><c>true</c> if this instance is an actual window; otherwise, <c>false</c>.</returns>
    public virtual bool IsWindow()
    {
        var handle = _control.GetPropertyValueFailFast(AutomationElement.NativeWindowHandleProperty, false, 0);
        return handle != 0 && WindowsApiHelper.IsWindow(new IntPtr(handle));
    }

    /// <summary>
    /// Determines whether the window is visible.
    /// </summary>
    /// <returns><c>true</c> if the window is visible; otherwise, <c>false</c>.</returns>
    public virtual bool IsWindowVisible()
    {
        var handle = _control.GetPropertyValueFailFast(AutomationElement.NativeWindowHandleProperty, false, 0);
        return handle != 0 && WindowsApiHelper.IsWindowVisible(new IntPtr(handle));
    }
}
