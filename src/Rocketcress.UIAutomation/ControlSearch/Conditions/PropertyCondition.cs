using Rocketcress.Core.Extensions;
using Rocketcress.UIAutomation.Extensions;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.ControlSearch.Conditions
{
    public class PropertyCondition : SearchConditionBase
    {
        public AutomationProperty Property { get; set; }
        public object Value { get; set; }
        public ByOptions Options { get; set; }

        public PropertyCondition(AutomationProperty property, object value)
            : this(property, value, ByOptions.None)
        {
        }

        public PropertyCondition(AutomationProperty property, object value, ByOptions options)
        {
            Property = property;
            Value = value;
            Options = options;
        }

        public override bool Check(AutomationElement element, TreeWalker treeWalker)
        {
            bool result;

            var value = element.GetCurrentPropertyValue(Property);
            if (value is string sActual && Value is string sExpected)
            {
                result = Options.Check(sExpected, sActual);
            }
            else
            {
                result = Equals(value, Value) ^ Options.HasFlag(ByOptions.Unequal);
            }

            return result;
        }

        protected override SearchConditionBase CloneInternal()
        {
            return new PropertyCondition(Property, Value, Options);
        }

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

        public override bool Equals(object obj)
        {
            return obj is PropertyCondition other &&
                   Equals(Property, other.Property) &&
                   Equals(Value, other.Value) &&
                   Equals(Options, other.Options);
        }

        public override int GetHashCode()
        {
            return (Property, Value, Options).GetHashCode();
        }
    }
}
