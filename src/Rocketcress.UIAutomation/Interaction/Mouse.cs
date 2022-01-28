using Rocketcress.Core;
using Rocketcress.Core.WindowsApiTypes;
using Rocketcress.UIAutomation.Controls;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Cursor = System.Windows.Forms.Cursor;

namespace Rocketcress.UIAutomation.Interaction;

/// <summary>
/// Manages Mouse actions.
/// </summary>
public static class Mouse
{
    /// <summary>
    /// Gets or sets a value indicating whether mouse actions wait until the control is ready.
    /// </summary>
    public static bool IsWaitForControlReadyEnabled { get; set; } = true;

    /// <summary>
    /// Clicks at the current mouse position.
    /// </summary>
    public static void Click() => ClickImplementation(null, MouseButtons.Left, ModifierKeys.None, null);

    /// <summary>
    /// Clicks the specified control.
    /// </summary>
    /// <param name="control">The control to click.</param>
    public static void Click(IUITestControl control) => ClickImplementation(control, MouseButtons.Left, ModifierKeys.None, null);

    /// <summary>
    /// Clicks at the specified point.
    /// </summary>
    /// <param name="absolutePoint">The absoluet point to click.</param>
    public static void Click(Point absolutePoint) => ClickImplementation(null, MouseButtons.Left, ModifierKeys.None, absolutePoint);

    /// <summary>
    /// Clicks at the current mouse position.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    public static void Click(MouseButtons button) => ClickImplementation(null, button, ModifierKeys.None, null);

    /// <summary>
    /// Clicks at the current mouse position.
    /// </summary>
    /// <param name="modifierKeys">The modifier keys to press during the click.</param>
    public static void Click(ModifierKeys modifierKeys) => ClickImplementation(null, MouseButtons.Left, modifierKeys, null);

    /// <summary>
    /// Clicks at the current mouse position.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="modifierKeys">The modifier keys to press during the click.</param>
    public static void Click(MouseButtons button, ModifierKeys modifierKeys) => ClickImplementation(null, button, modifierKeys, null);

    /// <summary>
    /// Clicks at the specified point.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="modifierKeys">The modifier keys to press during the click.</param>
    /// <param name="absolutePoint">The absoluet point to click.</param>
    public static void Click(MouseButtons button, ModifierKeys modifierKeys, Point absolutePoint) => ClickImplementation(null, button, modifierKeys, absolutePoint);

    /// <summary>
    /// Clicks the specified control.
    /// </summary>
    /// <param name="control">The control to click.</param>
    /// <param name="relativePoint">The point relative to the <paramref name="control"/>.</param>
    public static void Click(IUITestControl control, Point relativePoint) => ClickImplementation(control, MouseButtons.Left, ModifierKeys.None, relativePoint);

    /// <summary>
    /// Clicks the specified control.
    /// </summary>
    /// <param name="control">The control to click.</param>
    /// <param name="modifierKeys">The modifier keys to press during the click.</param>
    public static void Click(IUITestControl control, ModifierKeys modifierKeys) => ClickImplementation(control, MouseButtons.Left, modifierKeys, null);

    /// <summary>
    /// Clicks the specified control.
    /// </summary>
    /// <param name="control">The control to click.</param>
    /// <param name="button">The mouse button to press.</param>
    public static void Click(IUITestControl control, MouseButtons button) => ClickImplementation(control, button, ModifierKeys.None, null);

    /// <summary>
    /// Clicks the specified control.
    /// </summary>
    /// <param name="control">The control to click.</param>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="modifierKeys">The modifier keys to press during the click.</param>
    public static void Click(IUITestControl control, MouseButtons button, ModifierKeys modifierKeys) => ClickImplementation(control, button, modifierKeys, null);

    /// <summary>
    /// Clicks the specified control.
    /// </summary>
    /// <param name="control">The control to click.</param>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="modifierKeys">The modifier keys to press during the click.</param>
    /// <param name="relativePoint">The point relative to the <paramref name="control"/>.</param>
    public static void Click(IUITestControl control, MouseButtons button, ModifierKeys modifierKeys, Point relativePoint) => ClickImplementation(control, button, modifierKeys, relativePoint);

    /// <summary>
    /// Double-Clicks at the current mouse position.
    /// </summary>
    public static void DoubleClick() => DoubleClickImplementation(null, MouseButtons.Left, ModifierKeys.None, null);

    /// <summary>
    /// Double-Clicks the specified control.
    /// </summary>
    /// <param name="control">The control to click.</param>
    public static void DoubleClick(IUITestControl control) => DoubleClickImplementation(control, MouseButtons.Left, ModifierKeys.None, null);

    /// <summary>
    /// Double-Clicks at the specified point.
    /// </summary>
    /// <param name="absolutePoint">The absoluet point to click.</param>
    public static void DoubleClick(Point absolutePoint) => DoubleClickImplementation(null, MouseButtons.Left, ModifierKeys.None, absolutePoint);

    /// <summary>
    /// Double-Clicks at the current mouse position.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    public static void DoubleClick(MouseButtons button) => DoubleClickImplementation(null, button, ModifierKeys.None, null);

    /// <summary>
    /// Double-Clicks at the current mouse position.
    /// </summary>
    /// <param name="modifierKeys">The modifier keys to press during the click.</param>
    public static void DoubleClick(ModifierKeys modifierKeys) => DoubleClickImplementation(null, MouseButtons.Left, modifierKeys, null);

    /// <summary>
    /// Double-Clicks the specified control.
    /// </summary>
    /// <param name="control">The control to click.</param>
    /// <param name="relativePoint">The point relative to the <paramref name="control"/>.</param>
    public static void DoubleClick(IUITestControl control, Point relativePoint) => DoubleClickImplementation(control, MouseButtons.Left, ModifierKeys.None, relativePoint);

    /// <summary>
    /// Double-Clicks the specified control.
    /// </summary>
    /// <param name="control">The control to click.</param>
    /// <param name="modifierKeys">The modifier keys to press during the click.</param>
    public static void DoubleClick(IUITestControl control, ModifierKeys modifierKeys) => DoubleClickImplementation(control, MouseButtons.Left, modifierKeys, null);

    /// <summary>
    /// Double-Clicks at the specified point.
    /// </summary>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="modifierKeys">The modifier keys to press during the click.</param>
    /// <param name="absolutePoint">The absoluet point to click.</param>
    public static void DoubleClick(MouseButtons button, ModifierKeys modifierKeys, Point absolutePoint) => DoubleClickImplementation(null, button, modifierKeys, absolutePoint);

    /// <summary>
    /// Double-Clicks the specified control.
    /// </summary>
    /// <param name="control">The control to click.</param>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="modifierKeys">The modifier keys to press during the click.</param>
    /// <param name="relativePoint">The point relative to the <paramref name="control"/>.</param>
    public static void DoubleClick(IUITestControl control, MouseButtons button, ModifierKeys modifierKeys, Point relativePoint) => DoubleClickImplementation(control, button, modifierKeys, relativePoint);

    /// <summary>
    /// Moves the mouse to the specified control.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="relativePoint">The relative point.</param>
    public static void Hover(IUITestControl control, Point relativePoint) => HoverImplementation(control, relativePoint, 0);

    /// <summary>
    /// Moves the mouse to the specified control.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="relativePoint">The relative point.</param>
    /// <param name="duration">The duration in miliseconds.</param>
    public static void Hover(IUITestControl control, Point relativePoint, int duration) => HoverImplementation(control, relativePoint, duration);

    /// <summary>
    /// Moves the mouse to the specified point.
    /// </summary>
    /// <param name="absolutePoint">The absolute point.</param>
    public static void Hover(Point absolutePoint) => HoverImplementation(null, absolutePoint, 0);

    /// <summary>
    /// Moves the mouse to the specified point.
    /// </summary>
    /// <param name="absolutePoint">The absolute point.</param>
    /// <param name="duration">The duration.</param>
    public static void Hover(Point absolutePoint, int duration) => HoverImplementation(null, absolutePoint, duration);

    /// <summary>
    /// Moves the mouse to the specified control.
    /// </summary>
    /// <param name="control">The control.</param>
    public static void Hover(IUITestControl control) => HoverImplementation(control, null, 0);

    /// <summary>
    /// Moves the mouse to the specified control.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="duration">The duration.</param>
    public static void Hover(IUITestControl control, int duration) => HoverImplementation(control, null, duration);

    /// <summary>
    /// Starts dragging the mouse from the current cursor position.
    /// </summary>
    public static void StartDragging() => StartDraggingImplementation(null, MouseButtons.Left, ModifierKeys.None, null);

    /// <summary>
    /// Starts dragging the mouse from the specified control.
    /// </summary>
    /// <param name="control">The control.</param>
    public static void StartDragging(IUITestControl control) => StartDraggingImplementation(control, MouseButtons.Left, ModifierKeys.None, null);

    /// <summary>
    /// Starts dragging the mouse from the specified control.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="relativePoint">The relative point.</param>
    public static void StartDragging(IUITestControl control, Point relativePoint) => StartDraggingImplementation(control, MouseButtons.Left, ModifierKeys.None, relativePoint);

    /// <summary>
    /// Starts dragging the mouse from the specified control.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="button">The mouse button to press.</param>
    public static void StartDragging(IUITestControl control, MouseButtons button) => StartDraggingImplementation(control, button, ModifierKeys.None, null);

    /// <summary>
    /// Starts dragging the mouse from the specified control.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="relativePoint">The relative point.</param>
    /// <param name="button">The mouse button to press.</param>
    /// <param name="modifierKeys">The modifier keys to press.</param>
    public static void StartDragging(IUITestControl control, Point relativePoint, MouseButtons button, ModifierKeys modifierKeys) => StartDraggingImplementation(control, button, modifierKeys, relativePoint);

    /// <summary>
    /// Stops dragging to the specified control.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="moveByX">The relative x position.</param>
    /// <param name="moveByY">The relative y position.</param>
    public static void StopDragging(IUITestControl control, int moveByX, int moveByY) => StopDraggingImplementation(control, new Point(moveByX, moveByY), true);

    /// <summary>
    /// Stops dragging to the specified control.
    /// </summary>
    /// <param name="control">The control.</param>
    public static void StopDragging(IUITestControl control) => StopDraggingImplementation(control, null, false);

    /// <summary>
    /// Stops dragging to the specified point.
    /// </summary>
    /// <param name="absolutePoint">The absolute point.</param>
    public static void StopDragging(Point absolutePoint) => StopDraggingImplementation(null, absolutePoint, false);

    /// <summary>
    /// Stops dragging to the specified control.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="relativePoint">The relative point.</param>
    public static void StopDragging(IUITestControl control, Point relativePoint) => StopDraggingImplementation(control, relativePoint, false);

    /// <summary>
    /// Stops dragging to the specified point.
    /// </summary>
    /// <param name="moveByX">The absolute x position.</param>
    /// <param name="moveByY">The absolute y position.</param>
    public static void StopDragging(int moveByX, int moveByY) => StopDraggingImplementation(null, new Point(moveByX, moveByY), true);

    private static void ClickImplementation(IUITestControl control, MouseButtons button, ModifierKeys modifierKeys, Point? point)
    {
        var (dx, dy) = GetCoordinates(control, point, false);
        var eventFlags = MouseEventFlags.Absolute | MouseEventFlags.Move | GetFlagsForButton(button, true, true);

        Keyboard.PressModifierKeys(modifierKeys);
        WindowsApiHelper.mouse_event(eventFlags, dx, dy, 0, UIntPtr.Zero);
        if (IsWaitForControlReadyEnabled)
            control?.WaitForControlReady();
        Keyboard.ReleaseModifierKeys(modifierKeys);
    }

    private static void DoubleClickImplementation(IUITestControl control, MouseButtons button, ModifierKeys modifierKeys, Point? point)
    {
        var (dx, dy) = GetCoordinates(control, point, false);
        var eventFlags = MouseEventFlags.Absolute | MouseEventFlags.Move | GetFlagsForButton(button, true, true);

        Keyboard.PressModifierKeys(modifierKeys);
        WindowsApiHelper.mouse_event(eventFlags, dx, dy, 0, UIntPtr.Zero);
        Thread.Sleep(100);
        WindowsApiHelper.mouse_event(eventFlags, dx, dy, 0, UIntPtr.Zero);
        if (IsWaitForControlReadyEnabled)
            control?.WaitForControlReady();
        Keyboard.ReleaseModifierKeys(modifierKeys);
    }

    private static void HoverImplementation(IUITestControl control, Point? point, int duration)
    {
        var (dx, dy) = GetCoordinates(control, point, false);
        var eventFlags = MouseEventFlags.Absolute | MouseEventFlags.Move;

        int startX = GetDx(Cursor.Position.X);
        int startY = GetDy(Cursor.Position.Y);

        if (duration == 0)
        {
            WindowsApiHelper.mouse_event(eventFlags, dx, dy, 0, UIntPtr.Zero);
        }
        else
        {
            duration /= 10;
            for (double i = 1; i < duration; i++)
            {
                var currentX = startX + (i * ((dx - startX) / duration));
                var currentY = startY + (i * ((dy - startY) / duration));
                WindowsApiHelper.mouse_event(eventFlags, Convert.ToInt32(currentX), Convert.ToInt32(currentY), 0, UIntPtr.Zero);
                Thread.Sleep(10);
            }
        }
    }

    private static void StartDraggingImplementation(IUITestControl control, MouseButtons button, ModifierKeys modifierKeys, Point? point)
    {
        var (dx, dy) = GetCoordinates(control, point, false);
        var eventFlags = MouseEventFlags.Absolute | MouseEventFlags.Move | GetFlagsForButton(button, true, false);

        Keyboard.PressModifierKeys(modifierKeys);
        WindowsApiHelper.mouse_event(eventFlags, dx, dy, 0, UIntPtr.Zero);
        if (IsWaitForControlReadyEnabled)
            control?.WaitForControlReady();
    }

    private static void StopDraggingImplementation(IUITestControl control, Point? point, bool isDisplacement)
    {
        var (dx, dy) = GetCoordinates(control, point, false);
        var eventFlags = MouseEventFlags.Absolute | MouseEventFlags.Move | GetFlagsForButton(MouseButtons.Left | MouseButtons.Right | MouseButtons.Middle | MouseButtons.XButton1 | MouseButtons.XButton2, false, true);

        WindowsApiHelper.mouse_event(eventFlags, dx, dy, 0, UIntPtr.Zero);
        if (IsWaitForControlReadyEnabled)
            control?.WaitForControlReady();
        Keyboard.ReleaseModifierKeys(ModifierKeys.Alt | ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Windows);
    }

    private static MouseEventFlags GetFlagsForButton(MouseButtons button, bool addPress, bool addRelease)
    {
        MouseEventFlags result = 0;
        if (button.HasFlag(MouseButtons.Left))
            AppendButtonFlag(ref result, MouseEventFlags.LeftDown, MouseEventFlags.LeftUp, addPress, addRelease);
        if (button.HasFlag(MouseButtons.Right))
            AppendButtonFlag(ref result, MouseEventFlags.RightDown, MouseEventFlags.RightUp, addPress, addRelease);
        if (button.HasFlag(MouseButtons.Middle))
            AppendButtonFlag(ref result, MouseEventFlags.MiddleDown, MouseEventFlags.MiddleUp, addPress, addRelease);
        if (button.HasFlag(MouseButtons.XButton1) || button.HasFlag(MouseButtons.XButton2))
            AppendButtonFlag(ref result, MouseEventFlags.XDown, MouseEventFlags.XUp, addPress, addRelease);
        return result;
    }

    private static void AppendButtonFlag(ref MouseEventFlags flags, MouseEventFlags pressFlag, MouseEventFlags releaseFlag, bool addPress, bool addRelease)
    {
        if (addPress)
            flags |= pressFlag;
        if (addRelease)
            flags |= releaseFlag;
    }

    private static (int Dx, int Dy) GetCoordinates(IUITestControl control, Point? point, bool isDisplacement)
    {
        var mousePos = Cursor.Position;
        Point coordinates;
        if (control == null)
            coordinates = point ?? new Point(mousePos.X, mousePos.Y);
        else
            coordinates = point.HasValue ? ConcatPoints(control.Location, point.Value) : control.ClickablePoint;

        if (isDisplacement)
            coordinates = new Point(coordinates.X + mousePos.X, coordinates.Y + mousePos.Y);

        return (GetDx((int)coordinates.X), GetDy((int)coordinates.Y));
    }

    private static int GetDx(int x) => (int)((65536.0 / WindowsApiHelper.GetSystemMetrics(SystemMetric.SM_CXSCREEN) * x) - 1);
    private static int GetDy(int y) => (int)((65536.0 / WindowsApiHelper.GetSystemMetrics(SystemMetric.SM_CYSCREEN) * y) - 1);
    private static Point ConcatPoints(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
}
