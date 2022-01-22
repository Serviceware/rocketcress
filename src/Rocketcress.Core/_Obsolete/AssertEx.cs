using Rocketcress.Core.Extensions;

namespace Rocketcress.Core;

/// <summary>
/// Represents a extendable class, that contains methods for asserting different results.
/// </summary>
[Obsolete("Use Rocketcress.Core.Assert instead.")]
public class AssertEx : Assert
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AssertEx"/> class.
    /// </summary>
    public AssertEx()
        : base(true)
    {
    }
}

/// <summary>
/// Assertion class for Rocketcress tests.
/// </summary>
public partial class Assert
{
    /// <summary>
    /// Fails the assertion without checking any conditions. Displays a message.
    /// </summary>
    /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
    /// <param name="assert">Determines wether to throw a AssertFailedException.</param>
    /// <returns>Returns false.</returns>
    [Obsolete("Use .When(assert).That(x => x.Fail(message)) instead.")]
    public bool Fail(string? message, bool assert)
        => When(assert).That(x => x.Fail(message));

    /// <summary>
    /// Fails the assertion without checking any conditions. Displays a message.
    /// </summary>
    /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
    /// <param name="assert">Determines wether to throw a AssertFailedException.</param>
    /// <param name="result">The result that should be returned by this method.</param>
    /// <returns>Returns the value of the result parameter.</returns>
    [Obsolete("Use .When(assert).WithResult(result, result).That(x => x.Fail(message)) instead.")]
    public bool Fail(string message, bool assert, bool result)
        => When(assert).WithResult(result, result).That(x => x.Fail(message));

    /// <summary>
    /// Fails the assertion without checking any conditions. Displays a message.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
    /// <param name="assert">Determines wether to throw a AssertFailedException.</param>
    /// <returns>Returns the default value of the specified return type.</returns>
    [Obsolete("Use .When(assert).That<T>(x => x.Fail(message)) instead.")]
    public T? Fail<T>(string message, bool assert)
        => When(assert).That<T>(x => x.Fail(message));

    /// <summary>
    /// Fails the assertion without checking any conditions. Displays a message.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
    /// <param name="assert">Determines wether to throw a AssertFailedException.</param>
    /// <param name="result">The result that should be returned by this method.</param>
    /// <returns>Returns the value of the result parameter.</returns>
    [Obsolete("Use .When(assert).WithResult<T>(result, result).That(x => x.Fail(message)) instead.")]
    public T Fail<T>(string message, bool assert, T result)
        => When(assert).WithResult(result, result).That(x => x.Fail(message));

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
    [Obsolete("Use .When(assert).That(x => x.AreEqual(expected, actual, message)) instead.")]
    public bool AreEqual<T>(T expected, T actual, string message, bool assert)
        => When(assert).That(x => x.AreEqual(expected, actual, message));

    /// <summary>
    /// Tests whether the specified object is non-null and throws an exception if it is null.
    /// </summary>
    /// <typeparam name="T">The return value type.</typeparam>
    /// <param name="value">The object the test expects not to be null.</param>
    /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
    /// <param name="assert">Determines wether to throw a AssertFailedException.</param>
    /// <param name="returnOnSuccess">Value that should be returned if the assert succeeds.</param>
    /// <returns>The value of <paramref name="returnOnSuccess"/> if the assert succeeds; otherwise the default value of <typeparamref name="T"/>.</returns>
    [Obsolete("Use .When(assert).WithResult(returnOnSuccess).That(x => x.IsNotNull(value, message)) instead.")]
    public T? IsNotNull<T>(object value, string message, bool assert, T returnOnSuccess)
        => When(assert).WithResult(returnOnSuccess).That(x => x.IsNotNull(value, message));

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
    [Obsolete("Use .When(assert).WithResult(returnOnSuccess, returnOnFail).That(x => x.IsNotNull(value, message)) instead.")]
    public T IsNotNull<T>(object value, string message, bool assert, T returnOnSuccess, T returnOnFail)
        => When(assert).WithResult(returnOnSuccess, returnOnFail).That(x => x.IsNotNull(value, message));

    /// <summary>
    /// Tests whether the specified object is null and throws an exception if it is not null.
    /// </summary>
    /// <typeparam name="T">The return value type.</typeparam>
    /// <param name="value">The object the test expects to be null.</param>
    /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
    /// <param name="assert">Determines wether to throw a AssertFailedException.</param>
    /// <param name="returnOnSuccess">Value that should be returned if the assert succeeds.</param>
    /// <returns>The value of <paramref name="returnOnSuccess"/> if the assert succeeds; otherwise the default value of <typeparamref name="T"/>.</returns>
    [Obsolete("Use .When(assert).WithResult(returnOnSuccess).That(x => x.IsNull(value, message)) instead.")]
    public T? IsNull<T>(object value, string message, bool assert, T returnOnSuccess)
        => When(assert).WithResult(returnOnSuccess).That(x => x.IsNull(value, message));

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
    [Obsolete("Use .When(assert).WithResult(returnOnSuccess, returnOnFail).That(x => x.IsNull(value, message)) instead.")]
    public T IsNull<T>(object value, string message, bool assert, T returnOnSuccess, T returnOnFail)
        => When(assert).WithResult(returnOnSuccess, returnOnFail).That(x => x.IsNull(value, message));

    /// <summary>
    /// Runs an action and handles any AssertFailedExceptions.
    /// </summary>
    /// <typeparam name="T">The type of the return value type.</typeparam>
    /// <param name="action">The action to execute.</param>
    /// <param name="assert">Determines wether the assert should be rethrown.</param>
    /// <param name="returnOnSuccess">The return value that is used if the action does not throw an AssertFailedException.</param>
    /// <param name="returnOnFail">The return value that is used if the action does throw an AssertFailedException.</param>
    /// <returns>Returns the returnOnSuccess value if the action does not throw an AssertFailedException; otherwise the returnOnFail value.</returns>
    [Obsolete("Use .When(assert).WithResult(returnOnSuccess, returnOnFail).That(action) instead.")]
    public T RunAssertAction<T>(Action action, bool assert, T returnOnSuccess, T returnOnFail)
        => When(assert).WithResult(returnOnSuccess, returnOnFail).That(x => action());

    /// <summary>
    /// Runs an action and handles any AssertFailedExceptions.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <param name="assert">Determines wether the assert should be rethrown.</param>
    /// <returns>Returns true if the action does not throw an AssertFailedException; otherwise false.</returns>
    [Obsolete("Use .When(assert).That(action) instead.")]
    public bool RunAssertAction(Action action, bool assert)
        => When(assert).That(x => action());

    /// <summary>
    /// Verifies that a value is greater or equal to another value.
    /// </summary>
    /// <typeparam name="T">The type of value to compare.</typeparam>
    /// <param name="expectedMinimum">The inclusive expected minimum value.</param>
    /// <param name="actual">The actual value.</param>
    [Obsolete("Use IsGreaterThanOrEqualTo instead.")]
    public void IsGreaterOrEqual<T>(T expectedMinimum, T actual)
        where T : IComparable
        => IsGreaterThanOrEqualTo(expectedMinimum, actual);

    /// <summary>
    /// Verifies that a value is greater or equal to another value.
    /// </summary>
    /// <typeparam name="T">The type of value to compare.</typeparam>
    /// <param name="expectedMinimum">The inclusive expected minimum value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
    [Obsolete("Use IsGreaterThanOrEqualTo instead.")]
    public void IsGreaterOrEqual<T>(T expectedMinimum, T actual, string message)
        where T : IComparable
        => IsGreaterThanOrEqualTo(expectedMinimum, actual, message);

    /// <summary>
    /// Verifies that a value is greater than another value.
    /// </summary>
    /// <typeparam name="T">The type of value to compare.</typeparam>
    /// <param name="expectedMinimum">The exclusive expected minimum value.</param>
    /// <param name="actual">The actual value.</param>
    [Obsolete("Use IsGreaterThan instead.")]
    public void IsGreater<T>(T expectedMinimum, T actual)
        where T : IComparable
        => IsGreaterThan(expectedMinimum, actual);

    /// <summary>
    /// Verifies that a value is greater than another value.
    /// </summary>
    /// <typeparam name="T">The type of value to compare.</typeparam>
    /// <param name="expectedMinimum">The exclusive expected minimum value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
    [Obsolete("Use IsGreaterThan instead.")]
    public void IsGreater<T>(T expectedMinimum, T actual, string message)
        where T : IComparable
        => IsGreaterThan(expectedMinimum, actual, message);

    /// <summary>
    /// Verifies that a value is smaller or equal to another value.
    /// </summary>
    /// <typeparam name="T">The type of value to compare.</typeparam>
    /// <param name="expectedMaximum">The inclusive expected maximum value.</param>
    /// <param name="actual">The actual value.</param>
    [Obsolete("Use IsSmallerThanOrEqualTo instead.")]
    public void IsSmallerOrEqual<T>(T expectedMaximum, T actual)
        where T : IComparable
        => IsSmallerThanOrEqualTo(expectedMaximum, actual);

    /// <summary>
    /// Verifies that a value is smaller or equal to another value.
    /// </summary>
    /// <typeparam name="T">The type of value to compare.</typeparam>
    /// <param name="expectedMaximum">The inclusive expected maximum value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
    [Obsolete("Use IsSmallerThanOrEqualTo instead.")]
    public void IsSmallerOrEqual<T>(T expectedMaximum, T actual, string message)
        where T : IComparable
        => IsSmallerThanOrEqualTo(expectedMaximum, actual, message);

    /// <summary>
    /// Verifies that a value is smaller than another value.
    /// </summary>
    /// <typeparam name="T">The type of value to compare.</typeparam>
    /// <param name="expectedMaximum">The exclusive expected maximum value.</param>
    /// <param name="actual">The actual value.</param>
    [Obsolete("Use IsSmallerThan instead.")]
    public void IsSmaller<T>(T expectedMaximum, T actual)
        where T : IComparable
        => IsSmallerThan(expectedMaximum, actual);

    /// <summary>
    /// Verifies that a value is smaller than another value.
    /// </summary>
    /// <typeparam name="T">The type of value to compare.</typeparam>
    /// <param name="expectedMaximum">The exclusive expected maximum value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
    [Obsolete("Use IsSmallerThan instead.")]
    public void IsSmaller<T>(T expectedMaximum, T actual, string message)
        where T : IComparable
        => IsSmallerThan(expectedMaximum, actual, message);

    /// <summary>
    /// Verifies that a enumerable contains the expected items.
    /// </summary>
    /// <typeparam name="T">The type of the enumerable elements.</typeparam>
    /// <param name="expected">The elements that are expected to be contained in the actual enumerable.</param>
    /// <param name="actual">The actual enumerable to test.</param>
    /// <param name="assertSorting">Determines wether the elements has to be contained in the correct order.</param>
    /// <param name="matchExactly">Determines wether the actual enumerable can contain elements that are not specified in the expected elements.</param>
    [Obsolete("Use AreCollectionsEquivalent or AreCollectionsEqual instead.")]
    public void ArrayElements<T>(IEnumerable<T> expected, IEnumerable<T> actual, bool assertSorting, bool matchExactly)
        => ArrayElements(expected, actual, null, null, null, assertSorting, matchExactly);

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
    [Obsolete("Use AreCollectionsEquivalent or AreCollectionsEqual instead.")]
    public void ArrayElements<TExpected, TActual>(IEnumerable<TExpected> expected, IEnumerable<TActual> actual, Func<TExpected, TActual, bool> matchFunction, bool assertSorting, bool matchExactly)
        => ArrayElements(expected, actual, matchFunction, null, null, assertSorting, matchExactly);

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
    [Obsolete("Use AreCollectionsEquivalent or AreCollectionsEqual instead.")]
    public void ArrayElements<TExpected, TActual>(IEnumerable<TExpected> expected, IEnumerable<TActual> actual, Func<TExpected, TActual, bool>? matchFunction, Func<TExpected, string>? expectedDisplaySelector, Func<TActual, string>? actualDisplaySelector, bool assertSorting, bool matchExactly)
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
            ThrowAssertError(string.Join("\n", errors));
    }
}
