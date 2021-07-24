using System;

namespace Rocketcress.Core.Assertion
{
    /// <summary>
    /// Represents an assert operation.
    /// </summary>
    public interface IAssertOperation
    {
        /// <summary>
        /// Configures the operation so that it returns a value depending of wether the assertion failed or not.
        /// </summary>
        /// <typeparam name="T">The type of value to return.</typeparam>
        /// <param name="resultOnSuccess">The value to return when the assertion succeeds.</param>
        /// <param name="resultOnFailure">The value to return when the assertion fails.</param>
        /// <returns>A assert operation that is configured to return a value.</returns>
        IAssertWithResult<T> WithResult<T>(T resultOnSuccess, T resultOnFailure);

        /// <summary>
        /// Runs the specified assertion and handles the result as configured.
        /// </summary>
        /// <param name="assertAction">The assertion to execute.</param>
        /// <returns><c>true</c> if the assertion succeeds; otherwise <c>false</c>.</returns>
        bool That(Action<Assert> assertAction);
    }
}
