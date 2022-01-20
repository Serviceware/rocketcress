using Rocketcress.Core.Base;

namespace Rocketcress.UIAutomation;

public abstract class UIAutomationTestBase : UIAutomationTestBase<Settings, UIAutomationTestContext>
{
}

[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic representation")]
public abstract class UIAutomationTestBase<TSettings, TContext> : TestBase<TSettings, TContext>
    where TSettings : Settings
    where TContext : UIAutomationTestContext
{
}
