using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Common
{
    public static class PatternUtility
    {
        private static readonly Dictionary<Type, (AutomationProperty isAvailableProperty, AutomationPattern pattern)> _patternCache = new Dictionary<Type, (AutomationProperty, AutomationPattern)>();

        public static AutomationProperty GetIsPatternAvailableProperty<T>() where T : BasePattern => GetPatternInfo(typeof(T)).isAvailableProperty;
        public static AutomationProperty GetIsPatternAvailableProperty(Type patternType) => GetPatternInfo(patternType).isAvailableProperty;

        public static AutomationPattern GetPattern<T>() where T : BasePattern => GetPatternInfo(typeof(T)).pattern;
        public static AutomationPattern GetPattern(Type patternType) => GetPatternInfo(patternType).pattern;

        private static (AutomationProperty isAvailableProperty, AutomationPattern pattern) GetPatternInfo(Type patternType)
        {
            if (!_patternCache.ContainsKey(patternType))
            {

                var patternName = patternType.Name;
                var isAvailableProperty = typeof(AutomationElement).GetField($"Is{patternName}AvailableProperty", BindingFlags.Public | BindingFlags.Static);
                if (isAvailableProperty == null)
                    throw new NotSupportedException($"No \"Is[...]AvailableProperty\" was found for pattern {patternType.FullName} in the type {typeof(AutomationElement).FullName}.");
                if (!typeof(AutomationProperty).IsAssignableFrom(isAvailableProperty.FieldType))
                    throw new NotSupportedException($"The field type of field \"Is{patternName}AvailableProperty\" has to be derived from {typeof(AutomationProperty).FullName}.");
                var patternProperty = patternType.GetField("Pattern", BindingFlags.Public | BindingFlags.Static);
                if (patternProperty == null)
                    throw new NotSupportedException($"No \"Pattern\" field was found on type {patternType.FullName}.");
                if (!typeof(AutomationPattern).IsAssignableFrom(patternProperty.FieldType))
                    throw new NotSupportedException($"The field type of field \"Pattern\" of type {patternType.FullName} has to be derived from {typeof(AutomationPattern).FullName}.");
                _patternCache.Add(patternType, ((AutomationProperty)isAvailableProperty.GetValue(null), (AutomationPattern)patternProperty.GetValue(null)));
            }
            return _patternCache[patternType];
        }
    }
}
