using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1649 // File name should match first type name
#pragma warning disable SA1402 // File may only contain a single type

namespace Rocketcress.Core.Attributes
{
    internal static class GenerateUIMapPartsAttributeDefaults
    {
        public static readonly Type ControlsDefinition = null;
        public static readonly bool GenerateDefaultConstructors = true;
    }

    internal static class UIMapControlOptionsAttributeDefault
    {
        public static readonly bool Initialize = true;
        public static readonly string ParentControl = "this";
        public static readonly ControlPropertyAccessibility Accessibility = ControlPropertyAccessibility.Public;
        public static readonly bool IsVirtual = false;
    }

    /// <summary>
    /// Specifies the accessibilty of a control property.
    /// </summary>
    public enum ControlPropertyAccessibility
    {
        /// <summary>
        /// Uses the <c>public</c> accessibility.
        /// </summary>
        Public,

        /// <summary>
        /// Uses the <c>protected</c> accessibility.
        /// </summary>
        Protected,

        /// <summary>
        /// Uses the <c>private</c> accessibility.
        /// </summary>
        Private,
    }
}
