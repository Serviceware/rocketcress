using OpenQA.Selenium;
using Rocketcress.Core;
using Rocketcress.Core.Attributes;
using Rocketcress.Core.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
#if !SLIM
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Rocketcress.Selenium
{
    /// <summary>
    /// Base class for a test class containing Selenium tests.
    /// </summary>
#if !SLIM
    [TestClass]
    [DeploymentItem("geckodriver.exe")]
    [DeploymentItem("IEDriverServer.exe")]
    [DeploymentItem("webdriver_prefs.json")]
#endif
    [AddKeysClass("SettingKeys")]
    public abstract class SeleniumTestBase : SeleniumTestBase<Settings, SeleniumTestContext>
    {
        /// <summary>
        /// Initializes a Selenium test.
        /// </summary>
        /// <typeparam name="TSettings">The type of the settings.</typeparam>
        /// <typeparam name="TContext">The type of the test context.</typeparam>
        /// <param name="settings">The settings to use.</param>
        /// <param name="context">The MSTest test context of the test.</param>
        /// <param name="driverConfiguration">The driver configuration to use.</param>
        public static void Initialize<TSettings, TContext>(TSettings settings, TContext context, IDriverConfiguration driverConfiguration = null)
            where TSettings : Settings
            where TContext : SeleniumTestContext
        {
            Logger.LogDebug("Initializing test with browser {0}", SeleniumTestContext.CurrentBrowser);
            Logger.LogDebug("Running selenium test with selenium version {0}", typeof(IWebElement)?.Assembly?.GetName()?.Version?.ToString() ?? "(null)");

#if !SLIM
            Logger.LogDebug("TestContext.Properties:");
            foreach (var p in context.TestContext.Properties.Keys)
            {
                Logger.LogDebug($"    {p} = {context.TestContext.Properties[p]}");
            }
#endif

            if (SeleniumTestContext.CurrentBrowser != Browser.Unknown)
            {
                settings.CurrentBrowser = SeleniumTestContext.CurrentBrowser;
            }
            else
            {
                string sBrowser = null;
#if !SLIM
                sBrowser = Convert.ToString(context.TestContext.Properties["TestConfiguration"])?.ToLower();
                if (string.IsNullOrEmpty(sBrowser))
                    sBrowser = Convert.ToString(context.TestContext.Properties["__Tfs_TestConfigurationName__"])?.ToLower();
#endif

                Logger.LogDebug("TestConfiguration = '{0}'", sBrowser ?? "(null)");
                if (sBrowser != null)
                {
                    if (sBrowser.Contains("chrome"))
                        settings.CurrentBrowser = Browser.Chrome;
                    else if (sBrowser.Contains("firefox"))
                        settings.CurrentBrowser = Browser.Firefox;
                    else if (sBrowser.Contains("ie") || sBrowser.Contains("internet explorer") || sBrowser.Contains("internetexplorer"))
                        settings.CurrentBrowser = Browser.InternetExplorer;
                    else if (sBrowser.Contains("edge"))
                        settings.CurrentBrowser = Browser.Edge;
                }

                SeleniumTestContext.CurrentBrowser = settings.CurrentBrowser;
            }

            if (SeleniumTestContext.CurrentBrowserLanguage != null)
                settings.CurrentBrowserLanguage = SeleniumTestContext.CurrentBrowserLanguage;
            else
                SeleniumTestContext.CurrentBrowserLanguage = settings.CurrentBrowserLanguage;

#if !SLIM
            Logger.LogDebug("Running test \"{0}\" with browser \"{1}\" (lang: {2})...", context.TestContext.TestName, settings.CurrentBrowser, settings.CurrentBrowserLanguage.IetfLanguageTag);
#endif

            context.CreateAndSwitchToNewDriver(settings.CurrentBrowser, null, driverConfiguration);
            context.Driver.Manage().Timeouts().PageLoad = settings.Timeout;
            context.Driver.Manage().Timeouts().AsynchronousJavaScript = settings.Timeout;

            if (settings.CurrentBrowser == Browser.InternetExplorer)
                DesktopUtility.SetCursorPosition(0, 0);
        }

        /// <summary>
        /// Cleans up a Selenium test.
        /// </summary>
        /// <typeparam name="T">The type of the test context.</typeparam>
        /// <param name="context">The MSTest test context of the test.</param>
        public static void Cleanup<T>(T context)
            where T : SeleniumTestContext
        {
#if !SLIM
            bool isFailed = context.TestContext.CurrentTestOutcome != UnitTestOutcome.Passed;
            if (isFailed)
            {
                context.TakeAndAppendScreenshot();
            }
#endif

            context.Dispose();
        }

#if !SLIM
        /// <inheritdoc />
        protected override SeleniumTestContext OnCreateContext()
            => SeleniumTestContext.CreateContext(Settings, TestContext);
#endif
    }

    /// <summary>
    /// Generic base class for a test class containing Selenium tests.
    /// </summary>
    /// <typeparam name="TSettings">The type of the settings.</typeparam>
    /// <typeparam name="TContext">The type of the test context.</typeparam>
#if !SLIM
    [TestClass]
    [DeploymentItem("geckodriver.exe")]
    [DeploymentItem("IEDriverServer.exe")]
    [DeploymentItem("webdriver_prefs.json")]
#endif
    [AddKeysClass("SettingKeys")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic representation")]
    public abstract class SeleniumTestBase<TSettings, TContext> : TestBase<TSettings, TContext>, IDriverConfiguration
        where TSettings : Settings
        where TContext : SeleniumTestContext
    {
        /// <summary>
        /// Gets the currently focused Selenium web driver.
        /// </summary>
        public static WebDriver CurrentDriver => SeleniumTestContext.CurrentContext.Driver;

        /// <inheritdoc />
#if !SLIM
        [TestInitialize]
#endif
        public override void InitializeTest()
        {
            base.InitializeTest();

            try
            {
                SeleniumTestBase.Initialize(Settings, CurrentContext, this);
            }
            catch (Exception ex)
            {
                Logger.LogCritical("Error while initializing test: {0}", ex);
                TestHelper.Try(CleanupTest);
                throw;
            }
        }

        /// <inheritdoc />
#if !SLIM
        [TestCleanup]
#endif
        public override void CleanupTest()
        {
            base.CleanupTest();

            SeleniumTestBase.Cleanup(CurrentContext);
        }

        /// <inheritdoc />
        public virtual void ConfigureIEDriverOptions(OpenQA.Selenium.IE.InternetExplorerOptions options)
        {
        }

        /// <inheritdoc />
        public virtual void ConfigureFirefoxDriverOptions(OpenQA.Selenium.Firefox.FirefoxOptions options)
        {
        }

        /// <inheritdoc />
        public virtual void ConfigureChromeDriverOptions(OpenQA.Selenium.Chrome.ChromeOptions options)
        {
        }

        /// <inheritdoc />
        public virtual void ConfigureChromeArguments(IList<string> args)
        {
        }

        /// <inheritdoc />
        public virtual void ConfigureEdgeDriverOptions(OpenQA.Selenium.Edge.EdgeOptions options)
        {
        }

        /// <inheritdoc />
        public virtual void ConfigureEdgeArguments(IList<string> args)
        {
        }

        /// <inheritdoc />
        public virtual void ConfigureOperaDriverOptions(OpenQA.Selenium.Opera.OperaOptions options)
        {
        }

        /// <inheritdoc />
        public virtual void ConfigureSafariDriverOptions(OpenQA.Selenium.Safari.SafariOptions options)
        {
        }
    }

    /// <summary>
    /// Provides methods for configuring the Selenium web driver options.
    /// </summary>
    public interface IDriverConfiguration
    {
        /// <summary>
        /// Configures the Internet Explorer driver options.
        /// </summary>
        /// <param name="options">The options to modify.</param>
        void ConfigureIEDriverOptions(OpenQA.Selenium.IE.InternetExplorerOptions options);

        /// <summary>
        /// Configures the Mozilla Firefox driver options.
        /// </summary>
        /// <param name="options">The options to modify.</param>
        void ConfigureFirefoxDriverOptions(OpenQA.Selenium.Firefox.FirefoxOptions options);

        /// <summary>
        /// Configures the Google Chrome driver options.
        /// </summary>
        /// <param name="options">The options to modify.</param>
        void ConfigureChromeDriverOptions(OpenQA.Selenium.Chrome.ChromeOptions options);

        /// <summary>
        /// Configures the Google Chrome driver arguments.
        /// </summary>
        /// <param name="args">The arguments to modify.</param>
        void ConfigureChromeArguments(IList<string> args);

        /// <summary>
        /// Configures the Microsoft Edge driver options.
        /// </summary>
        /// <param name="options">The options to modify.</param>
        void ConfigureEdgeDriverOptions(OpenQA.Selenium.Edge.EdgeOptions options);

        /// <summary>
        /// Configures the Microsoft Edge driver arguments.
        /// </summary>
        /// <param name="args">The arguments to modify.</param>
        void ConfigureEdgeArguments(IList<string> args);

        /// <summary>
        /// Configures the Opera driver options.
        /// </summary>
        /// <param name="options">The options to modify.</param>
        void ConfigureOperaDriverOptions(OpenQA.Selenium.Opera.OperaOptions options);

        /// <summary>
        /// Configures the Safari driver options.
        /// </summary>
        /// <param name="options">The options to modify.</param>
        void ConfigureSafariDriverOptions(OpenQA.Selenium.Safari.SafariOptions options);
    }
}
