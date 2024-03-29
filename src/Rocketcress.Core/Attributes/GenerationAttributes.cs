﻿namespace Rocketcress.Core.Attributes;

/// <summary>
/// When applied to a class that derives from a control class from either Rocketcress.Selenium or Rocketcress.UIAutomation,
/// some specified parts of the class will be generated when the NuGet package Rocketcress.SourceGenerators is references.
/// </summary>
/// <seealso cref="System.Attribute" />
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class GenerateUIMapPartsAttribute : Attribute
{
    /// <summary>
    /// Gets or sets a value indicating whether to generate the same constructors as the base class.
    /// </summary>
    public bool GenerateDefaultConstructors { get; set; } = GenerateUIMapPartsAttributeDefaults.GenerateDefaultConstructors;

    /// <summary>
    /// Gets or sets the naming style that should be used to generate default location keys for properties.
    /// </summary>
    public IdStyle IdStyle { get; set; } = GenerateUIMapPartsAttributeDefaults.IdStyle;

    /// <summary>
    /// Gets or sets the format that should be used to generate default location keys for properties.
    /// </summary>
    public string? IdFormat { get; set; } = GenerateUIMapPartsAttributeDefaults.IdFormat;
}

/// <summary>
/// When applied to a property of a class that has the <see cref="GenerateUIMapPartsAttribute"/> code attribute,
/// the generation for the initialization of the property is enabled and altered by specifing some options.
/// </summary>
/// <seealso cref="System.Attribute" />
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class UIMapControlAttribute : Attribute
{
    /// <summary>
    /// Gets or sets a value indicating whether to generate initialization code for this control property.
    /// If set to <c>false</c> the property needs to be initialized manually using the partial methods that are provided.
    /// </summary>
    public bool Initialize { get; set; } = UIMapControlAttributeDefault.Initialize;

    /// <summary>
    /// Gets or sets the parent control name. By default this is set to <c>this</c>.
    /// </summary>
    public string? ParentControl { get; set; } = UIMapControlAttributeDefault.ParentControl;

    /// <summary>
    /// Gets or sets the naming style that should be used to generate default location keys for this property.
    /// </summary>
    public IdStyle IdStyle { get; set; } = UIMapControlAttributeDefault.IdStyle;

    /// <summary>
    /// Gets or sets the format that should be used to generate default location keys for properties.
    /// </summary>
    public string? IdFormat { get; set; } = UIMapControlAttributeDefault.IdFormat;

    /// <summary>
    /// Gets or sets the id that should be used to generate default location keys for properties.
    /// </summary>
    public string? Id { get; set; } = UIMapControlAttributeDefault.Id;
}
