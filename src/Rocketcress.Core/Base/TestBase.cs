﻿using System.Reflection;
using System.Runtime.ExceptionServices;
using Rocketcress.Core.Attributes;

namespace Rocketcress.Core.Base;

/// <summary>
/// Base class for test classes.
/// </summary>
/// <typeparam name="TSettings">The type of the settings to use.</typeparam>
/// <typeparam name="TContext">The type of the test context to use.</typeparam>
#if !SLIM
[DeploymentItem("TestSettings/", "TestSettings/")]
[DeploymentItem("TestSettings\\", "TestSettings\\")]
#endif
[AddKeysClass("SettingKeys")]
public abstract class TestBase<TSettings, TContext> : TestObjectBase
    where TSettings : SettingsBase
    where TContext : TestContextBase
{
#if !SLIM
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    /// <summary>
    /// Gets or sets the current MSTest <see cref="TestContext"/>.
    /// </summary>
    public TestContext TestContext { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#endif

    /// <summary>
    /// Gets the last exception that occurred in the current AppDomain.
    /// </summary>
    protected Exception? LastException { get; private set; }

    /// <summary>
    /// Initializes a Test.
    /// </summary>
#if !SLIM
    [TestInitialize]
#endif
    public void InitializeTest()
    {
#if !SLIM
        Logger.LogInfo($"Test '{TestContext.TestName}' initializing...");
        Logger.LogDebug($"Deployment directory: {TestContext.DeploymentDirectory}");
#endif

        TestHelper.IsDebugConfiguration = GetIsDebugConfiguration();
        Logger.LogDebug($"IsDebugConfiguration (could be overridden by derives classes): " + TestHelper.IsDebugConfiguration);

#if !SLIM
        Logger.LogDebug("TestContext.Properties:");
        foreach (var p in TestContext.Properties.Keys)
        {
            Logger.LogDebug($"    {p} = {TestContext.Properties[p]}");
        }
#endif

        AppDomain.CurrentDomain.FirstChanceException += AppDomain_FirstChanceException;
        OnInitializeTest();
    }

    /// <summary>
    /// Cleans up the current Test.
    /// </summary>
#if !SLIM
    [TestCleanup]
#endif
    public void CleanupTest()
    {
        try
        {
            OnCleanupTest();
        }
        finally
        {
#if !SLIM
            if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed && LastException != null)
                Logger.LogError("Exception while test run: {0}", LastException);

            Logger.LogInfo($"Test '{TestContext.TestName}' cleaning up...");
#endif

            AppDomain.CurrentDomain.FirstChanceException -= AppDomain_FirstChanceException;
        }
    }

    /// <summary>
    /// This method is called when a test is initializing.
    /// </summary>
    protected virtual void OnInitializeTest()
    {
    }

    /// <summary>
    /// This method is called when a test is cleaning up.
    /// </summary>
    protected virtual void OnCleanupTest()
    {
    }

    /// <summary>
    /// Creates a new Rocketcress Test Context.
    /// </summary>
    /// <returns>The created context.</returns>
    protected virtual TContext CreateContext()
    {
#if !SLIM
        return CreateContext(
            new[] { typeof(TestContext), typeof(TSettings) },
            new object?[] { TestContext, LoadSettings() });
#else
        return CreateContext(
            new[] { typeof(TSettings) },
            new object?[] { LoadSettings() });
#endif
    }

    /// <summary>
    /// Creates a new Rocketcress Test Context.
    /// </summary>
    /// <param name="constructorParameterTypes">The parameter types needed for the constructor that should be called.</param>
    /// <param name="constructorParameterValues">The parameter values to use when constructing the context.</param>
    /// <returns>The created context.</returns>
    protected TContext CreateContext(Type[] constructorParameterTypes, object?[] constructorParameterValues)
    {
        var constructor = typeof(TContext).GetConstructor(constructorParameterTypes);
        if (constructor is null)
        {
            throw new MissingMethodException($"A public constructor with the following parameter could not be found on type \"{typeof(TContext).FullName}\": " +
                string.Join(", ", constructorParameterTypes.Select(x => x.FullName)));
        }

        var context = constructor.Invoke(constructorParameterValues) ?? throw new NullReferenceException($"An instance of class {typeof(TContext).FullName} could not be created.");
        return (TContext)context;
    }

    /// <summary>
    /// Creates a new Rocketcress Test Context and initializes it.
    /// </summary>
    /// <returns>The created and initialized context.</returns>
    protected TContext CreateAndInitializeContext()
    {
        var context = CreateContext();
        context.Initialize();
        return context;
    }

    /// <summary>
    /// Loads the settings for this test class.
    /// </summary>
    /// <returns>The loaded settings.</returns>
    protected TSettings LoadSettings()
    {
        var assembly = GetType().Assembly;
        var specificSettingsFile = SettingsBase.GetSettingFile(assembly, null, false);
        var defaultSettingsFile = SettingsBase.GetSettingFile(assembly, null, true);
        return SettingsBase.GetFromFiles<TSettings>(specificSettingsFile, defaultSettingsFile);
    }

#region Public Functions

#endregion

    /// <summary>
    /// Dtermines wether the test is run in debug configuration.
    /// </summary>
    /// <returns>A value indicating wether the test is run in debug configuration.</returns>
    protected virtual bool GetIsDebugConfiguration()
    {
        var debuggableAttribute = GetType().Assembly.GetCustomAttribute<DebuggableAttribute>();
        return debuggableAttribute?.IsJITOptimizerDisabled == true;
    }

    private void AppDomain_FirstChanceException(object? sender, FirstChanceExceptionEventArgs e)
    {
        LastException = e.Exception;
    }
}
