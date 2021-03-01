using Rocketcress.Core.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
#if !SLIM
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Rocketcress.UIAutomation
{
    /// <summary>
    /// Represents a test context for a UIAutomation Test.
    /// </summary>
    public class UIAutomationTestContext : WindowsTestContextBase
    {
        /// <summary>
        /// Gets the current instance of the <see cref="UIAutomationTestContext"/>.
        /// </summary>
        public static new UIAutomationTestContext CurrentContext { get; private set; }

        /// <summary>
        /// Gets or sets the current test settings.
        /// </summary>
        public new Settings Settings
        {
            get => (Settings)base.Settings;
            set => base.Settings = value;
        }

        /// <summary>
        /// Gets or sets the <see cref="Application"/> that is currently under test and is actively tested currently.
        /// </summary>
        public Application ActiveApplication { get; set; }

        /// <summary>
        /// Gets or sets all <see cref="Application"/>s that are currently under test.
        /// </summary>
        public List<Application> Applications { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIAutomationTestContext"/> class.
        /// </summary>
        protected UIAutomationTestContext()
        {
        }

        /// <inheritdoc />
        protected override void OnContextCreated(TestContextBase lastContext)
        {
            base.OnContextCreated(lastContext);
            CurrentContext = this;
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            foreach (var app in Applications)
            {
                if (!app.Process.HasExited)
                    app.Process.Kill();
            }

            CurrentContext = null;
            base.Dispose(disposing);
        }

#pragma warning disable CS1572 // XML comment has a param tag, but there is no parameter by that name
#pragma warning disable SA1612 // Element parameter documentation should match element parameters
        /// <summary>
        /// Creates a new <see cref="UIAutomationTestContext"/> as uses it as the current test context. Please make sure to dispose any preexisting <see cref="TestContextBase"/> instances beforehand.
        /// </summary>
        /// <typeparam name="T">The type of the context.</typeparam>
        /// <param name="activationFunc">A function that creates an instance of the wanted test context class.</param>
        /// <param name="settings">The settings to use during the test.</param>
        /// <param name="testContext">The MSTest Test Context.</param>
        /// <param name="initAction">An action that is executed before the new context is set as current context. Add additional information to the object here if needed.</param>
        /// <returns>The created context.</returns>
        protected static T CreateContext<T>(
            Func<T> activationFunc,
            Settings settings,
#if !SLIM
            TestContext testContext,
#endif
            Action<T> initAction)
            where T : UIAutomationTestContext
        {
            return TestContextBase.CreateContext<T>(
                activationFunc,
                settings,
#if !SLIM
                testContext,
#endif
                Initialize);

            void Initialize(T ctx)
            {
                ctx.Applications = new List<Application>();
                initAction?.Invoke(ctx);
            }
        }

        /// <summary>
        /// Creates a new <see cref="UIAutomationTestContext"/> as uses it as the current test context. Please make sure to dispose any preexisting <see cref="TestContextBase"/> instances beforehand.
        /// </summary>
        /// <param name="settings">The settings to use during the test.</param>
        /// <param name="testContext">The MSTest Test Context.</param>
        /// <returns>The created context.</returns>
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = "SLIM check")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1111:Closing parenthesis should be on line of last parameter", Justification = "SLIM check")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1115:Parameter should follow comma", Justification = "SLIM check")]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1001:Commas should be spaced correctly", Justification = "SLIM check")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1113:Comma should be on the same line as previous parameter", Justification = "SLIM check")]
        public static UIAutomationTestContext CreateContext(
            Settings settings
#if !SLIM
          , TestContext testContext
#endif
            )
        {
            return CreateContext(
                () => new UIAutomationTestContext(),
                settings,
#if !SLIM
                testContext,
#endif
                null);
        }
#pragma warning restore SA1612 // Element parameter documentation should match element parameters
#pragma warning restore CS1572 // XML comment has a param tag, but there is no parameter by that name
    }
}
