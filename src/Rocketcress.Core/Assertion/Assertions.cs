using System;
using System.Diagnostics.CodeAnalysis;

namespace Rocketcress.Core
{
    public partial class Assert
    {
        /// <summary>
        /// Verifies that a value is between two other values.
        /// </summary>
        /// <typeparam name="T">The type of value to compare.</typeparam>
        /// <param name="expectedMinimum">The inclusive expected minimum value.</param>
        /// <param name="expectedMaximum">The inclusive expected maximum value.</param>
        /// <param name="actual">The actual value.</param>
        public void IsBetweenOrEqual<T>(T expectedMinimum, T expectedMaximum, T actual)
            where T : IComparable<T>
            => IsBetween(expectedMinimum, expectedMaximum, actual, true, true);

        /// <summary>
        /// Verifies that a value is between two other values.
        /// </summary>
        /// <typeparam name="T">The type of value to compare.</typeparam>
        /// <param name="expectedMinimum">The inclusive expected minimum value.</param>
        /// <param name="expectedMaximum">The inclusive expected maximum value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public void IsBetweenOrEqual<T>(T expectedMinimum, T expectedMaximum, T actual, string message)
            where T : IComparable<T>
            => IsBetween(expectedMinimum, expectedMaximum, actual, true, true, message);

        /// <summary>
        /// Verifies that an action throws a specific exception.
        /// </summary>
        /// <typeparam name="T">The expected type of the exception.</typeparam>
        /// <param name="action">The action to verify.</param>
        /// <param name="matchExactly">Determines wether the exception type has to match <typeparamref name="T"/> exactly or derived types are allowed.</param>
        public void ThrowsException<T>(Action action, bool matchExactly)
            where T : Exception
            => ThrowsException<T>(action, matchExactly, false);

        /// <summary>
        /// Verifies that an action throws a specific exception.
        /// </summary>
        /// <typeparam name="T">The expected type of the exception.</typeparam>
        /// <param name="action">The action to verify.</param>
        /// <param name="matchExactly">Determines wether the exception type has to match <typeparamref name="T"/> exactly or derived types are allowed.</param>
        /// <param name="validateInnerException">Determine wether to validate the inner exception of the resulting exception, instead of the exception itself. If set to true and no inner exception exists the assert will fail.</param>
        public void ThrowsException<T>(Action action, bool matchExactly, bool validateInnerException)
            where T : Exception
        {
            bool hasException = false;
            try
            {
                action();
            }
            catch (Exception ex)
            {
                hasException = true;
                if (validateInnerException)
                {
                    if (ex.InnerException == null)
                        Throw($"The exception that occurred does not have an inner exception.", ex);
                    ex = ex.InnerException;
                }

                if ((matchExactly && ex.GetType() != typeof(T)) || (!matchExactly && !typeof(T).IsInstanceOfType(ex)))
                    Throw($"The action was expected to throw an exception{(matchExactly ? " exactly" : string.Empty)} of type \"{typeof(T).FullName}\", but did throw an exception of type \"{ex.GetType().FullName}\".", ex);
            }

            if (!hasException)
                Throw($"The action was expected to throw an exception of type \"{typeof(T).FullName}\", but did not throw any exceptions.", null);

            [DoesNotReturn]
            void Throw(string message, Exception? exception)
            {
                ThrowAssertError(message, ("ExpectedException", typeof(T).Name), ("ActualException", exception?.GetType().Name), ("ExceptionDetails", exception?.ToString()));
            }
        }
    }
}
