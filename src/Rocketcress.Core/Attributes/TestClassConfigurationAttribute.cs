using Rocketcress.Core.Base;

namespace Rocketcress.Core.Attributes;

/// <summary>
/// When applied to a class that derives from <see cref="TestBase{TSettings, TContext}"/> the behavior
/// of certain aspects of the test class is configured.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class TestClassConfigurationAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestClassConfigurationAttribute"/> class.
    /// </summary>
    public TestClassConfigurationAttribute()
    {
        ContextCreationMode = ContextCreationMode.CreateAndInitialize;
        DisposeContextOnCleanup = true;
    }

    /// <summary>
    /// Gets the default values of the <see cref="TestClassConfigurationAttribute"/>.
    /// </summary>
    public static TestClassConfigurationAttribute Default { get; } = new TestClassConfigurationAttribute();

    /// <summary>
    /// Gets or sets a mode that indicates how the local <see cref="TestBase{TSettings, TContext}.Context"/> property is initialized.
    /// </summary>
    public ContextCreationMode ContextCreationMode { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the context of the test class should be disposed when the test cleanup is called.
    /// If set to <c>false</c> the context needs to be manually disposed in the test(s).
    /// </summary>
    public bool DisposeContextOnCleanup { get; set; }
}
