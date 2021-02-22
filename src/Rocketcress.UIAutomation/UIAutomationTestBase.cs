using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Interaction;
using Rocketcress.Core.Attributes;
using Rocketcress.Core.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Rocketcress.Core;

namespace Rocketcress.UIAutomation
{
    [TestClass]
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
        protected override UIAutomationTestContext OnCreateContext()
            => UIAutomationTestContext.CreateContext(Settings, TestContext);
    }

    [TestClass]
    [AddKeysClass("SettingKeys")]
    public abstract class UIAutomationTestBase<TSettings, TContext> : TestBase<TSettings, TContext> 
        where TSettings : Settings 
        where TContext : UIAutomationTestContext
    {
        public virtual Application CurrentApp { get; protected set; }
        
        [TestInitialize]
        public override void InitializeTest() 
        {
            base.InitializeTest();

            UIAutomationTestBase.Initialize();
            CurrentApp = CurrentContext.ActiveApplication;
        }

        [TestCleanup]
        public override void CleanupTest()
        {
            base.CleanupTest();

            if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
            {
                CurrentContext.TakeAndAppendScreenshot();
            }
            CurrentContext.Dispose();
        }
    }
}
