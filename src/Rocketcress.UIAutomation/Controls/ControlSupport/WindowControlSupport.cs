using Rocketcress.Core;
using Rocketcress.UIAutomation.ControlSearch;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace Rocketcress.UIAutomation.Controls.ControlSupport;

public class WindowControlSupport
{
    private UITestControl _control;

    public WindowControlSupport(UITestControl control)
    {
        _control = control;
    }

    public bool SetWindowSize(Size windowSize, bool moveCenter, bool assert)
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
        return true;
    }

    public void MoveToCenter()
    {
        SetWindowSize(new Size(_control.BoundingRectangle.Width, _control.BoundingRectangle.Height), true, true);
    }

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

    public void SetFocus(Action baseSetFocus)
    {
        baseSetFocus();
        Thread.Sleep(100);
        if (_control.Application != _control.Application.Context.ActiveApplication)
            _control.Application.Context.ActiveApplication = _control.Application;
    }

    public virtual bool Close(int timeout, bool assert)
    {
        WindowsApiHelper.CloseWindow(_control.WindowHandle);
        return _control.WaitUntilNotDisplayed(timeout, assert);
    }

    public virtual bool IsWindow()
    {
        var handle = _control.GetPropertyValueFailFast(AutomationElement.NativeWindowHandleProperty, false, 0);
        return handle != 0 && WindowsApiHelper.IsWindow(new IntPtr(handle));
    }

    public virtual bool IsWindowVisible()
    {
        var handle = _control.GetPropertyValueFailFast(AutomationElement.NativeWindowHandleProperty, false, 0);
        return handle != 0 && WindowsApiHelper.IsWindowVisible(new IntPtr(handle));
    }
}
