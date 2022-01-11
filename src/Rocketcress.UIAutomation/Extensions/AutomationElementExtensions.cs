using System.Text;

namespace Rocketcress.UIAutomation.Extensions
{
    public static class AutomationElementExtensions
    {
        public static bool TryGetCurrentPropertyValue(this AutomationElement element, AutomationProperty property, out object value)
        {
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
}
