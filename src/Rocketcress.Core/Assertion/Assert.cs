using MaSch.Test.Assertion;
using Rocketcress.Core.Assertion;

namespace Rocketcress.Core;

/// <summary>
/// Assertion class for Rocketcress tests.
/// </summary>
public partial class Assert : AssertBase
{
    private static Assert? _instance;
    private static Assert? _nonThrowInstance;

    private readonly bool _throw;

    /// <summary>
    /// Gets the current instance of the AssertEx singleton instance.
    /// </summary>
    public static Assert Instance => _instance ??= new Assert(true);

    internal static Assert NonThrowInstance => _nonThrowInstance ??= new Assert(false);

    /// <summary>
    /// Gets or sets a value indicating whether failed assertions should be logged.
    /// </summary>
    public bool IsFailedAssertionLogEnabled { get; set; } = true;

    /// <inheritdoc/>
    protected override string? AssertNamePrefix { get; } = "Assert";

    /// <summary>
    /// Initializes a new instance of the <see cref="Assert"/> class.
    /// </summary>
    /// <param name="throw">Determines wether this instance is intended for the case, an <see cref="AssertFailedException"/> should be thrown.</param>
    internal Assert(bool @throw)
    {
        _throw = @throw;
    }

    /// <summary>
    /// Creates an assert operation.
    /// </summary>
    /// <param name="throw">Determines wether the assert operation should throw an <see cref="AssertFailedException"/> when assertions fail.</param>
    /// <returns>An assert operation.</returns>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "This is intended to be an instance member for easier usage.")]
    public IAssertOperation When(bool @throw)
    {
        return new AssertOperation(@throw);
    }

    /// <inheritdoc/>
    [DoesNotReturn]
    protected override void HandleFailedAssertion(string message)
    {
        if (IsFailedAssertionLogEnabled)
        {
            if (_throw)
                Logger.Log(LogLevel.Error, "Failed assertion: " + message + Environment.NewLine + new StackTrace());
            else
                Logger.Log(LogLevel.Info, "Failed assertion: " + message);
        }

        // Exception is thrown regardles of '_throw' so it can be catched by the assert operation.
        throw new AssertFailedException(message);
    }
}
