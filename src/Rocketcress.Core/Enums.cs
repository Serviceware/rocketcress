using Rocketcress.Core.Base;

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

/// <summary>
/// Specified constants that control how a context should be created in a <see cref="TestBase{T1,T2}"/>.
/// </summary>
public enum ContextCreationMode
{
    /// <summary>
    /// No context will be created. A context needs to be created manually for each test.
    /// </summary>
    None,

    /// <summary>
    /// A context is only created but not initialized. The context needs to be initialized manually for each test.
    /// </summary>
    OnlyCreate,

    /// <summary>
    /// A context is created an initialized.
    /// </summary>
    CreateAndInitialize,
}