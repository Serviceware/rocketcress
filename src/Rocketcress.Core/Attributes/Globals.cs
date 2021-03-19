using System;

#pragma warning disable SA1649 // File name should match first type name
#pragma warning disable SA1402 // File may only contain a single type

namespace Rocketcress.Core.Attributes
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#if DEBUG
    public static class GenerateUIMapPartsAttributeDefaults
#else
    internal static class GenerateUIMapPartsAttributeDefaults
#endif
    {
        public static readonly Type ControlsDefinition = null;
        public static readonly bool GenerateDefaultConstructors = true;
    }

#if DEBUG
    public static class UIMapControlOptionsAttributeDefault
#else
    internal static class UIMapControlOptionsAttributeDefault
#endif
    {
        public static readonly bool Initialize = true;
        public static readonly string ParentControl = "this";
        public static readonly ControlPropertyAccessibility Accessibility = ControlPropertyAccessibility.Public;
        public static readonly bool IsVirtual = false;
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

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
