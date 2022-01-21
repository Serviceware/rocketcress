using Rocketcress.Core.Utilities;
using System.Runtime.Versioning;

namespace Rocketcress.Core
{
    /// <summary>
    /// Helper for OS specific actions.
    /// </summary>
    public static class OsHelper
    {
#if !NETFRAMEWORK
        /// <summary>
        /// Runs a bash command on linux distributions.
        /// If the current OS is not Linux a <see cref="NotSupportedException"/> is thrown.
        /// </summary>
        /// <param name="command">The bash command to execute.</param>
        /// <returns>Returns the standard output of the bash command.</returns>
        [SupportedOSPlatform("linux")]
        [Obsolete("Use Rocketcress.Core.Utilities.ScriptUtility.RunBashCommand instead.")]
        public static string RunBashCommand(string command)
            => ScriptUtility.RunBashCommand(command);
#endif

        /// <summary>
        /// Sets the positon of the cursor in the current desktop session.
        /// </summary>
        /// <param name="x">The X-position to set the cursor to.</param>
        /// <param name="y">The Y-position to set the cursor to.</param>
        [SupportedOSPlatform("windows")]
        [Obsolete("Use Rocketcress.Core.Utilities.DesktopUtility.SetCursorPosition instead.")]
        public static void SetCursorPosition(int x, int y)
            => DesktopUtility.SetCursorPosition(x, y);
    }
}
