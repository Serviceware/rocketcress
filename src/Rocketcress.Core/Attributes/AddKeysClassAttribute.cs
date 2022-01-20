using System.Reflection;

namespace Rocketcress.Core.Attributes;

/// <summary>
/// Used to register a different settings keys class.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AddKeysClassAttribute : Attribute
{
    private readonly Type? _keysClassType;
    private readonly string _keysClassName;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddKeysClassAttribute"/> class.
    /// </summary>
    /// <param name="keysClassType">The type of the key class.</param>
    public AddKeysClassAttribute(Type keysClassType)
    {
        _keysClassType = keysClassType;
        _keysClassName = keysClassType.Name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AddKeysClassAttribute"/> class.
    /// </summary>
    /// <param name="keysClassName">The class name of the key class.</param>
    public AddKeysClassAttribute(string keysClassName)
    {
        _keysClassName = keysClassName;
        _keysClassType = null;
    }

    /// <summary>
    /// Searches for a class with the given key class name in a specified namespace.
    /// </summary>
    /// <param name="testNamespace">The namespace to search in.</param>
    /// <returns>Returns the found type. If no type was found null is returned.</returns>
    public Type? GetKeysClassType(string? testNamespace)
    {
        var result = _keysClassType;
        if (result == null && !string.IsNullOrEmpty(testNamespace))
        {
            var nsSplit = testNamespace.Split('.');
            for (int i = nsSplit.Length; i > 0 && result == null; i--)
            {
                result = AppDomain.CurrentDomain.GetAssemblies()
                    .Select(x => x.GetType(string.Join(".", nsSplit.Take(i)) + "." + _keysClassName))
                    .FirstOrDefault(x => x != null);
            }
        }

        return result;
    }

    /// <summary>
    /// Retrieves all keys (including classes registered by a <see cref="AddKeysClassAttribute"/>) of a specified type.
    /// </summary>
    /// <param name="testClass">The type to get the keys from.</param>
    /// <returns>Returns a list of keys defined in the specified type.</returns>
    public static string[] GetKeys(Type testClass) => GetKeys(testClass, null);

    /// <summary>
    /// Retrieves all keys (including classes registered by a <see cref="AddKeysClassAttribute"/>) of a specified type.
    /// </summary>
    /// <param name="testClass">The type to get the keys from.</param>
    /// <param name="testNamespace">The namespace which should be used for <see cref="AddKeysClassAttribute"/> instance which do not provide direct types.</param>
    /// <returns>Returns a list of keys defined in the specified type.</returns>
    public static string[] GetKeys(Type testClass, string? testNamespace)
    {
        var @namespace = string.IsNullOrEmpty(testNamespace) ? testClass.Namespace : testNamespace;
        var attributes = testClass.GetCustomAttributes<AddKeysClassAttribute>();
        return (from x in attributes
                let type = x.GetKeysClassType(@namespace)
                where type != null
                from key in (from f in type.GetFields(BindingFlags.Public | BindingFlags.Static)
                                where f.FieldType == typeof(string) && f.GetCustomAttribute<IgnoreKeyAttribute>() == null
                                select (string?)f.GetValue(null)).Concat(GetKeys(type, @namespace))
                select key).ToArray();
    }
}
