﻿using System;

#nullable disable

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
        public static readonly IdStyle IdStyle = IdStyle.None;
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
        public static readonly bool IsHidden = false;
        public static readonly IdStyle IdStyle = IdStyle.Unset;
    }

#if DEBUG
    public static class UIMapControlAttributeDefault
#else
    internal static class UIMapControlAttributeDefault
#endif
    {
        public static readonly bool Initialize = true;
        public static readonly string ParentControl = "this";
        public static readonly IdStyle IdStyle = IdStyle.Unset;
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

    /// <summary>
    /// Specifies the naming style for ids.
    /// </summary>
    public enum IdStyle
    {
        /// <summary>
        /// The casing of the property is not set and the default is used.
        /// </summary>
        Unset,

        /// <summary>
        /// The casing of the property is not changed.
        /// </summary>
        None,

        /// <summary>
        /// The pascal case (ThisIsAnExample).
        /// </summary>
        PascalCase,

        /// <summary>
        /// The camel case (thisIsAndExample).
        /// </summary>
        CamelCase,

        /// <summary>
        /// The kebab case (this-is-an-example).
        /// </summary>
        KebabCase,

        /// <summary>
        /// The lower case (thisisanexample).
        /// </summary>
        LowerCase,

        /// <summary>
        /// The upper case (THISISANEXAMPLE).
        /// </summary>
        UpperCase,
    }
}
