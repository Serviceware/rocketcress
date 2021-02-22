using Rocketcress.UIAutomation.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Common
{
    public static class ControlUtility
    {
        private static List<(By locationKey, Type controlType)> _controlRegistry;
        
        public static IUITestControl GetControl(AutomationElement element)
        {
            if (element == null)
                return null;

            Type targetType = null;

            foreach(var (locationKey, controlType) in GetControlRegistry())
            {
                if (locationKey.ElementSearchPart.Condition.Check(element, TreeWalker.RawViewWalker))
                {
                    targetType = controlType;
                    break;
                }
            }

            if (targetType == null)
                return new UITestControl(element);
            return (IUITestControl)Activator.CreateInstance(targetType, element);
        }

        public static void EnsureControlRegistryIsFilled()
        {
            if (_controlRegistry == null)
            {
                var types = new List<(Type type, AutoDetectControlAttribute attr)>();
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
                                    orderby t.attr.Priority descending
                                    let locationKey = UITestControl.GetBaseLocationKey(t.type)
                                    select (locationKey, t.type)).ToList();
            }
        }

        private static List<(By locationKey, Type controlType)> GetControlRegistry()
        {
            EnsureControlRegistryIsFilled();
            return _controlRegistry;
        }
    }
}
