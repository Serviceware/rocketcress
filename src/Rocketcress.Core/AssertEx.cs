using Rocketcress.Core.Extensions;
#if !SLIM
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

#nullable disable

namespace Rocketcress.Core
{
    /// <summary>
    /// Represents a extendable class, that contains methods for asserting different results.
    /// </summary>
    public class AssertEx
    {
        private static AssertEx _instance;

        /// <summary>
        /// Gets the current instance of the AssertEx singleton instance.
        /// </summary>
        public static AssertEx Instance => _instance ??= new AssertEx();

        /// <summary>
        /// Gets or sets a value indicating whether failed assertions should be logged.
        /// </summary>
        public bool IsFailedAssertionLogEnabled { get; set; } = true;

        private AssertEx()
        {
        }

        /// <summary>
        /// Fails the assertion without checking any conditions. Displays a message.
        /// </summary>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="assert">Determines wether to throw a AssertFailedException.</param>
        /// <returns>Returns false.</returns>
        public bool Fail(string message, bool assert) => Fail<bool>(message, assert, false);

        /// <summary>
        /// Fails the assertion without checking any conditions. Displays a message.
        /// </summary>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="assert">Determines wether to throw a AssertFailedException.</param>
        /// <param name="result">The result that should be returned by this method.</param>
        /// <returns>Returns the value of the result parameter.</returns>
        public bool Fail(string message, bool assert, bool result) => Fail<bool>(message, assert, result);

        /// <summary>
        /// Fails the assertion without checking any conditions. Displays a message.
        /// </summary>
        /// <typeparam name="T">The type of the return value.</typeparam>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="assert">Determines wether to throw a AssertFailedException.</param>
        /// <returns>Returns the default value of the specified return type.</returns>
        public T Fail<T>(string message, bool assert) => Fail(message, assert, default(T));

        /// <summary>
        /// Fails the assertion without checking any conditions. Displays a message.
        /// </summary>
        /// <typeparam name="T">The type of the return value.</typeparam>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="assert">Determines wether to throw a AssertFailedException.</param>
        /// <param name="result">The result that should be returned by this method.</param>
        /// <returns>Returns the value of the result parameter.</returns>
        public T Fail<T>(string message, bool assert, T result)
            => RunAssertAction(() => Fail(message), assert, result, result);

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception
        /// if the two values are not equal. Different numeric types are treated
        /// as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <typeparam name="T">
        /// The type of values to compare.
        /// </typeparam>
        /// <param name="expected">
        /// The first value to compare. This is the value the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second value to compare. This is the value produced by the code under test.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is not equal to <paramref name="expected"/>. The message is shown in
        /// test results.
        /// </param>
        /// <param name="assert">Determines wether a AssertFailedException should be thrown.</param>
        /// <returns>Returns true if the assertion succeeds; otherwise false.</returns>
        public bool AreEqual<T>(T expected, T actual, string message, bool assert)
            => RunAssertAction(() => AreEqual(expected, actual, message), assert, true, false);

        /// <summary>
        /// Tests whether the specified object is non-null and throws an exception if it is null.
        /// </summary>
        /// <typeparam name="T">The return value type.</typeparam>
        /// <param name="value">The object the test expects not to be null.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="assert">Determines wether to throw a AssertFailedException.</param>
        /// <param name="returnOnSuccess">Value that should be returned if the assert succeeds.</param>
        /// <returns>The value of <paramref name="returnOnSuccess"/> if the assert succeeds; otherwise the default value of <typeparamref name="T"/>.</returns>
        public T IsNotNull<T>(object value, string message, bool assert, T returnOnSuccess) => IsNotNull(value, message, assert, returnOnSuccess, default);

        /// <summary>
        /// Tests whether the specified object is non-null and throws an exception if it is null.
        /// </summary>
        /// <typeparam name="T">The return value type.</typeparam>
        /// <param name="value">The object the test expects not to be null.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="assert">Determines wether to throw a AssertFailedException.</param>
        /// <param name="returnOnSuccess">Value that should be returned if the assert succeeds.</param>
        /// <param name="returnOnFail">Value that should be returned if the assert fails.</param>
        /// <returns>The value of <paramref name="returnOnSuccess"/> if the assert succeeds; otherwise the value of <paramref name="returnOnFail"/>.</returns>
        public T IsNotNull<T>(object value, string message, bool assert, T returnOnSuccess, T returnOnFail)
            => RunAssertAction(() => IsNotNull(value, message), assert, returnOnSuccess, returnOnFail);

        /// <summary>
        /// Tests whether the specified object is null and throws an exception if it is not null.
        /// </summary>
        /// <typeparam name="T">The return value type.</typeparam>
        /// <param name="value">The object the test expects to be null.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="assert">Determines wether to throw a AssertFailedException.</param>
        /// <param name="returnOnSuccess">Value that should be returned if the assert succeeds.</param>
        /// <returns>The value of <paramref name="returnOnSuccess"/> if the assert succeeds; otherwise the default value of <typeparamref name="T"/>.</returns>
        public T IsNull<T>(object value, string message, bool assert, T returnOnSuccess) => IsNull(value, message, assert, returnOnSuccess, default);

        /// <summary>
        /// Tests whether the specified object is null and throws an exception if it is not null.
        /// </summary>
        /// <typeparam name="T">The return value type.</typeparam>
        /// <param name="value">The object the test expects to be null.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="assert">Determines wether to throw a AssertFailedException.</param>
        /// <param name="returnOnSuccess">Value that should be returned if the assert succeeds.</param>
        /// <param name="returnOnFail">Value that should be returned if the assert fails.</param>
        /// <returns>The value of <paramref name="returnOnSuccess"/> if the assert succeeds; otherwise the value of <paramref name="returnOnFail"/>.</returns>
        public T IsNull<T>(object value, string message, bool assert, T returnOnSuccess, T returnOnFail)
            => RunAssertAction(() => IsNull(value, message), assert, returnOnSuccess, returnOnFail);

        /// <summary>
        /// Runs an action and handles any AssertFailedExceptions.
        /// </summary>
        /// <typeparam name="T">The type of the return value type.</typeparam>
        /// <param name="action">The action to execute.</param>
        /// <param name="assert">Determines wether the assert should be rethrown.</param>
        /// <param name="returnOnSuccess">The return value that is used if the action does not throw an AssertFailedException.</param>
        /// <param name="returnOnFail">The return value that is used if the action does throw an AssertFailedException.</param>
        /// <returns>Returns the returnOnSuccess value if the action does not throw an AssertFailedException; otherwise the returnOnFail value.</returns>
        public T RunAssertAction<T>(Action action, bool assert, T returnOnSuccess, T returnOnFail)
        {
            try
            {
                using (EnterNonLoggingScope())
                    action();
                return returnOnSuccess;
            }
            catch (AssertFailedException ex)
            {
                LogFailedAssertion(ex, assert ? LogLevel.Error : LogLevel.Info);
                if (assert)
                    throw;
                return returnOnFail;
            }
        }

        /// <summary>
        /// Runs an action and handles any AssertFailedExceptions.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="assert">Determines wether the assert should be rethrown.</param>
        /// <returns>Returns true if the action does not throw an AssertFailedException; otherwise false.</returns>
        public bool RunAssertAction(Action action, bool assert)
        {
            try
            {
                using (EnterNonLoggingScope())
                    action();
                return true;
            }
            catch (AssertFailedException ex)
            {
                LogFailedAssertion(ex, assert ? LogLevel.Error : LogLevel.Info);
                if (assert)

                    throw;
                return false;
            }
        }

        /// <summary>
        /// Verifies that a value is greater or equal to another value.
        /// </summary>
        /// <typeparam name="T">The type of value to compare.</typeparam>
        /// <param name="expectedMinimum">The inclusive expected minimum value.</param>
        /// <param name="actual">The actual value.</param>
        public void IsGreaterOrEqual<T>(T expectedMinimum, T actual)
            where T : IComparable
            => IsGreaterOrEqual(expectedMinimum, actual, null);

        /// <summary>
        /// Verifies that a value is greater or equal to another value.
        /// </summary>
        /// <typeparam name="T">The type of value to compare.</typeparam>
        /// <param name="expectedMinimum">The inclusive expected minimum value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public void IsGreaterOrEqual<T>(T expectedMinimum, T actual, string message)
            where T : IComparable
        {
            if (Comparer<T>.Default.Compare(expectedMinimum, actual) > 0)
                ThrowAssertFailed($"Assert.IsGreaterOrEqual failed. ExpectedMinimum:<{expectedMinimum}>. Actual:<{actual}>. {message}");
        }

        /// <summary>
        /// Verifies that a value is greater than another value.
        /// </summary>
        /// <typeparam name="T">The type of value to compare.</typeparam>
        /// <param name="expectedMinimum">The exclusive expected minimum value.</param>
        /// <param name="actual">The actual value.</param>
        public void IsGreater<T>(T expectedMinimum, T actual)
            where T : IComparable
            => IsGreater(expectedMinimum, actual, null);

        /// <summary>
        /// Verifies that a value is greater than another value.
        /// </summary>
        /// <typeparam name="T">The type of value to compare.</typeparam>
        /// <param name="expectedMinimum">The exclusive expected minimum value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public void IsGreater<T>(T expectedMinimum, T actual, string message)
            where T : IComparable
        {
            if (Comparer<T>.Default.Compare(expectedMinimum, actual) >= 0)
                ThrowAssertFailed($"Assert.IsGreater failed. ExpectedMinimum:<{expectedMinimum}>. Actual:<{actual}>. {message}");
        }

        /// <summary>
        /// Verifies that a value is smaller or equal to another value.
        /// </summary>
        /// <typeparam name="T">The type of value to compare.</typeparam>
        /// <param name="expectedMaximum">The inclusive expected maximum value.</param>
        /// <param name="actual">The actual value.</param>
        public void IsSmallerOrEqual<T>(T expectedMaximum, T actual)
            where T : IComparable
            => IsSmallerOrEqual(expectedMaximum, actual, null);

        /// <summary>
        /// Verifies that a value is smaller or equal to another value.
        /// </summary>
        /// <typeparam name="T">The type of value to compare.</typeparam>
        /// <param name="expectedMaximum">The inclusive expected maximum value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public void IsSmallerOrEqual<T>(T expectedMaximum, T actual, string message)
            where T : IComparable
        {
            if (Comparer<T>.Default.Compare(expectedMaximum, actual) < 0)
                ThrowAssertFailed($"Assert.IsSmallerOrEqual failed. ExpectedMaximum:<{expectedMaximum}>. Actual:<{actual}>. {message}");
        }

        /// <summary>
        /// Verifies that a value is smaller than another value.
        /// </summary>
        /// <typeparam name="T">The type of value to compare.</typeparam>
        /// <param name="expectedMaximum">The exclusive expected maximum value.</param>
        /// <param name="actual">The actual value.</param>
        public void IsSmaller<T>(T expectedMaximum, T actual)
            where T : IComparable
            => IsSmaller(expectedMaximum, actual, null);

        /// <summary>
        /// Verifies that a value is smaller than another value.
        /// </summary>
        /// <typeparam name="T">The type of value to compare.</typeparam>
        /// <param name="expectedMaximum">The exclusive expected maximum value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public void IsSmaller<T>(T expectedMaximum, T actual, string message)
            where T : IComparable
        {
            if (Comparer<T>.Default.Compare(expectedMaximum, actual) <= 0)
                ThrowAssertFailed($"Assert.IsSmaller failed. ExpectedMaximum:<{expectedMaximum}>. Actual:<{actual}>. {message}");
        }

        /// <summary>
        /// Verifies that a value is between two other values.
        /// </summary>
        /// <typeparam name="T">The type of value to compare.</typeparam>
        /// <param name="expectedMinimum">The inclusive expected minimum value.</param>
        /// <param name="expectedMaximum">The inclusive expected maximum value.</param>
        /// <param name="actual">The actual value.</param>
        public void IsBetweenOrEqual<T>(T expectedMinimum, T expectedMaximum, T actual)
            where T : IComparable
            => IsBetweenOrEqual(expectedMinimum, expectedMaximum, actual, null);

        /// <summary>
        /// Verifies that a value is between two other values.
        /// </summary>
        /// <typeparam name="T">The type of value to compare.</typeparam>
        /// <param name="expectedMinimum">The inclusive expected minimum value.</param>
        /// <param name="expectedMaximum">The inclusive expected maximum value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public void IsBetweenOrEqual<T>(T expectedMinimum, T expectedMaximum, T actual, string message)
            where T : IComparable
        {
            if (Comparer<T>.Default.Compare(expectedMinimum, actual) > 0 || Comparer<T>.Default.Compare(expectedMaximum, actual) < 0)
                ThrowAssertFailed($"Assert.IsBetweenOrEqual failed. ExpectedMinimum:<{expectedMinimum}>. ExpectedMaximum:<{expectedMaximum}>. Actual:<{actual}>. {message}");
        }

        /// <summary>
        /// Verifies that a value is between two other values.
        /// </summary>
        /// <typeparam name="T">The type of value to compare.</typeparam>
        /// <param name="expectedMinimum">The exclusive expected minimum value.</param>
        /// <param name="expectedMaximum">The exclusive expected maximum value.</param>
        /// <param name="actual">The actual value.</param>
        public void IsBetween<T>(T expectedMinimum, T expectedMaximum, T actual)
            where T : IComparable
            => IsBetween(expectedMinimum, expectedMaximum, actual, null);

        /// <summary>
        /// Verifies that a value is between two other values.
        /// </summary>
        /// <typeparam name="T">The type of value to compare.</typeparam>
        /// <param name="expectedMinimum">The exclusive expected minimum value.</param>
        /// <param name="expectedMaximum">The exclusive expected maximum value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public void IsBetween<T>(T expectedMinimum, T expectedMaximum, T actual, string message)
            where T : IComparable
        {
            if (Comparer<T>.Default.Compare(expectedMinimum, actual) >= 0 || Comparer<T>.Default.Compare(expectedMaximum, actual) <= 0)
                ThrowAssertFailed($"Assert.IsBetween failed. ExpectedMinimum:<{expectedMinimum}>. ExpectedMaximum:<{expectedMaximum}>. Actual:<{actual}>. {message}");
        }

        /// <summary>
        /// Verifies that a string value is null or an empty string.
        /// </summary>
        /// <param name="actual">The value to verify.</param>
        public void IsNullOrEmpty(string actual) => IsNullOrEmpty(actual, null);

        /// <summary>
        /// Verifies that a string value is null or an empty string.
        /// </summary>
        /// <param name="actual">The value to verify.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public void IsNullOrEmpty(string actual, string message)
        {
            if (!string.IsNullOrEmpty(actual))
                ThrowAssertFailed($"Assert.IsNullOrEmpty failed. {message}");
        }

        /// <summary>
        /// Verifies that a string value string is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="actual">The value to verify.</param>
        public void IsNullOrWhitespace(string actual) => IsNullOrWhitespace(actual, null);

        /// <summary>
        /// Verifies that a string value string is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="actual">The value to verify.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public void IsNullOrWhitespace(string actual, string message)
        {
            if (!string.IsNullOrWhiteSpace(actual))
                ThrowAssertFailed($"Assert.IsNullOrWhitespace failed. {message}");
        }

        /// <summary>
        /// Verifies that a string value is not null nor an empty string.
        /// </summary>
        /// <param name="actual">The value to verify.</param>
        public void IsNotNullOrEmpty(string actual) => IsNotNullOrEmpty(actual, null);

        /// <summary>
        /// Verifies that a string value is not null nor an empty string.
        /// </summary>
        /// <param name="actual">The value to verify.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public void IsNotNullOrEmpty(string actual, string message)
        {
            if (string.IsNullOrEmpty(actual))
                ThrowAssertFailed($"Assert.IsNullOrEmpty failed. {message}");
        }

        /// <summary>
        /// Verifies that a string value string is not null, empty, nor consists only of white-space characters.
        /// </summary>
        /// <param name="actual">The value to verify.</param>
        public void IsNotNullOrWhitespace(string actual) => IsNotNullOrWhitespace(actual, null);

        /// <summary>
        /// Verifies that a string value string is not null, empty, nor consists only of white-space characters.
        /// </summary>
        /// <param name="actual">The value to verify.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public void IsNotNullOrWhitespace(string actual, string message)
        {
            if (string.IsNullOrWhiteSpace(actual))
                ThrowAssertFailed($"Assert.IsNotNullOrWhitespace failed. {message}");
        }

        /// <summary>
        /// Verifies that a enumerable is null or has no elements.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable elements.</typeparam>
        /// <param name="actual">The value to verify.</param>
        public void IsNullOrEmpty<T>(IEnumerable<T> actual) => IsNullOrEmpty(actual, null);

        /// <summary>
        /// Verifies that a enumerable is null or has no elements.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable elements.</typeparam>
        /// <param name="actual">The value to verify.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public void IsNullOrEmpty<T>(IEnumerable<T> actual, string message)
        {
            if (actual != null && actual.Any())
                ThrowAssertFailed($"Assert.IsNullOrEmpty failed. {message}");
        }

        /// <summary>
        /// Verifies that a enumerable is not null and has at least one element.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable elements.</typeparam>
        /// <param name="actual">The value to verify.</param>
        public void IsNotNullOrEmpty<T>(IEnumerable<T> actual) => IsNotNullOrEmpty(actual, null);

        /// <summary>
        /// Verifies that a enumerable is not null and has at least one element.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable elements.</typeparam>
        /// <param name="actual">The value to verify.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public void IsNotNullOrEmpty<T>(IEnumerable<T> actual, string message)
        {
            if (actual == null || !actual.Any())
                ThrowAssertFailed($"Assert.IsNotNullOrEmpty failed. {message}");
        }

        /// <summary>
        /// Verifies that a enumerable contains the expected items.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable elements.</typeparam>
        /// <param name="expected">The elements that are expected to be contained in the actual enumerable.</param>
        /// <param name="actual">The actual enumerable to test.</param>
        /// <param name="assertSorting">Determines wether the elements has to be contained in the correct order.</param>
        /// <param name="matchExactly">Determines wether the actual enumerable can contain elements that are not specified in the expected elements.</param>
        public void ArrayElements<T>(IEnumerable<T> expected, IEnumerable<T> actual, bool assertSorting, bool matchExactly) => ArrayElements(expected, actual, null, null, null, assertSorting, matchExactly);

        /// <summary>
        /// Verifies that a enumerable contains the expected items.
        /// </summary>
        /// <typeparam name="TExpected">The type of the expected enumerable elements.</typeparam>
        /// <typeparam name="TActual">The type of the actual enumerable elements.</typeparam>
        /// <param name="expected">The elements that are expected to be contained in the actual enumerable.</param>
        /// <param name="actual">The actual enumerable to test.</param>
        /// <param name="matchFunction">A function that is used to determine if two elements match.</param>
        /// <param name="assertSorting">Determines wether the elements has to be contained in the correct order.</param>
        /// <param name="matchExactly">Determines wether the actual enumerable can contain elements that are not specified in the expected elements.</param>
        public void ArrayElements<TExpected, TActual>(IEnumerable<TExpected> expected, IEnumerable<TActual> actual, Func<TExpected, TActual, bool> matchFunction, bool assertSorting, bool matchExactly) => ArrayElements(expected, actual, matchFunction, null, null, assertSorting, matchExactly);

        /// <summary>
        /// Verifies that a enumerable contains the expected items.
        /// </summary>
        /// <typeparam name="TExpected">The type of the expected enumerable elements.</typeparam>
        /// <typeparam name="TActual">The type of the actual enumerable elements.</typeparam>
        /// <param name="expected">The elements that are expected to be contained in the actual enumerable.</param>
        /// <param name="actual">The actual enumerable to test.</param>
        /// <param name="matchFunction">A function that is used to determine if two elements match.</param>
        /// <param name="expectedDisplaySelector">A function that is used for displaying an item from the expected item list in the error message.</param>
        /// <param name="actualDisplaySelector">A function that is used for displaying an item from the actual item list in the error message.</param>
        /// <param name="assertSorting">Determines wether the elements has to be contained in the correct order.</param>
        /// <param name="matchExactly">Determines wether the actual enumerable can contain elements that are not specified in the expected elements.</param>
        public void ArrayElements<TExpected, TActual>(IEnumerable<TExpected> expected, IEnumerable<TActual> actual, Func<TExpected, TActual, bool> matchFunction, Func<TExpected, string> expectedDisplaySelector, Func<TActual, string> actualDisplaySelector, bool assertSorting, bool matchExactly)
        {
            bool IsMatching(TExpected a, TActual b) => matchFunction?.Invoke(a, b) ?? Equals(a, b);
            string ActualToString(TActual a) => actualDisplaySelector?.Invoke(a) ?? a?.ToString() ?? "(null)";
            string ExpectedToString(TExpected e) => expectedDisplaySelector?.Invoke(e) ?? e?.ToString() ?? "(null)";

            var actualList = actual.ToList();
            var expectedList = expected.ToList();
            if (matchExactly)
                AreEqual(expectedList.Count, actualList.Count, "The actual list has not the correct number of items.");

            int elementIndex = 0;
            var notMatchedActualValues = new List<int>();
            for (int i = 0; i < actualList.Count; i++)
            {
                var matchedIndex = assertSorting
                    ? IsMatching(expectedList[0], actualList[i]) ? 0 : -1
                    : expectedList.IndexOf(x => IsMatching(x, actualList[i]));

                if (matchedIndex < 0)
                {
                    if (matchExactly)
                        notMatchedActualValues.Add(i);
                }
                else
                {
                    expectedList.RemoveAt(matchedIndex);
                    actualList.RemoveAt(i);
                    i--;
                }

                elementIndex++;
            }

            var errors = new List<string>();
            if (expectedList.Count > 0)
                errors.Add($"The following elements are not contained in the actual enumerable: \n    - {string.Join("\n    - ", expectedList.Select(x => ExpectedToString(x)))}");
            if (notMatchedActualValues.Count > 0)
                errors.Add($"The following elements were not matched in the actual enumerable: \n    - {string.Join("\n    - ", notMatchedActualValues.Select(x => $"[{x}] {ActualToString(actualList[x])}"))}");
            if (errors.Count > 0)
                Fail(string.Join("\n", errors));
        }

        /// <summary>
        /// Verifies that an action throws a specific exception.
        /// </summary>
        /// <typeparam name="T">The expected type of the exception.</typeparam>
        /// <param name="action">The action to verify.</param>
        public void ThrowsException<T>(Action action)
            where T : Exception
            => ThrowsException<T>(action, false, false);

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
                        ThrowAssertFailed($"The exception that occurred does not have an inner exception.", ex);
                    ex = ex.InnerException;
                }

                if ((matchExactly && ex.GetType() != typeof(T)) || (!matchExactly && !typeof(T).IsInstanceOfType(ex)))
                    ThrowAssertFailed($"The action was expected to throw an exception{(matchExactly ? " exactly" : string.Empty)} of type \"{typeof(T).FullName}\", but did throw an exception of type \"{ex.GetType().FullName}\".", ex);
            }

            if (!hasException)
                ThrowAssertFailed($"The action was expected to throw an exception of type \"{typeof(T).FullName}\", but did not throw any exceptions.");
        }

        /// <summary>
        /// Verifies that a string contains a specific other string.
        /// </summary>
        /// <param name="expected">The string that is expected to be contained in the <paramref name="actual"/> string.</param>
        /// <param name="actual">The string to verify.</param>
        public void Contains(string expected, string actual) => Contains(expected, actual, null);

        /// <summary>
        /// Verifies that a string contains a specific other string.
        /// </summary>
        /// <param name="expected">The string that is expected to be contained in the <paramref name="actual"/> string.</param>
        /// <param name="actual">The string to verify.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public void Contains(string expected, string actual, string message)
        {
            if (!string.IsNullOrEmpty(expected) && actual?.Contains(expected) != true)
                ThrowAssertFailed($"Assert.IsNullOrEmpty failed. ExpectedContent:<{expected}>. Actual<{actual ?? "(null)"}> {message}");
        }

        private void ThrowAssertFailed(string message)
            => ThrowAssertFailed(message, null);
        private void ThrowAssertFailed(string message, Exception innerException)
        {
            var ex = new AssertFailedException(message, innerException);
            LogFailedAssertion(ex, LogLevel.Error);
            if (innerException != null)
            {
                Logger.LogDebug("The Assertion failed due to the following exception: " + innerException);
            }

            throw ex;
        }

        private void LogFailedAssertion(AssertFailedException exception, LogLevel logLevel)
            => LogFailedAssertion(exception.Message + Environment.NewLine + new StackTrace(1), logLevel);
        private void LogFailedAssertion(string message, LogLevel logLevel)
        {
            if (IsFailedAssertionLogEnabled)
                Logger.Log(logLevel, "Failed assertion: " + message);
        }

        private IDisposable EnterNonLoggingScope()
        {
            IsFailedAssertionLogEnabled = false;
            return new ActionOnDispose(() => IsFailedAssertionLogEnabled = true);
        }

        #region Microsoft.VisualStudio.TestTools.UnitTesting.Assert
#if SLIM
#pragma warning disable SA1501 // Statement should not be on a single line
#endif

        /// <summary>
        /// Tests whether the specified floats are equal and throws an exception
        /// if they are not equal.
        /// </summary>
        /// <param name="expected">
        /// The first float to compare. This is the float the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second float to compare. This is the float produced by the code under test.
        /// </param>
        /// <param name="delta">
        /// The required accuracy. An exception will be thrown only if
        /// <paramref name="actual"/> is different than <paramref name="expected"/>
        /// by more than <paramref name="delta"/>.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is different than <paramref name="expected"/> by more than
        /// <paramref name="delta"/>. The message is shown in test results.
        /// </param>
        public void AreEqual(float expected, float actual, float delta, string message)
#if SLIM
            => RunAssertAction(() => { if (Math.Abs(expected - actual) > delta) { throw new AssertFailedException($"{message ?? "Assert.AreEqual failed."} Expected: {expected}, Actual: {actual}"); } }, true);
#else
            => RunAssertAction(() => Assert.AreEqual(expected, actual, delta, message), true);
#endif

        /// <summary>
        /// Tests whether the specified objects are equal and throws an exception
        /// if the two objects are not equal. Different numeric types are treated
        /// as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <param name="expected">
        /// The first object to compare. This is the object the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second object to compare. This is the object produced by the code under test.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is not equal to <paramref name="expected"/>. The message is shown in
        /// test results.
        /// </param>
        public void AreEqual(object expected, object actual, string message)
#if SLIM
            => RunAssertAction(() => { if (!Equals(expected, actual)) { throw new AssertFailedException($"{message ?? "Assert.AreEqual failed."} Expected: {expected}, Actual: {actual}"); } }, true);
#else
            => RunAssertAction(() => Assert.AreEqual(expected, actual, message), true);
#endif

        /// <summary>
        /// Tests whether the specified objects are equal and throws an exception
        /// if the two objects are not equal. Different numeric types are treated
        /// as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <param name="expected">
        /// The first object to compare. This is the object the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second object to compare. This is the object produced by the code under test.
        /// </param>
        public void AreEqual(object expected, object actual)
#if SLIM
            => AreEqual(expected, actual, null);
#else
            => RunAssertAction(() => Assert.AreEqual(expected, actual), true);
#endif

        /// <summary>
        /// Tests whether the specified doubles are equal and throws an exception
        /// if they are not equal.
        /// </summary>
        /// <param name="expected">
        /// The first double to compare. This is the double the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second double to compare. This is the double produced by the code under test.
        /// </param>
        /// <param name="delta">
        /// The required accuracy. An exception will be thrown only if
        /// <paramref name="actual"/> is different than <paramref name="expected"/>
        /// by more than <paramref name="delta"/>.
        /// </param>
        public void AreEqual(double expected, double actual, double delta)
#if SLIM
            => AreEqual(expected, actual, delta, null);
#else
            => RunAssertAction(() => Assert.AreEqual(expected, actual, delta), true);
#endif

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception
        /// if the two values are not equal. Different numeric types are treated
        /// as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <typeparam name="T">
        /// The type of values to compare.
        /// </typeparam>
        /// <param name="expected">
        /// The first value to compare. This is the value the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second value to compare. This is the value produced by the code under test.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is not equal to <paramref name="expected"/>. The message is shown in
        /// test results.
        /// </param>
        public void AreEqual<T>(T expected, T actual, string message)
#if SLIM
            => RunAssertAction(() => { if (Equals(expected, actual)) { throw new AssertFailedException($"{message ?? "Assert.AreEqual failed."} Expected: {expected}, Actual: {actual}"); } }, true);
#else
            => RunAssertAction(() => Assert.AreEqual(expected, actual, message), true);
#endif

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception
        /// if the two values are not equal. Different numeric types are treated
        /// as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <typeparam name="T">
        /// The type of values to compare.
        /// </typeparam>
        /// <param name="expected">
        /// The first value to compare. This is the value the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second value to compare. This is the value produced by the code under test.
        /// </param>
        public void AreEqual<T>(T expected, T actual)
#if SLIM
            => AreEqual(expected, actual, null);
#else
            => RunAssertAction(() => Assert.AreEqual(expected, actual), true);
#endif

        /// <summary>
        /// Tests whether the specified doubles are equal and throws an exception
        /// if they are not equal.
        /// </summary>
        /// <param name="expected">
        /// The first double to compare. This is the double the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second double to compare. This is the double produced by the code under test.
        /// </param>
        /// <param name="delta">
        /// The required accuracy. An exception will be thrown only if
        /// <paramref name="actual"/> is different than <paramref name="expected"/>
        /// by more than <paramref name="delta"/>.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is different than <paramref name="expected"/> by more than
        /// <paramref name="delta"/>. The message is shown in test results.
        /// </param>
        public void AreEqual(double expected, double actual, double delta, string message)
#if SLIM
            => RunAssertAction(() => { if (Math.Abs(expected - actual) > delta) { throw new AssertFailedException($"{message ?? "Assert.AreEqual failed."} Expected: {expected}, Actual: {actual}"); } }, true);
#else
            => RunAssertAction(() => Assert.AreEqual(expected, actual, delta, message), true);
#endif

        /// <summary>
        /// Tests whether the specified strings are equal and throws an exception
        /// if they are not equal. The invariant culture is used for the comparison.
        /// </summary>
        /// <param name="expected">
        /// The first string to compare. This is the string the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second string to compare. This is the string produced by the code under test.
        /// </param>
        /// <param name="ignoreCase">
        /// A Boolean indicating a case-sensitive or insensitive comparison. (true
        /// indicates a case-insensitive comparison.)
        /// </param>
        public void AreEqual(string expected, string actual, bool ignoreCase)
#if SLIM
            => AreEqual(expected, actual, ignoreCase, null);
#else
            => RunAssertAction(() => Assert.AreEqual(expected, actual, ignoreCase), true);
#endif

        /// <summary>
        /// Tests whether the specified floats are equal and throws an exception
        /// if they are not equal.
        /// </summary>
        /// <param name="expected">
        /// The first float to compare. This is the float the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second float to compare. This is the float produced by the code under test.
        /// </param>
        /// <param name="delta">
        /// The required accuracy. An exception will be thrown only if
        /// <paramref name="actual"/> is different than <paramref name="expected"/>
        /// by more than <paramref name="delta"/>.
        /// </param>
        public void AreEqual(float expected, float actual, float delta)
#if SLIM
            => AreEqual(expected, actual, delta, null);
#else
            => RunAssertAction(() => Assert.AreEqual(expected, actual, delta), true);
#endif

        /// <summary>
        /// Tests whether the specified strings are equal and throws an exception
        /// if they are not equal. The invariant culture is used for the comparison.
        /// </summary>
        /// <param name="expected">
        /// The first string to compare. This is the string the tests expects.
        /// </param>
        /// <param name="actual">
        /// The second string to compare. This is the string produced by the code under test.
        /// </param>
        /// <param name="ignoreCase">
        /// A Boolean indicating a case-sensitive or insensitive comparison. (true
        /// indicates a case-insensitive comparison.)
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is not equal to <paramref name="expected"/>. The message is shown in
        /// test results.
        /// </param>
        public void AreEqual(string expected, string actual, bool ignoreCase, string message)
#if SLIM
            => RunAssertAction(() => { if (string.Equals(expected, actual, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)) { throw new AssertFailedException($"{message ?? "Assert.AreEqual failed."} Expected: {expected}, Actual: {actual}"); } }, true);
#else
            => RunAssertAction(() => Assert.AreEqual(expected, actual, ignoreCase, message), true);
#endif

        /// <summary>
        /// Tests whether the specified doubles are unequal and throws an exception
        /// if they are equal.
        /// </summary>
        /// <param name="notExpected">
        /// The first double to compare. This is the double the test expects not to
        /// match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second double to compare. This is the double produced by the code under test.
        /// </param>
        /// <param name="delta">
        /// The required accuracy. An exception will be thrown only if
        /// <paramref name="actual"/> is different than <paramref name="notExpected"/>
        /// by at most <paramref name="delta"/>.
        /// </param>
        public void AreNotEqual(double notExpected, double actual, double delta)
#if SLIM
            => AreNotEqual(notExpected, actual, delta, null);
#else
            => RunAssertAction(() => Assert.AreNotEqual(notExpected, actual, delta), true);
#endif

        /// <summary>
        /// Tests whether the specified strings are unequal and throws an exception
        /// if they are equal. The invariant culture is used for the comparison.
        /// </summary>
        /// <param name="notExpected">
        /// The first string to compare. This is the string the test expects not to
        /// match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second string to compare. This is the string produced by the code under test.
        /// </param>
        /// <param name="ignoreCase">
        /// A Boolean indicating a case-sensitive or insensitive comparison. (true
        /// indicates a case-insensitive comparison.)
        /// </param>
        public void AreNotEqual(string notExpected, string actual, bool ignoreCase)
#if SLIM
            => AreNotEqual(notExpected, actual, ignoreCase, null);
#else
            => RunAssertAction(() => Assert.AreNotEqual(notExpected, actual, ignoreCase), true);
#endif

        /// <summary>
        /// Tests whether the specified strings are unequal and throws an exception
        /// if they are equal. The invariant culture is used for the comparison.
        /// </summary>
        /// <param name="notExpected">
        /// The first string to compare. This is the string the test expects not to
        /// match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second string to compare. This is the string produced by the code under test.
        /// </param>
        /// <param name="ignoreCase">
        /// A Boolean indicating a case-sensitive or insensitive comparison. (true
        /// indicates a case-insensitive comparison.)
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is equal to <paramref name="notExpected"/>. The message is shown in
        /// test results.
        /// </param>
        public void AreNotEqual(string notExpected, string actual, bool ignoreCase, string message)
#if SLIM
            => RunAssertAction(() => { if (string.Equals(notExpected, actual, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)) { throw new AssertFailedException($"{message ?? "Assert.AreNotEqual failed."} NotExpected: {notExpected}, Actual: {actual}"); } }, true);
#else
            => RunAssertAction(() => Assert.AreNotEqual(notExpected, actual, ignoreCase, message), true);
#endif

        /// <summary>
        /// Tests whether the specified floats are unequal and throws an exception
        /// if they are equal.
        /// </summary>
        /// <param name="notExpected">
        /// The first float to compare. This is the float the test expects not to
        /// match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second float to compare. This is the float produced by the code under test.
        /// </param>
        /// <param name="delta">
        /// The required accuracy. An exception will be thrown only if
        /// <paramref name="actual"/> is different than <paramref name="notExpected"/>
        /// by at most <paramref name="delta"/>.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is equal to <paramref name="notExpected"/> or different by less than
        /// <paramref name="delta"/>. The message is shown in test results.
        /// </param>
        public void AreNotEqual(float notExpected, float actual, float delta, string message)
#if SLIM
            => RunAssertAction(() => { if (Math.Abs(notExpected - actual) <= delta) { throw new AssertFailedException($"{message ?? "Assert.AreNotEqual failed."} NotExpected: {notExpected}, Actual: {actual}"); } }, true);
#else
            => RunAssertAction(() => Assert.AreNotEqual(notExpected, actual, delta, message), true);
#endif

        /// <summary>
        /// Tests whether the specified doubles are unequal and throws an exception
        /// if they are equal.
        /// </summary>
        /// <param name="notExpected">
        /// The first double to compare. This is the double the test expects not to
        /// match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second double to compare. This is the double produced by the code under test.
        /// </param>
        /// <param name="delta">
        /// The required accuracy. An exception will be thrown only if
        /// <paramref name="actual"/> is different than <paramref name="notExpected"/>
        /// by at most <paramref name="delta"/>.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is equal to <paramref name="notExpected"/> or different by less than
        /// <paramref name="delta"/>. The message is shown in test results.
        /// </param>
        public void AreNotEqual(double notExpected, double actual, double delta, string message)
#if SLIM
            => RunAssertAction(() => { if (Math.Abs(notExpected - actual) <= delta) { throw new AssertFailedException($"{message ?? "Assert.AreNotEqual failed."} NotExpected: {notExpected}, Actual: {actual}"); } }, true);
#else
            => RunAssertAction(() => Assert.AreNotEqual(notExpected, actual, delta, message), true);
#endif

        /// <summary>
        /// Tests whether the specified floats are unequal and throws an exception
        /// if they are equal.
        /// </summary>
        /// <param name="notExpected">
        /// The first float to compare. This is the float the test expects not to
        /// match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second float to compare. This is the float produced by the code under test.
        /// </param>
        /// <param name="delta">
        /// The required accuracy. An exception will be thrown only if
        /// <paramref name="actual"/> is different than <paramref name="notExpected"/>
        /// by at most <paramref name="delta"/>.
        /// </param>
        public void AreNotEqual(float notExpected, float actual, float delta)
#if SLIM
            => AreNotEqual(notExpected, actual, delta, null);
#else
            => RunAssertAction(() => Assert.AreNotEqual(notExpected, actual, delta), true);
#endif

        /// <summary>
        /// Tests whether the specified objects are unequal and throws an exception
        /// if the two objects are equal. Different numeric types are treated
        /// as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <param name="notExpected">
        /// The first object to compare. This is the value the test expects not
        /// to match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second object to compare. This is the object produced by the code under test.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is equal to <paramref name="notExpected"/>. The message is shown in
        /// test results.
        /// </param>
        public void AreNotEqual(object notExpected, object actual, string message)
#if SLIM
            => RunAssertAction(() => { if (Equals(notExpected, actual)) { throw new AssertFailedException($"{message ?? "Assert.AreNotEqual failed."} NotExpected: {notExpected}, Actual: {actual}"); } }, true);
#else
            => RunAssertAction(() => Assert.AreNotEqual(notExpected, actual, message), true);
#endif

        /// <summary>
        /// Tests whether the specified objects are unequal and throws an exception
        /// if the two objects are equal. Different numeric types are treated
        /// as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <param name="notExpected">
        /// The first object to compare. This is the value the test expects not
        /// to match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second object to compare. This is the object produced by the code under test.
        /// </param>
        public void AreNotEqual(object notExpected, object actual)
#if SLIM
            => AreNotEqual(notExpected, actual, null);
#else
            => RunAssertAction(() => Assert.AreNotEqual(notExpected, actual), true);
#endif

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception
        /// if the two values are equal. Different numeric types are treated
        /// as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <typeparam name="T">
        /// The type of values to compare.
        /// </typeparam>
        /// <param name="notExpected">
        /// The first value to compare. This is the value the test expects not
        /// to match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second value to compare. This is the value produced by the code under test.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is equal to <paramref name="notExpected"/>. The message is shown in
        /// test results.
        /// </param>
        public void AreNotEqual<T>(T notExpected, T actual, string message)
#if SLIM
            => RunAssertAction(() => { if (Equals(notExpected, actual)) { throw new AssertFailedException($"{message ?? "Assert.AreNotEqual failed."} NotExpected: {notExpected}, Actual: {actual}"); } }, true);
#else
            => RunAssertAction(() => Assert.AreNotEqual(notExpected, actual, message), true);
#endif

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception
        /// if the two values are equal. Different numeric types are treated
        /// as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <typeparam name="T">
        /// The type of values to compare.
        /// </typeparam>
        /// <param name="notExpected">
        /// The first value to compare. This is the value the test expects not
        /// to match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second value to compare. This is the value produced by the code under test.
        /// </param>
        public void AreNotEqual<T>(T notExpected, T actual)
#if SLIM
            => AreNotEqual(notExpected, actual, null);
#else
            => RunAssertAction(() => Assert.AreNotEqual(notExpected, actual), true);
#endif

        /// <summary>
        /// Tests whether the specified objects refer to different objects and
        /// throws an exception if the two inputs refer to the same object.
        /// </summary>
        /// <param name="notExpected">
        /// The first object to compare. This is the value the test expects not
        /// to match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second object to compare. This is the value produced by the code under test.
        /// </param>
        public void AreNotSame(object notExpected, object actual)
#if SLIM
            => AreNotSame(notExpected, actual, null);
#else
            => RunAssertAction(() => Assert.AreNotSame(notExpected, actual), true);
#endif

        /// <summary>
        /// Tests whether the specified objects refer to different objects and
        /// throws an exception if the two inputs refer to the same object.
        /// </summary>
        /// <param name="notExpected">
        /// The first object to compare. This is the value the test expects not
        /// to match <paramref name="actual"/>.
        /// </param>
        /// <param name="actual">
        /// The second object to compare. This is the value produced by the code under test.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is the same as <paramref name="notExpected"/>. The message is shown in
        /// test results.
        /// </param>
        public void AreNotSame(object notExpected, object actual, string message)
#if SLIM
            => RunAssertAction(() => { if (ReferenceEquals(notExpected, actual)) { throw new AssertFailedException($"{message ?? "Assert.AreNotSame failed."} NotExpected: {notExpected}, Actual: {actual}"); } }, true);
#else
            => RunAssertAction(() => Assert.AreNotSame(notExpected, actual, message), true);
#endif

        /// <summary>
        /// Tests whether the specified objects both refer to the same object and
        /// throws an exception if the two inputs do not refer to the same object.
        /// </summary>
        /// <param name="expected">
        /// The first object to compare. This is the value the test expects.
        /// </param>
        /// <param name="actual">
        /// The second object to compare. This is the value produced by the code under test.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="actual"/>
        /// is not the same as <paramref name="expected"/>. The message is shown
        /// in test results.
        /// </param>
        public void AreSame(object expected, object actual, string message)
#if SLIM
            => RunAssertAction(() => { if (!ReferenceEquals(expected, actual)) { throw new AssertFailedException($"{message ?? "Assert.AreSame failed."} Expected: {expected}, Actual: {actual}"); } }, true);
#else
            => RunAssertAction(() => Assert.AreSame(expected, actual, message), true);
#endif

        /// <summary>
        /// Tests whether the specified objects both refer to the same object and
        /// throws an exception if the two inputs do not refer to the same object.
        /// </summary>
        /// <param name="expected">
        /// The first object to compare. This is the value the test expects.
        /// </param>
        /// <param name="actual">
        /// The second object to compare. This is the value produced by the code under test.
        /// </param>
        public void AreSame(object expected, object actual)
#if SLIM
            => AreSame(expected, actual, null);
#else
            => RunAssertAction(() => Assert.AreSame(expected, actual), true);
#endif

        /// <summary>
        /// Throws an AssertFailedException.
        /// </summary>
        /// <param name="message">
        /// The message to include in the exception. The message is shown in
        /// test results.
        /// </param>
        public void Fail(string message)
#if SLIM
            => throw new AssertFailedException(message ?? "Assert failed.");
#else
            => RunAssertAction(() => Assert.Fail(message), true);
#endif

        /// <summary>
        /// Throws an AssertFailedException.
        /// </summary>
        public void Fail()
#if SLIM
            => Fail(null);
#else
            => RunAssertAction(() => Assert.Fail(), true);
#endif

        /// <summary>
        /// Throws an AssertInconclusiveException.
        /// </summary>
        /// <param name="message">
        /// The message to include in the exception. The message is shown in
        /// test results.
        /// </param>
        public void Inconclusive(string message)
#if SLIM
            => throw new AssertInconclusiveException(message ?? "Assert inconclusive.");
#else
            => RunAssertAction(() => Assert.Inconclusive(message), true);
#endif

        /// <summary>
        /// Throws an AssertInconclusiveException.
        /// </summary>
        public void Inconclusive()
#if SLIM
            => Inconclusive(null);
#else
            => RunAssertAction(() => Assert.Inconclusive(), true);
#endif

        /// <summary>
        /// Tests whether the specified condition is false and throws an exception
        /// if the condition is true.
        /// </summary>
        /// <param name="condition">
        /// The condition the test expects to be false.
        /// </param>
        public void IsFalse(bool condition)
#if SLIM
            => IsFalse(condition, null);
#else
            => RunAssertAction(() => Assert.IsFalse(condition), true);
#endif

        /// <summary>
        /// Tests whether the specified condition is false and throws an exception
        /// if the condition is true.
        /// </summary>
        /// <param name="condition">
        /// The condition the test expects to be false.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="condition"/>
        /// is true. The message is shown in test results.
        /// </param>
        public void IsFalse(bool condition, string message)
#if SLIM
            => RunAssertAction(() => { if (condition) { throw new AssertFailedException(message ?? "Assert.IsFalse failed."); } }, true);
#else
            => RunAssertAction(() => Assert.IsFalse(condition, message), true);
#endif

        /// <summary>
        /// Tests whether the specified object is an instance of the expected
        /// type and throws an exception if the expected type is not in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <param name="value">
        /// The object the test expects to be of the specified type.
        /// </param>
        /// <param name="expectedType">
        /// The expected type of <paramref name="value"/>.
        /// </param>
        public void IsInstanceOfType(object value, Type expectedType)
#if SLIM
            => IsInstanceOfType(value, expectedType);
#else
            => RunAssertAction(() => Assert.IsInstanceOfType(value, expectedType), true);
#endif

        /// <summary>
        /// Tests whether the specified object is an instance of the expected
        /// type and throws an exception if the expected type is not in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <param name="value">
        /// The object the test expects to be of the specified type.
        /// </param>
        /// <param name="expectedType">
        /// The expected type of <paramref name="value"/>.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// is not an instance of <paramref name="expectedType"/>. The message is
        /// shown in test results.
        /// </param>
        public void IsInstanceOfType(object value, Type expectedType, string message)
#if SLIM
            => RunAssertAction(() => { if (value == null || !expectedType.IsInstanceOfType(value)) { throw new AssertFailedException($"{message ?? "Assert.IsInstanceOfType failed."} ExpectedType: {expectedType.FullName}, ActualType: {value?.GetType().FullName ?? "(null)"}"); } }, true);
#else
            => RunAssertAction(() => Assert.IsInstanceOfType(value, expectedType, message), true);
#endif

        /// <summary>
        /// Tests whether the specified object is not an instance of the wrong
        /// type and throws an exception if the specified type is in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <param name="value">
        /// The object the test expects not to be of the specified type.
        /// </param>
        /// <param name="wrongType">
        /// The type that <paramref name="value"/> should not be.
        /// </param>
        public void IsNotInstanceOfType(object value, Type wrongType)
#if SLIM
            => IsNotInstanceOfType(value, wrongType, null);
#else
            => RunAssertAction(() => Assert.IsNotInstanceOfType(value, wrongType), true);
#endif

        /// <summary>
        /// Tests whether the specified object is not an instance of the wrong
        /// type and throws an exception if the specified type is in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <param name="value">
        /// The object the test expects not to be of the specified type.
        /// </param>
        /// <param name="wrongType">
        /// The type that <paramref name="value"/> should not be.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// is an instance of <paramref name="wrongType"/>. The message is shown
        /// in test results.
        /// </param>
        public void IsNotInstanceOfType(object value, Type wrongType, string message)
#if SLIM
            => RunAssertAction(() => { if (value == null || wrongType.IsInstanceOfType(value)) { throw new AssertFailedException($"{message ?? "Assert.IsNotInstanceOfType failed."} WrongType: {wrongType.FullName}"); } }, true);
#else
            => RunAssertAction(() => Assert.IsNotInstanceOfType(value, wrongType, message), true);
#endif

        /// <summary>
        /// Tests whether the specified object is non-null and throws an exception
        /// if it is null.
        /// </summary>
        /// <param name="value">
        /// The object the test expects not to be null.
        /// </param>
        public void IsNotNull(object value)
#if SLIM
            => IsNotNull(value, null);
#else
            => RunAssertAction(() => Assert.IsNotNull(value), true);
#endif

        /// <summary>
        /// Tests whether the specified object is non-null and throws an exception
        /// if it is null.
        /// </summary>
        /// <param name="value">
        /// The object the test expects not to be null.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// is null. The message is shown in test results.
        /// </param>
        public void IsNotNull(object value, string message)
#if SLIM
            => RunAssertAction(() => { if (value is null) { throw new AssertFailedException(message ?? "Assert.IsNotNull failed."); } }, true);
#else
            => RunAssertAction(() => Assert.IsNotNull(value, message), true);
#endif

        /// <summary>
        /// Tests whether the specified object is null and throws an exception
        /// if it is not.
        /// </summary>
        /// <param name="value">
        /// The object the test expects to be null.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="value"/>
        /// is not null. The message is shown in test results.
        /// </param>
        public void IsNull(object value, string message)
#if SLIM
            => RunAssertAction(() => { if (value is not null) { throw new AssertFailedException(message ?? "Assert.IsNull failed."); } }, true);
#else
            => RunAssertAction(() => Assert.IsNull(value, message), true);
#endif

        /// <summary>
        /// Tests whether the specified object is null and throws an exception
        /// if it is not.
        /// </summary>
        /// <param name="value">
        /// The object the test expects to be null.
        /// </param>
        public void IsNull(object value)
#if SLIM
            => IsNull(value, null);
#else
            => RunAssertAction(() => Assert.IsNull(value), true);
#endif

        /// <summary>
        /// Tests whether the specified condition is true and throws an exception
        /// if the condition is false.
        /// </summary>
        /// <param name="condition">
        /// The condition the test expects to be true.
        /// </param>
        public void IsTrue(bool condition)
#if SLIM
            => IsTrue(condition, null);
#else
            => RunAssertAction(() => Assert.IsTrue(condition), true);
#endif

        /// <summary>
        /// Tests whether the specified condition is true and throws an exception
        /// if the condition is false.
        /// </summary>
        /// <param name="condition">
        /// The condition the test expects to be true.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="condition"/>
        /// is false. The message is shown in test results.
        /// </param>
        public void IsTrue(bool condition, string message)
#if SLIM
            => RunAssertAction(() => { if (!condition) { throw new AssertFailedException(message ?? "Assert.IsTrue failed."); } }, true);
#else
            => RunAssertAction(() => Assert.IsTrue(condition, message), true);
#endif

        /// <summary>
        /// Replaces null characters ('\0') with "\\0".
        /// </summary>
        /// <param name="input">
        /// The string to search.
        /// </param>
        /// <returns>
        /// The converted string with null characters replaced by "\\0".
        /// </returns>
        public string ReplaceNullChars(string input)
#if SLIM
            => input?.Replace("\0", "\\0");
#else
            => Assert.ReplaceNullChars(input);
#endif
        #endregion
    }

#if SLIM
#pragma warning restore SA1501 // Statement should not be on a single line
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1402 // File may only contain a single type
    public class UnitTestAssertException : Exception
    {
        public UnitTestAssertException()
        {
        }

        public UnitTestAssertException(string message)
            : base(message)
        {
        }

        public UnitTestAssertException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UnitTestAssertException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }

    public class AssertFailedException : UnitTestAssertException
    {
        public AssertFailedException()
        {
        }

        public AssertFailedException(string message)
            : base(message)
        {
        }

        public AssertFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AssertFailedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }

    public class AssertInconclusiveException : UnitTestAssertException
    {
        public AssertInconclusiveException()
        {
        }

        public AssertInconclusiveException(string message)
            : base(message)
        {
        }

        public AssertInconclusiveException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AssertInconclusiveException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
#pragma warning restore SA1402 // File may only contain a single type
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#endif
}
