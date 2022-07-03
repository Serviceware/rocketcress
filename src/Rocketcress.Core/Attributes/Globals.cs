namespace Rocketcress.Core.Attributes;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements should be documented
#if DEBUG
public static class GenerateUIMapPartsAttributeDefaults
#else
internal static class GenerateUIMapPartsAttributeDefaults
#endif
{
    public static readonly bool GenerateDefaultConstructors = true;
    public static readonly IdStyle IdStyle = IdStyle.None;
    public static readonly string? IdFormat = null;
}

#if DEBUG
public static class UIMapControlAttributeDefault
#else
internal static class UIMapControlAttributeDefault
#endif
{
    public static readonly bool Initialize = true;
    public static readonly string? ParentControl = "this";
    public static readonly IdStyle IdStyle = IdStyle.Unset;
    public static readonly string? IdFormat = null;
    public static readonly string? Id = null;
}
#pragma warning restore SA1600 // Elements should be documented
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
    /// The automatic id generation is disabled and an empty location key is used instead.
    /// </summary>
    Disabled,

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
