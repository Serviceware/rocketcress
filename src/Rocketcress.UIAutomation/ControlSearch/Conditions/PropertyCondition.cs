using Rocketcress.Core.Extensions;
using Rocketcress.UIAutomation.Extensions;

namespace Rocketcress.UIAutomation.ControlSearch.Conditions;

/// <summary>
/// Represents a search condition for finding UIAutomation elements that match a specific property.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.ControlSearch.Conditions.SearchConditionBase" />
public class PropertyCondition : SearchConditionBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyCondition"/> class.
    /// </summary>
    /// <param name="property">The property to check.</param>
    /// <param name="value">The value to match.</param>
    public PropertyCondition(AutomationProperty property, object value)
        : this(property, value, ByOptions.None)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyCondition"/> class.
    /// </summary>
    /// <param name="property">The property to check.</param>
    /// <param name="value">The value to match.</param>
    /// <param name="options">The comparison options.</param>
    public PropertyCondition(AutomationProperty property, object value, ByOptions options)
    {
        Property = property;
        Value = value;
        Options = options;
    }

    /// <summary>
    /// Gets or sets the property to check.
    /// </summary>
    public AutomationProperty Property { get; set; }

    /// <summary>
    /// Gets or sets the value to match.
    /// </summary>
    public object Value { get; set; }

    /// <summary>
    /// Gets or sets the comparison options.
    /// </summary>
    public ByOptions Options { get; set; }

    /// <inheritdoc />
    public override bool Check(AutomationElement element, TreeWalker treeWalker)
    {
        Guard.NotNull(element);

        bool result;

        var value = element.GetCurrentPropertyValue(Property);
        if (value is string actualString && Value is string expectedString)
        {
            result = Options.Check(expectedString, actualString);
        }
        else
        {
            result = Equals(value, Value) ^ Options.HasFlag(ByOptions.Unequal);
        }

        return result;
    }

    /// <inheritdoc />
    public override string GetDescription()
    {
        string strValue;
        if (Value is ControlType controlType)
            strValue = controlType.ProgrammaticName?.Split('.').Last();
        else
            strValue = Value.ToString();

        string propertyName = Property.ProgrammaticName?.Split('.').Last().TrimEnd("Property");

        char[] @operator = new char[2];
        @operator[0] = Options.HasFlag(ByOptions.UseContains) ? '~' : '=';
        @operator[1] = Options.HasFlag(ByOptions.IgnoreCase) ? '~' : '=';
        if (@operator[0] == @operator[1])
            @operator = new char[1] { @operator[0] };
        return $"@{propertyName}{new string(@operator)}'{strValue}'";
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is PropertyCondition other &&
               Equals(Property, other.Property) &&
               Equals(Value, other.Value) &&
               Equals(Options, other.Options);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return (Property, Value, Options).GetHashCode();
    }

    /// <inheritdoc />
    protected override SearchConditionBase CloneInternal()
    {
        return new PropertyCondition(Property, Value, Options);
    }
}
