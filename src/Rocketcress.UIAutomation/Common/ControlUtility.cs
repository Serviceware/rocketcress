using Rocketcress.UIAutomation.Controls;
using System.Reflection;

namespace Rocketcress.UIAutomation.Common;

/// <summary>
/// Utility for working with UIAutomation controls.
/// </summary>
public static class ControlUtility
{
    private static List<(By LocationKey, Type ControlType)> _controlRegistry;

    /// <summary>
    /// Finds the correct control class for the given <see cref="AutomationElement"/> and creates an instance of it.
    /// </summary>
    /// <param name="application">The application the element is attached to.</param>
    /// <param name="element">The element to wrap into a control instance.</param>
    /// <returns>An instance of the correct control class matching the <paramref name="element"/>.</returns>
    public static IUITestControl GetControl(Application application, AutomationElement element)
    {
        if (element == null)
            return null;

        Type targetType = null;

        foreach (var (locationKey, controlType) in GetControlRegistry())
        {
            if (locationKey.ElementSearchPart.Condition.Check(element, TreeWalker.RawViewWalker))
            {
                targetType = controlType;
                break;
            }
        }

        if (targetType == null)
            return new UITestControl(application, element);
        return (IUITestControl)Activator.CreateInstance(targetType, element);
    }

    /// <summary>
    /// Ensures the control registry is filled by scanning all assemblies in the current <see cref="AppDomain"/> for classes with <see cref="AutoDetectControlAttribute"/>.
    /// </summary>
    public static void EnsureControlRegistryIsFilled()
    {
        if (_controlRegistry == null)
        {
            var types = new List<(Type Type, AutoDetectControlAttribute Attr)>();
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] asmTypes;
                try
                {
                    asmTypes = asm.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    asmTypes = ex.Types;
                }

                types.AddRange(from t in asmTypes
                               let attr = t?.GetCustomAttribute<AutoDetectControlAttribute>()
                               where t != null && attr != null
                               select (t, attr));
            }

            _controlRegistry = (from t in types
                                orderby t.Attr.Priority descending
                                let locationKey = UITestControl.GetBaseLocationKey(t.Type)
                                select (locationKey, t.Type)).ToList();
        }
    }

    private static List<(By LocationKey, Type ControlType)> GetControlRegistry()
    {
        EnsureControlRegistryIsFilled();
        return _controlRegistry;
    }
}
