using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Rocketcress.Core.Utilities
{
    /// <summary>
    /// Utility for desktop actions.
    /// </summary>
    public static class DesktopUtility
    {
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);

        /// <summary>
        /// Sets the positon of the cursor in the current desktop session.
        /// </summary>
        /// <param name="x">The X-position to set the cursor to.</param>
        /// <param name="y">The Y-position to set the cursor to.</param>
        [SupportedOSPlatform("windows")]
        public static void SetCursorPosition(int x, int y)
        {
            if (OperatingSystem.IsWindows())
            {
                SetCursorPos(x, y);
            }
        }
    }
}
