namespace Rocketcress.Core;

/// <summary>
/// Specifies constants for log levels.
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Used for log messages that are used to debug.
    /// </summary>
    Debug,

    /// <summary>
    /// Used for log messages that show useful information.
    /// </summary>
    Info,

    /// <summary>
    /// Used for log messages that warns about some issue.
    /// </summary>
    Warning,

    /// <summary>
    /// Used for log messages that notifies about errors.
    /// </summary>
    Error,

    /// <summary>
    /// Used for log messages that notifies about errors that lead to the application failing.
    /// </summary>
    Critical,
}

/// <summary>
/// Specifies constants for order directions.
/// </summary>
public enum Order
{
    /// <summary>
    /// Undefined sort order.
    /// </summary>
    None,

    /// <summary>
    /// Ascending sort order.
    /// </summary>
    Ascending,

    /// <summary>
    /// Descending sort order.
    /// </summary>
    Descending,
}
