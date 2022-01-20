using System.Runtime.Versioning;

namespace Rocketcress.Core.Utilities;

/// <summary>
/// Helper for OS specific actions.
/// </summary>
public static class OsHelper
{
    [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetCursorPos(int x, int y);

    /// <summary>
    /// Runs a bash command on linux distributions.
    /// If the current OS is not Linux a <see cref="NotSupportedException"/> is thrown.
    /// </summary>
    /// <param name="command">The bash command to execute.</param>
    /// <returns>Returns the standard output of the bash command.</returns>
    public static string RunBashCommand(string command)
    {
        if (!OperatingSystem.IsLinux())
            throw new NotSupportedException("Bash Commands can only be executed on Linux.");

        var escapedArgs = command.Replace("\"", "\\\"");
        var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{escapedArgs}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            },
        };
        process.Start();
        string result = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        return result;
    }

    /// <summary>
    /// Sets the positon of the cursor in the current desktop session.
    /// </summary>
    /// <param name="x">The X-position to set the cursor to.</param>
    /// <param name="y">The Y-position to set the cursor to.</param>
    public static void SetCursorPosition(int x, int y)
    {
        if (OperatingSystem.IsWindows())
        {
            SetCursorPos(x, y);
        }
    }
}
