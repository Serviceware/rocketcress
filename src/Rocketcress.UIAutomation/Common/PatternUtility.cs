using System.Reflection;

namespace Rocketcress.UIAutomation.Common;

/// <summary>
/// Utility to handle <see cref="AutomationPattern"/>.
/// </summary>
public static class PatternUtility
{
    private static readonly Dictionary<Type, (AutomationProperty IsAvailableProperty, AutomationPattern Pattern)> _patternCache = new();

    /// <summary>
    /// Retrieves the property that determines wether an element has the pattern provided by the given type.
    /// </summary>
    /// <typeparam name="T">The type of pattern.</typeparam>
    /// <returns>The <see cref="AutomationProperty"/> that determines wether an <see cref="AutomationElement"/> has the pattern <typeparamref name="T"/> available.</returns>
    public static AutomationProperty GetIsPatternAvailableProperty<T>()
        where T : BasePattern
        => GetPatternInfo(typeof(T)).IsAvailableProperty;

    /// <summary>
    /// Retrieves the property that determines wether an element has the pattern provided by the given type.
    /// </summary>
    /// <param name="patternType">The type of pattern.</param>
    /// <returns>The <see cref="AutomationProperty"/> that determines wether an <see cref="AutomationElement"/> has the pattern <paramref name="patternType"/> available.</returns>
    public static AutomationProperty GetIsPatternAvailableProperty(Type patternType) => GetPatternInfo(patternType).IsAvailableProperty;

    /// <summary>
    /// Retrieves the instance of the given type.
    /// </summary>
    /// <typeparam name="T">The type of pattern.</typeparam>
    /// <returns>The <see cref="AutomationPattern"/> instance for pattern type <typeparamref name="T"/>.</returns>
    public static AutomationPattern GetPattern<T>()
        where T : BasePattern
        => GetPatternInfo(typeof(T)).Pattern;

    /// <summary>
    /// Retrieves the instance of the given type.
    /// </summary>
    /// <param name="patternType">The type of pattern.</param>
    /// <returns>The <see cref="AutomationPattern"/> instance for pattern type <paramref name="patternType"/>.</returns>
    public static AutomationPattern GetPattern(Type patternType) => GetPatternInfo(patternType).Pattern;

    private static (AutomationProperty IsAvailableProperty, AutomationPattern Pattern) GetPatternInfo(Type patternType)
    {
        Guard.NotNull(patternType);

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
