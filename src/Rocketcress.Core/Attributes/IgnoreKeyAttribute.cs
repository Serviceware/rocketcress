namespace Rocketcress.Core.Attributes
{
    /// <summary>
    /// Used to ignore a specific key in the key check.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class IgnoreKeyAttribute : Attribute
    {
    }
}
