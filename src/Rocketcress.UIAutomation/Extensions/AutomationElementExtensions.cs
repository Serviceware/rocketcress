using System.Text;

namespace Rocketcress.UIAutomation.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="AutomationElement"/> class.
/// </summary>
public static class AutomationElementExtensions
{
    /// <summary>
    /// Tries the get current property value.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="property">The property.</param>
    /// <param name="value">The value of the property.</param>
    /// <returns><c>true</c> if the property value could be retrieved.</returns>
    public static bool TryGetCurrentPropertyValue(this AutomationElement element, AutomationProperty property, out object value)
    {
        Guard.NotNull(element);
        try
        {
            value = element.GetCurrentPropertyValue(property);
            return true;
        }
        catch
        {
            value = null;
            return false;
        }
    }

    /// <summary>
    /// Determines whether this instance is stale.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>
    ///   <c>true</c> if the specified element is stale; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsStale(this AutomationElement element)
    {
        try
        {
            if (element == null)
                return true;
            return element.Current.Name == "{DisconnectedItem}";
        }
        catch (ElementNotAvailableException)
        {
            return true;
        }
    }

    /// <summary>
    /// Gets the search description.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>The search description.</returns>
    public static string GetSearchDescription(this AutomationElement element)
    {
        var result = new StringBuilder();
        if (element == null)
        {
            result.Append("(null)");
        }
        else if (element.IsStale())
        {
            result.Append("(stale)");
        }
        else
        {
            bool isFirst = true;
            void TryAddProperty(string propertyName, Func<object> func)
            {
                try
                {
                    var value = func();
                    if (!isFirst)
                        result.Append(", ");
                    result.Append(propertyName)
                          .Append(": ")
                          .Append(value);
                    isFirst = false;
                }
                catch
                { /* Ignore not supported properties. */
                }
            }

            TryAddProperty("ControlType", () => element.Current.ControlType.ProgrammaticName);
            TryAddProperty("Name", () => element.Current.Name);
            TryAddProperty("ClassName", () => element.Current.ClassName);
            TryAddProperty("NativeWindowHandle", () => "0x" + Convert.ToString(element.Current.NativeWindowHandle, 16));
        }

        return result.ToString();
    }
}
