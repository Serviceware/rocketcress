using MaSch.Test.Assertion;
using Rocketcress.Core.Assertion;
using System;

namespace Rocketcress.Core
{
    /// <summary>
    /// Provides extension methods for the <see cref="IAssertOperation"/> interface.
    /// </summary>
    public static class AssertOperationExtensions
    {
        /// <summary>
        /// Configures the operation so that it returns the specified value if the assertion succeeds and the default value of the same type if it does not.
        /// </summary>
        /// <typeparam name="T">The type of value to return.</typeparam>
        /// <param name="assertOperation">The assert operation to configure.</param>
        /// <param name="resultOnSuccess">The value to return when the assertion succeeds.</param>
        /// <returns>A assert operation that is configured to return a value.</returns>
        public static IAssertWithResult<T?> WithResult<T>(this IAssertOperation assertOperation, T resultOnSuccess)
        {
            return assertOperation.WithResult(resultOnSuccess, default);
        }

        /// <summary>
        /// Runs the specified assertion and handles the result as configured.
        /// </summary>
        /// <typeparam name="T">The type of value to return.</typeparam>
        /// <param name="assertOperation">The assert operation to execute.</param>
        /// <param name="assertAction">The assertion to execute.</param>
        /// <returns>The <c>default</c> value of <typeparamref name="T"/> regardles of the result of the assertion.</returns>
        public static T? That<T>(this IAssertOperation assertOperation, Action<AssertBase> assertAction)
        {
            assertOperation.That(assertAction);
            return default;
        }
    }
}
