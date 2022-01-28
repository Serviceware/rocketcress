using Rocketcress.Core.Models;
using System.Runtime.CompilerServices;

namespace Rocketcress.Core.Base;

/// <summary>
/// Base class for any test related object.
/// </summary>
public class TestObjectBase
{
    private static readonly PropertyStorage _staticPropertyStorage = new();
    private readonly PropertyStorage _instancePropertyStorage = new();

    /// <summary>
    /// Gets the current instance of the <see cref="AssertEx"/> class.
    /// </summary>
    protected static Assert Assert => Assert.Instance;

    /// <summary>
    /// Retrieves the current method name from the Call Stack.
    /// </summary>
    /// <returns>Returns the name of the method that executed this method.</returns>
    protected static string? GetCurrentMethodName() => GetCurrentMethodName(2);

    /// <summary>
    /// Retrieves the current method name from the Call Stack.
    /// </summary>
    /// <param name="skipFrames">The number of frames to skip in the Call Stack.</param>
    /// <returns>Returns the name of the method that executed this method.</returns>
    protected static string? GetCurrentMethodName(int skipFrames)
    {
        return new StackFrame(skipFrames).GetMethod()?.Name;
    }

    /// <summary>
    /// Gets the value of a property in the static property cache.
    /// </summary>
    /// <typeparam name="T">The type of value to retrieve.</typeparam>
    /// <param name="propertyName">The name of the property to get.</param>
    /// <returns>Returns the current value of the specified property.</returns>
    protected static T? GetStaticProperty<T>([CallerMemberName] string propertyName = "")
        => _staticPropertyStorage.GetProperty<T>(propertyName);

    /// <summary>
    /// Gets the value of a property in the static property cache.
    /// </summary>
    /// <typeparam name="T">The type of value to retrieve.</typeparam>
    /// <param name="initializer">A function that is executed if the property has no value yet.</param>
    /// <param name="propertyName">The name of the property to get.</param>
    /// <returns>Returns the current value of the specified property.</returns>
    protected static T? GetStaticProperty<T>(Func<T> initializer, [CallerMemberName] string propertyName = "")
        => _staticPropertyStorage.GetProperty(initializer, propertyName);

    /// <summary>
    /// Sets the value of a property in the static property cache.
    /// </summary>
    /// <typeparam name="T">The type of value to set.</typeparam>
    /// <param name="value">The value to set.</param>
    /// <param name="propertyName">The name of the property to set.</param>
    protected static void SetStaticProperty<T>(T value, [CallerMemberName] string propertyName = "")
        => _staticPropertyStorage.SetProperty(value, propertyName);

    /// <summary>
    /// Resets the value of a property in the static property cache.
    /// </summary>
    /// <param name="propertyName">The name of the property for which the value should be reset.</param>
    protected static void ResetStaticProperty([CallerMemberName] string propertyName = "")
        => _staticPropertyStorage.ResetProperty(propertyName);

    /// <summary>
    /// Determines wether a property has a value in the static property cache.
    /// </summary>
    /// <param name="propertyName">The name of the property to check.</param>
    /// <returns>Returns true if the property has a value; otherwise false.</returns>
    protected static bool HasStaticProperty([CallerMemberName] string propertyName = "")
        => _staticPropertyStorage.HasProperty(propertyName);

    /// <summary>
    /// Gets the value of a property in the property cache.
    /// </summary>
    /// <typeparam name="T">The type of value to retrieve.</typeparam>
    /// <param name="propertyName">The name of the property to get.</param>
    /// <returns>Returns the current value of the specified property.</returns>
    protected virtual T? GetProperty<T>([CallerMemberName] string propertyName = "")
        => _instancePropertyStorage.GetProperty<T>(propertyName);

    /// <summary>
    /// Gets the value of a property in the property cache.
    /// </summary>
    /// <typeparam name="T">The type of value to retrieve.</typeparam>
    /// <param name="initializer">A function that is executed if the property has no value yet.</param>
    /// <param name="propertyName">The name of the property to get.</param>
    /// <returns>Returns the current value of the specified property.</returns>
    protected virtual T? GetProperty<T>(Func<T> initializer, [CallerMemberName] string propertyName = "")
        => _instancePropertyStorage.GetProperty(initializer, propertyName);

    /// <summary>
    /// Sets the value of a property in the property cache.
    /// </summary>
    /// <typeparam name="T">The type of value to set.</typeparam>
    /// <param name="value">The value to set.</param>
    /// <param name="propertyName">The name of the property to set.</param>
    protected virtual void SetProperty<T>(T value, [CallerMemberName] string propertyName = "")
        => _instancePropertyStorage.SetProperty(value, propertyName);

    /// <summary>
    /// Resets the value of a property in the property cache.
    /// </summary>
    /// <param name="propertyName">The name of the property for which the value should be reset.</param>
    protected virtual void ResetProperty([CallerMemberName] string propertyName = "")
        => _instancePropertyStorage.ResetProperty(propertyName);

    /// <summary>
    /// Determines wether a property has a value in the property cache.
    /// </summary>
    /// <param name="propertyName">The name of the property to check.</param>
    /// <returns>Returns true if the property has a value; otherwise false.</returns>
    protected virtual bool HasProperty([CallerMemberName] string propertyName = "")
        => _instancePropertyStorage.HasProperty(propertyName);

    /// <summary>
    /// Clears all property values of the property cache.
    /// </summary>
    protected virtual void ClearProperties()
        => _instancePropertyStorage.Clear();

    /// <summary>
    /// Clears all property values of the static property cache.
    /// </summary>
    protected virtual void ClearStaticProperties()
        => _staticPropertyStorage.Clear();
}
