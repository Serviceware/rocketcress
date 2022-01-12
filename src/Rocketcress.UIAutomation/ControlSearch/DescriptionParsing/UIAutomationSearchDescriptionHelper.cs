using System.Reflection;

namespace Rocketcress.UIAutomation.ControlSearch.DescriptionParsing;

internal static class UIAutomationSearchDescriptionHelper
{
    private static IDictionary<string, AutomationProperty> _properties;
    private static IDictionary<string, ControlType> _controlTypes;

    static UIAutomationSearchDescriptionHelper()
    {
        _properties = typeof(AutomationElement).GetFields(BindingFlags.Static | BindingFlags.Public)
            .Where(x => x.Name.EndsWith("Property"))
            .ToDictionary(x => x.Name.Substring(0, x.Name.Length - 8).ToLower(), x => (AutomationProperty)x.GetValue(null));

        _properties.Add("id", AutomationElement.AutomationIdProperty);

        _controlTypes = typeof(ControlType).GetFields(BindingFlags.Static | BindingFlags.Public)
            .ToDictionary(x => x.Name.ToLower(), x => (ControlType)x.GetValue(null));

        _controlTypes.Add("label", ControlType.Text);
        _controlTypes.Add("textbox", ControlType.Edit);
        _controlTypes.Add("tablist", ControlType.Tab);
    }

    public static AutomationProperty GetPropertyByName(string propertyName)
    {
        if (_properties.TryGetValue(propertyName.ToLower(), out var tempProp1))
            return tempProp1;
        else if (_properties.TryGetValue(propertyName.Replace("-", string.Empty).ToLower(), out var tempProp2))
            return tempProp2;
        throw new InvalidOperationException($"A property with name '{propertyName}' was not found.");
    }

    public static ControlType GetControlTypeByName(string controlTypeName)
    {
        if (_controlTypes.TryGetValue(controlTypeName.ToLower(), out var tempCt1))
            return tempCt1;
        else if (_controlTypes.TryGetValue(controlTypeName.Replace("-", string.Empty).ToLower(), out var tempCt2))
            return tempCt2;
        throw new InvalidOperationException($"A control type with the name '{controlTypeName}' was not found.");
    }
}
