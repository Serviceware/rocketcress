using Rocketcress.Core;
using Rocketcress.Core.Attributes;
using Rocketcress.Core.Base;
using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Interaction;
using System;
using System.Diagnostics.CodeAnalysis;
#if !SLIM
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Rocketcress.UIAutomation
{
#if !SLIM
    [TestClass]
#endif
    [AddKeysClass("SettingKeys")]
    public abstract class UIAutomationTestBase : UIAutomationTestBase<Settings, UIAutomationTestContext>
    {
        public static void Initialize()
        {
            Mouse.IsWaitForControlReadyEnabled = true;
            ControlUtility.EnsureControlRegistryIsFilled();

            // Set process DPI Aware, so Clicks are working on High DPI screens
            if (Environment.OSVersion.Version.Major >= 6)
                WindowsApiHelper.SetProcessDPIAware();
        }

        /// <inheritdoc />
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = "SLIM check")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1111:Closing parenthesis should be on line of last parameter", Justification = "SLIM check")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1115:Parameter should follow comma", Justification = "SLIM check")]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1001:Commas should be spaced correctly", Justification = "SLIM check")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1113:Comma should be on the same line as previous parameter", Justification = "SLIM check")]
        protected override UIAutomationTestContext OnCreateContext()
        {
            return UIAutomationTestContext.CreateContext(
                Settings
#if !SLIM
              , TestContext
#endif
                );
        }
    }

#if !SLIM
    [TestClass]
#endif
    [AddKeysClass("SettingKeys")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic representation")]
    public abstract class UIAutomationTestBase<TSettings, TContext> : TestBase<TSettings, TContext>
        where TSettings : Settings
        where TContext : UIAutomationTestContext
    {
        public virtual Application CurrentApp { get; protected set; }

#if !SLIM
        [TestInitialize]
#endif
        public override void InitializeTest()
        {
            base.InitializeTest();

            UIAutomationTestBase.Initialize();
            CurrentApp = CurrentContext.ActiveApplication;
        }

#if !SLIM
        [TestCleanup]
#endif
        public override void CleanupTest()
        {
            base.CleanupTest();

#if !SLIM
            if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
            {
                CurrentContext.TakeAndAppendScreenshot();
            }
#endif

            CurrentContext.Dispose();
        }
    }
}
