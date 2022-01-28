using Rocketcress.Core.Base;

namespace Rocketcress.UIAutomation;

/// <summary>
/// Test base class for UIAutomation tests.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.UIAutomationTestBase{TSettings, TContext}" />
public abstract class UIAutomationTestBase : UIAutomationTestBase<Settings, UIAutomationTestContext>
{
}

/// <summary>
/// Test base class for UIAutomation tests.
/// </summary>
/// <typeparam name="TSettings">The type of the settings.</typeparam>
/// <typeparam name="TContext">The type of the context.</typeparam>
/// <seealso cref="Rocketcress.UIAutomation.UIAutomationTestBase{TSettings, TContext}" />
public abstract class UIAutomationTestBase<TSettings, TContext> : TestBase<TSettings, TContext>
    where TSettings : Settings
    where TContext : UIAutomationTestContext
{
}
