using System;

#pragma warning disable SA1649 // File name should match first type name
#pragma warning disable SA1402 // File may only contain a single type

namespace Rocketcress.Core.Attributes
{
    /// <summary>
    /// When applied to a class that derives from a control class from either Rocketcress.Selenium or Rocketcress.UIAutomation,
    /// some specified parts of the class will be generated when the NuGet package Rocketcress.SourceGenerators is references.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class GenerateUIMapPartsAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the type from which to take the control properties that should be generated.
        /// </summary>
        public Type ControlsDefinition { get; set; } = GenerateUIMapPartsAttributeDefaults.ControlsDefinition;

        /// <summary>
        /// Gets or sets a value indicating whether to generate the same constructors as the base class.
        /// </summary>
        public bool GenerateDefaultConstructors { get; set; } = GenerateUIMapPartsAttributeDefaults.GenerateDefaultConstructors;
    }

    /// <summary>
    /// When applied to a property of an interface that is used as <see cref="GenerateUIMapPartsAttribute.ControlsDefinition"/>,
    /// the generation for the property is altered by specifing some options.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class UIMapControlOptionsAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether to generate initialization code for this control property.
        /// If set to <c>false</c> the property needs to be initialized manually using the partial methods that are provided.
        /// </summary>
        public bool Initialize { get; set; } = UIMapControlOptionsAttributeDefault.Initialize;

        /// <summary>
        /// Gets or sets the parent control name. By default this is set to <c>this</c>.
        /// </summary>
        public string ParentControl { get; set; } = UIMapControlOptionsAttributeDefault.ParentControl;

        /// <summary>
        /// Gets or sets the accessibility of the generated control property.
        /// </summary>
        public ControlPropertyAccessibility Accessibility { get; set; } = UIMapControlOptionsAttributeDefault.Accessibility;

        /// <summary>
        /// Gets or sets a value indicating whether the generated control should be marked as <c>virtual</c>.
        /// </summary>
        public bool IsVirtual { get; set; } = UIMapControlOptionsAttributeDefault.IsVirtual;
    }
}
