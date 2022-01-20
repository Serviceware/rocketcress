using System.Globalization;

namespace Rocketcress.Core;

/// <summary>
/// Provides methods to easily log messages to the Trace-Log.
/// </summary>
public static class Logger
{
    private static readonly IDictionary<LogLevel, string> _logLevelShortNames = new Dictionary<LogLevel, string>
    {
        [LogLevel.Debug] = "DBG",
        [LogLevel.Info] = "INF",
        [LogLevel.Warning] = "WRN",
        [LogLevel.Error] = "ERR",
        [LogLevel.Critical] = "CRT",
    };

    /// <summary>
    /// Logs a message.
    /// </summary>
    /// <param name="level">The log-level of the message.</param>
    /// <param name="message">The message text.</param>
    /// <param name="params">The parameters that should be inserted into the message text (like string.Format).</param>
    public static void Log(LogLevel level, string? message, params object?[]? @params)
    {
        try
        {
            var shortName = _logLevelShortNames.TryGetValue(level, out string? tmp) ? tmp : "___";
            Trace.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {shortName} - {(@params == null || @params.Length == 0 ? message : string.Format(CultureInfo.InvariantCulture, message ?? string.Empty, @params))}");
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: ERR - Error while writing trace with message \"{message}\" and parameters \"{string.Join("\", \"", @params ?? Array.Empty<object?>())}\": {ex}");
        }
    }

    /// <summary>
    /// Logs a message at debug log-level.
    /// </summary>
    /// <param name="message">The message text.</param>
    /// <param name="params">The parameters that should be inserted into the message text (like string.Format).</param>
    public static void LogDebug(string? message, params object?[]? @params)
        => Log(LogLevel.Debug, message, @params);

    /// <summary>
    /// Logs a message at information log-level.
    /// </summary>
    /// <param name="message">The message text.</param>
    /// <param name="params">The parameters that should be inserted into the message text (like string.Format).</param>
    public static void LogInfo(string? message, params object?[]? @params)
        => Log(LogLevel.Info, message, @params);

    /// <summary>
    /// Logs a message at warning log-level.
    /// </summary>
    /// <param name="message">The message text.</param>
    /// <param name="params">The parameters that should be inserted into the message text (like string.Format).</param>
    public static void LogWarning(string? message, params object?[]? @params)
        => Log(LogLevel.Warning, message, @params);

    /// <summary>
    /// Logs a message at error log-level.
    /// </summary>
    /// <param name="message">The message text.</param>
    /// <param name="params">The parameters that should be inserted into the message text (like string.Format).</param>
    public static void LogError(string? message, params object?[]? @params)
        => Log(LogLevel.Error, message, @params);

    /// <summary>
    /// Logs a message at critial log-level.
    /// </summary>
    /// <param name="message">The message text.</param>
    /// <param name="params">The parameters that should be inserted into the message text (like string.Format).</param>
    public static void LogCritical(string? message, params object?[]? @params) => Log(LogLevel.Critical, message, @params);
}
