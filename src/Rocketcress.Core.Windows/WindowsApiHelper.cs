using Rocketcress.Core.WindowsApiTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

#pragma warning disable SA1625 // Element documentation should not be copied and pasted
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable CA1401 // P/Invokes should not be visible
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
#pragma warning disable SA1300 // Element should begin with upper-case letter
#pragma warning disable SA1305 // Field names should not use Hungarian notation
#pragma warning disable SA1201 // Elements should appear in the correct order
#pragma warning disable SA1202 // Elements should be ordered by access

namespace Rocketcress.Core;

/// <summary>
/// Helper class for calling windows API methods.
/// </summary>
public static class WindowsApiHelper
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements should be documented
    public delegate bool Win32Callback(IntPtr hwnd, IntPtr lParam);
#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Retrieves the identifier of the thread that created the specified window and, optionally, the identifier of the process that created the window.
    /// </summary>
    /// <param name="hWnd">A handle to the window. </param>
    /// <param name="lpdwProcessId">A pointer to a variable that receives the process identifier. If this parameter is not NULL, GetWindowThreadProcessId copies the identifier of the process to the variable; otherwise, it does not. </param>
    /// <returns>The return value is the identifier of the thread that created the window. </returns>
    /// <remarks>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-getwindowthreadprocessid.</remarks>
    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    /// <summary>
    /// Enumerates the child windows that belong to the specified parent window by passing the handle to each child window, in turn, to an application-defined callback function. EnumChildWindows continues until the last child window is enumerated or the callback function returns FALSE.
    /// </summary>
    /// <param name="parentHandle">A handle to the parent window whose child windows are to be enumerated. If this parameter is NULL, this function is equivalent to EnumWindows.</param>
    /// <param name="callback">A pointer to an application-defined callback function. For more information, see EnumChildProc.</param>
    /// <param name="lParam">An application-defined value to be passed to the callback function.</param>
    /// <returns>The return value is not used.</returns>
    /// <remarks>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-enumchildwindows.</remarks>
    [DllImport("user32.Dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumChildWindows(IntPtr parentHandle, Win32Callback callback, IntPtr lParam);

    /// <summary>
    /// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
    /// </summary>
    /// <param name="hWnd">A handle to the window whose window procedure will receive the message.</param>
    /// <param name="Msg">The message to be sent.</param>
    /// <param name="wParam">Additional message-specific information.</param>
    /// <param name="lParam">Additional message-specific information.</param>
    /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
    /// <remarks>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-sendmessage.</remarks>
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

    /// <summary>
    /// Determines the visibility state of the specified window.
    /// </summary>
    /// <param name="hWnd">A handle to the window to be tested.</param>
    /// <returns>If the specified window, its parent window, its parent's parent window, and so forth, have the WS_VISIBLE style, the return value is nonzero. Otherwise, the return value is zero.</returns>
    /// <remarks>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-iswindowvisible.</remarks>
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern bool IsWindowVisible(IntPtr hWnd);

    /// <summary>
    /// Determines whether the specified window handle identifies an existing window.
    /// </summary>
    /// <param name="hWnd">A handle to the window to be tested.</param>
    /// <returns>If the window handle identifies an existing window, the return value is nonzero; otherwise zero.</returns>
    /// <remarks>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-iswindow.</remarks>
    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern bool IsWindow(IntPtr hWnd);

    /// <summary>
    /// Changes the position and dimensions of the specified window. For a top-level window, the position and dimensions are relative to the upper-left corner of the screen. For a child window, they are relative to the upper-left corner of the parent window's client area.
    /// </summary>
    /// <param name="hwnd">A handle to the window.</param>
    /// <param name="x">The new position of the left side of the window.</param>
    /// <param name="y">The new position of the top of the window.</param>
    /// <param name="w">The new width of the window.</param>
    /// <param name="h">The new height of the window.</param>
    /// <param name="redraw">Indicates whether the window is to be repainted. If this parameter is TRUE, the window receives a message. If the parameter is FALSE, no repainting of any kind occurs.</param>
    /// <returns>If the function succeeds, the return value is nonzero; otherwise zero.</returns>
    /// <remarks>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-movewindow.</remarks>
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool MoveWindow(IntPtr hwnd, int x, int y, int w, int h, bool redraw);

    /// <summary>
    /// Changes the text of the specified window's title bar (if it has one).
    /// </summary>
    /// <param name="hWnd">A handle to the window whose text is to be changed.</param>
    /// <param name="text">The new title.</param>
    /// <returns>If the function succeeds, the return value is nonzero; otherwise zero.</returns>
    /// <remarks>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-setwindowtexta.</remarks>
    [DllImport("user32.dll")]
    public static extern bool SetWindowText(IntPtr hWnd, string text);

    /// <summary>
    /// Brings the thread that created the specified window into the foreground and activates the window.
    /// Keyboard input is directed to the window, and various visual cues are changed for the user.
    /// The system assigns a slightly higher priority to the thread that created the foreground window than it does to other threads.
    /// </summary>
    /// <param name="hWnd">A handle to the window that should be activated and brought to the foreground.</param>
    /// <returns>If the window was brought to the foreground, the return value is nonzero; otherwise zero.</returns>
    /// <remarks>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-setforegroundwindow.</remarks>
    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    /// <summary>
    /// Synthesizes keystrokes, mouse motions, and button clicks.
    /// </summary>
    /// <param name="numberOfInputs">The number of structures in the pInputs array.</param>
    /// <param name="inputs">An array of INPUT structures. Each structure represents an event to be inserted into the keyboard or mouse input stream.</param>
    /// <param name="sizeOfInputStructure">The size, in bytes, of an INPUT structure. If cbSize is not the size of an INPUT structure, the function fails.</param>
    /// <returns>The function returns the number of events that it successfully inserted into the keyboard or mouse input stream. If the function returns zero, the input was already blocked by another thread. To get extended error information, call GetLastError.</returns>
    /// <remarks>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-sendinput.</remarks>
    [DllImport("user32.dll")]
    public static extern uint SendInput(uint numberOfInputs, Input[] inputs, int sizeOfInputStructure);

    /// <summary>
    /// The mouse_event function synthesizes mouse motion and button clicks.
    /// </summary>
    /// <param name="dwFlags">Controls various aspects of mouse motion and button clicking.</param>
    /// <param name="dx">The mouse's absolute position along the x-axis or its amount of motion since the last mouse event was generated, depending on the setting of MOUSEEVENTF_ABSOLUTE. Absolute data is specified as the mouse's actual x-coordinate; relative data is specified as the number of mickeys moved. A mickey is the amount that a mouse has to move for it to report that it has moved.</param>
    /// <param name="dy">The mouse's absolute position along the y-axis or its amount of motion since the last mouse event was generated, depending on the setting of MOUSEEVENTF_ABSOLUTE. Absolute data is specified as the mouse's actual y-coordinate; relative data is specified as the number of mickeys moved.</param>
    /// <param name="dwData">Some data for the event. Please look into microsoft docs for more information.</param>
    /// <param name="dwExtraInfo">An additional value associated with the mouse event. An application calls GetMessageExtraInfo to obtain this extra information.</param>
    /// <remarks>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-mouse_event.</remarks>
    [DllImport("user32.dll")]
    public static extern void mouse_event(MouseEventFlags dwFlags, int dx, int dy, uint dwData, UIntPtr dwExtraInfo);

    /// <summary>
    /// Synthesizes a keystroke. The system can use such a synthesized keystroke to generate a WM_KEYUP or WM_KEYDOWN message. The keyboard driver's interrupt handler calls the keybd_event function.
    /// </summary>
    /// <param name="bVk">A virtual-key code. The code must be a value in the range 1 to 254. For a complete list, see Virtual Key Codes.</param>
    /// <param name="bScan">A hardware scan code for the key.</param>
    /// <param name="dwFlags">Controls various aspects of function operation.</param>
    /// <param name="dwExtraInfo">An additional value associated with the key stroke.</param>
    /// <remarks>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-keybd_event.</remarks>
    [DllImport("user32.dll")]
    public static extern void keybd_event(Keys bVk, byte bScan, KeyboardEvent dwFlags, int dwExtraInfo);

    /// <summary>
    /// Retrieves the specified system metric or system configuration setting.
    /// Note that all dimensions retrieved by GetSystemMetrics are in pixels.
    /// </summary>
    /// <param name="smIndex">The system metric or configuration setting to be retrieved. This parameter can be one of the following values. Note that all SM_CX* values are widths and all SM_CY* values are heights. Also note that all settings designed to return Boolean data represent TRUE as any nonzero value, and FALSE as a zero value.</param>
    /// <returns>If the function succeeds, the return value is the requested system metric or configuration setting. If the function fails, the return value is 0. GetLastError does not provide extended error information.</returns>
    /// <remarks>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-getsystemmetrics.</remarks>
    [DllImport("user32.dll")]
    public static extern int GetSystemMetrics(SystemMetric smIndex);

    /// <summary>
    /// Sets the process-default DPI awareness to system-DPI awareness. This is equivalent to calling SetProcessDpiAwarenessContext with a DPI_AWARENESS_CONTEXT value of DPI_AWARENESS_CONTEXT_SYSTEM_AWARE.
    /// </summary>
    /// <returns>If the function succeeds, the return value is nonzero. Otherwise, the return value is zero.</returns>
    /// <remarks>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-setprocessdpiaware.</remarks>
    [DllImport("user32.dll")]
    public static extern bool SetProcessDPIAware();

    /// <summary>
    /// Moves the cursor to the specified screen coordinates. If the new coordinates are not within the screen rectangle set by the most recent ClipCursor function call, the system automatically adjusts the coordinates so that the cursor stays within the rectangle.
    /// </summary>
    /// <param name="x">The new x-coordinate of the cursor, in screen coordinates.</param>
    /// <param name="y">The new y-coordinate of the cursor, in screen coordinates.</param>
    /// <returns>Returns nonzero if successful or zero otherwise. To get extended error information, call GetLastError.</returns>
    /// <remarks>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-setcursorpos.</remarks>
    [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetCursorPos(int x, int y);

    /// <summary>
    /// Sends the specified message to one or more windows.
    /// </summary>
    /// <param name="hWnd">A handle to the window whose window procedure will receive the message.</param>
    /// <param name="msg">The message to be sent.</param>
    /// <param name="wParam">Any additional message-specific information.</param>
    /// <param name="lParam">Any additional message-specific information.</param>
    /// <param name="flags">The behavior of this function.</param>
    /// <param name="timeout">The duration of the time-out period, in milliseconds. If the message is a broadcast message, each window can use the full time-out period. For example, if you specify a five second time-out period and there are three top-level windows that fail to process the message, you could have up to a 15 second delay.</param>
    /// <param name="pdwResult">The result of the message processing. The value of this parameter depends on the message that is specified.</param>
    /// <returns>If the function succeeds, the return value is nonzero. SendMessageTimeout does not provide information about individual windows timing out if HWND_BROADCAST is used.</returns>
    /// <remarks>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-sendmessagetimeouta.</remarks>
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessageTimeout(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam, int flags, int timeout, out IntPtr pdwResult);

    /// <summary>
    /// The BitBlt function performs a bit-block transfer of the color data corresponding to a rectangle of pixels from the specified source device context into a destination device context.
    /// </summary>
    /// <param name="hDC">A handle to the destination device context.</param>
    /// <param name="x">The x-coordinate, in logical units, of the upper-left corner of the destination rectangle.</param>
    /// <param name="y">The y-coordinate, in logical units, of the upper-left corner of the destination rectangle.</param>
    /// <param name="nWidth">The width, in logical units, of the source and destination rectangles.</param>
    /// <param name="nHeight">The height, in logical units, of the source and the destination rectangles.</param>
    /// <param name="hSrcDC">A handle to the source device context.</param>
    /// <param name="xSrc">The x-coordinate, in logical units, of the upper-left corner of the source rectangle.</param>
    /// <param name="ySrc">The y-coordinate, in logical units, of the upper-left corner of the source rectangle.</param>
    /// <param name="dwRop">A raster-operation code. These codes define how the color data for the source rectangle is to be combined with the color data for the destination rectangle to achieve the final color.</param>
    /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
    /// <remarks>https://docs.microsoft.com/en-us/windows/desktop/api/wingdi/nf-wingdi-bitblt.</remarks>
    [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
    public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

    /// <summary>
    /// Multi Monitor Function.
    /// </summary>
    public const int MONITOR_DEFAULTTONULL = 0x00000000;

    /// <summary>
    /// The MonitorFromRect function retrieves a handle to the display monitor that has the largest area of intersection with a specified rectangle.
    /// </summary>
    /// <param name="rect">A pointer to a RECT structure that specifies the rectangle of interest in virtual-screen coordinates.</param>
    /// <param name="dwFlags">Determines the function's return value if the rectangle does not intersect any display monitor.</param>
    /// <returns>If the rectangle intersects one or more display monitor rectangles, the return value is an HMONITOR handle to the display monitor that has the largest area of intersection with the rectangle.</returns>
    /// <remarks>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-monitorfromrect.</remarks>
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr MonitorFromRect(ref RECT rect, int dwFlags);

    /// <summary>
    /// Sent as a signal that a window or an application should terminate.
    /// </summary>
    public static readonly uint WM_CLOSE = 0x0010;

    /// <summary>
    /// A window receives this message when the user chooses a command from the Window menu (formerly known as the system or control menu) or when the user chooses the maximize button, minimize button, restore button, or close button.
    /// </summary>
    public static readonly uint WM_SYSCOMMAND = 0x0112;

    /// <summary>
    /// Closes the window.
    /// </summary>
    public static readonly int SC_CLOSE = 0xF060;

    /// <summary>
    /// The function returns without waiting for the time-out period to elapse if the receiving thread appears to not respond or "hangs.".
    /// </summary>
    public static readonly int SMTO_ABORTIFHUNG = 2;

    #region Windows Interop

    /// <summary>
    /// Closes a window without waiting for the closing.
    /// </summary>
    /// <param name="windowHandle">The handle to the window.</param>
    public static void CloseWindow(IntPtr windowHandle)
    {
        Task.Run(() => SendMessage(windowHandle, WM_SYSCOMMAND, SC_CLOSE, 0));
    }

    /// <summary>
    /// Searches for the root windows of a process.
    /// </summary>
    /// <param name="pid">The id of the process for which the root windows should be searched.</param>
    /// <returns>Returns a list of handles to the root windows of the given process.</returns>
    public static List<IntPtr> GetRootWindowsOfProcess(int pid)
    {
        List<IntPtr> rootWindows = GetChildWindows(IntPtr.Zero);
        List<IntPtr> dsProcRootWindows = new List<IntPtr>();
        foreach (IntPtr hWnd in rootWindows)
        {
            GetWindowThreadProcessId(hWnd, out uint lpdwProcessId);
            if (lpdwProcessId == pid)
                dsProcRootWindows.Add(hWnd);
        }

        return dsProcRootWindows;
    }

    private static List<IntPtr> GetChildWindows(IntPtr parent)
    {
        List<IntPtr> result = new List<IntPtr>();
        GCHandle listHandle = GCHandle.Alloc(result);
        try
        {
            Win32Callback childProc = EnumWindow;
            EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
        }
        finally
        {
            if (listHandle.IsAllocated)
                listHandle.Free();
        }

        return result;
    }

    private static bool EnumWindow(IntPtr handle, IntPtr pointer)
    {
        GCHandle gch = GCHandle.FromIntPtr(pointer);
        List<IntPtr> list = gch.Target as List<IntPtr>;
        if (list == null)
        {
            throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
        }

        list.Add(handle);

        // You can modify this to check to see if you want to cancel the operation, then return a null here
        return true;
    }

    /// <summary>
    /// Checks if a process is responding.
    /// </summary>
    /// <param name="processId">The process id of the process to check.</param>
    /// <param name="timeoutMs">The check timeout in miliseconds.</param>
    /// <returns>Returns true if the process is responding; otherwise false.</returns>
    public static bool IsProcessResponding(int processId, int timeoutMs = 10) => IsProcessRespondingInternal(Process.GetProcessById(processId), timeoutMs);

    /// <summary>
    /// Checks if a process is responding.
    /// </summary>
    /// <param name="process">The process to check.</param>
    /// <param name="timeoutMs">The check timeout in miliseconds.</param>
    /// <returns>Returns true if the process is responding; otherwise false.</returns>
    public static bool IsProcessResponding(Process process, int timeoutMs = 10) => IsProcessRespondingInternal(Process.GetProcessById(Guard.NotNull(process).Id), timeoutMs);
    private static bool IsProcessRespondingInternal(Process process, int timeoutMs = 10)
    {
        var handleRef = new HandleRef(process, process.MainWindowHandle);
        return SendMessageTimeout(handleRef, 0, IntPtr.Zero, IntPtr.Zero, SMTO_ABORTIFHUNG, timeoutMs, out var _) != IntPtr.Zero;
    }

    /// <summary>
    /// Retrieves the color of a specified screen location.
    /// </summary>
    /// <param name="x">The x position of the pixel.</param>
    /// <param name="y">The y position of the pixel.</param>
    /// <returns>Returns the color of the specified pixel.</returns>
    public static Color GetScreenPixel(int x, int y)
    {
        var bmp = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        using (Graphics gdest = Graphics.FromImage(bmp))
        using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
        {
            IntPtr hSrcDC = gsrc.GetHdc();
            IntPtr hDC = gdest.GetHdc();
            int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, x, y, (int)CopyPixelOperation.SourceCopy);
            gdest.ReleaseHdc();
            gsrc.ReleaseHdc();
        }

        return bmp.GetPixel(0, 0);
    }

    #endregion
}
