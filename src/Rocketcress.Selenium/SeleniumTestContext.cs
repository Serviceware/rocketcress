using OpenQA.Selenium;
using Rocketcress.Core;
using Rocketcress.Core.Base;
using Rocketcress.Core.Extensions;
using Rocketcress.Selenium.DriverProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
#if !SLIM
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Rocketcress.Selenium
{
    /// <summary>
    /// The context of a Selenium test.
    /// </summary>
    public class SeleniumTestContext : TestContextBase
    {
        #region Fields

        private static readonly Dictionary<Browser, int[]> IgnoredPidsOnClose = new Dictionary<Browser, int[]>();
        private static readonly Dictionary<Browser, IDriverProvider> DriverProviders;
        internal static readonly string DriverCachePath = Path.Combine(Path.GetTempPath(), "SeleniumDriverCache");

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current instance of the <see cref="SeleniumTestContext"/>.
        /// </summary>
        public static new SeleniumTestContext CurrentContext { get; private set; }

        /// <summary>
        /// Gets or sets the browser for which this test has been executed.
        /// </summary>
        public static Browser CurrentBrowser { get; set; }

        /// <summary>
        /// Gets or sets the browser language with which this test has been executed.
        /// </summary>
        public static CultureInfo CurrentBrowserLanguage { get; set; }

        /// <summary>
        /// Gets a list of all <see cref="WebDriver"/>s that are currently running.
        /// </summary>
        public List<WebDriver> AllOpenedDrivers { get; private set; }

        /// <summary>
        /// Gets the currently selected <see cref="WebDriver"/>.
        /// </summary>
        public WebDriver Driver { get; private set; }

        /// <summary>
        /// Gets or sets the test settings.
        /// </summary>
        public new Settings Settings
        {
            get => (Settings)base.Settings;
            set => base.Settings = value;
        }

        #endregion

        #region Constructors

        static SeleniumTestContext()
        {
            DriverProviders = new Dictionary<Browser, IDriverProvider>
            {
                [Browser.Chrome] = new ChromeDriverProvider(),
                [Browser.Firefox] = new FirefoxDriverProvider(),
                [Browser.Edge] = new EdgeDriverProvider(),
            };

#if NETFRAMEWORK
            DriverProviders.Add(Browser.InternetExplorer, new InternetExplorerDriverProvider());
#else
            if (OperatingSystem.IsWindows())
                DriverProviders.Add(Browser.InternetExplorer, new InternetExplorerDriverProvider());
#endif
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeleniumTestContext"/> class.
        /// </summary>
        protected SeleniumTestContext()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Switches the current driver to the index of AllOpenedDrivers.
        /// </summary>
        /// <param name="index">The index of the driver to switch to.</param>
        public void SwitchCurrentDriver(int index)
        {
            var driver = index < AllOpenedDrivers.Count ? AllOpenedDrivers[index] : null;
            SwitchCurrentDriver(driver);
        }

        /// <summary>
        /// Switches the current driver to the given <see cref="IWebDriver"/> with the given timeout.
        /// </summary>
        /// <param name="driver">The <see cref="IWebDriver"/>-Object.</param>
        /// <param name="timeout">The timeout.</param>
        public void SwitchCurrentDriver(IWebDriver driver, TimeSpan timeout)
        {
            SwitchCurrentDriver(new WebDriver(driver, timeout));
        }

        /// <summary>
        /// Switches the current driver to the given <see cref="WebDriver"/>.
        /// </summary>
        /// <param name="driver">The <see cref="WebDriver"/> to switch to.</param>
        public void SwitchCurrentDriver(WebDriver driver)
        {
            SwitchCurrentDriver(driver, true);
        }

        /// <summary>
        /// Switches the current driver to the given <see cref="WebDriver"/>.
        /// </summary>
        /// <param name="driver">The <see cref="WebDriver"/> to switch to.</param>
        /// <param name="switchToLastWindow">Determines wether the last opened window of the driver should be focused.</param>
        public void SwitchCurrentDriver(WebDriver driver, bool switchToLastWindow)
        {
            if (driver != null && !AllOpenedDrivers.Contains(driver))
                AllOpenedDrivers.Add(driver);
            Waiter.DefaultWaitBetweenChecks = driver?.GetBrowser() == Browser.InternetExplorer ? 1000 : 100;
            Driver = driver;
            if (switchToLastWindow)
                driver?.SwitchTo(driver.WindowHandles.Count - 1);
        }

        /// <summary>
        /// Creates a new <see cref="IWebDriver"/> and switches directly to it.
        /// </summary>
        /// <param name="browser">The browser for the <see cref="IWebDriver"/>.</param>
        /// <param name="timeout">The timeout for the <see cref="IWebDriver"/>.</param>
        /// <param name="driverConfiguration">An object that is used to confiure the selenium driver that is created.</param>
        public void CreateAndSwitchToNewDriver(Browser browser, TimeSpan? timeout = null, IDriverConfiguration driverConfiguration = null)
        {
            var driver = GetDriver(browser, Settings.CurrentBrowserLanguage, Settings, driverConfiguration);
            driver.Manage().Window.Size = Settings.Resolution;
            SwitchCurrentDriver(driver, timeout ?? Settings.Timeout);
        }

        /// <summary>
        /// Closes the current web driver.
        /// </summary>
        /// <param name="writeLog">Determines wether to write the browser log to the test log.</param>
        public void CloseCurrentDriver(bool writeLog = true) => CloseDriver(Driver, writeLog);

        /// <summary>
        /// Closes a web driver.
        /// </summary>
        /// <param name="driver">The driver to close.</param>
        /// <param name="writeLog">Determines wether to write the browser log to the test log.</param>
        public void CloseDriver(WebDriver driver, bool writeLog = true)
        {
            DisposeDriver(driver, Settings, writeLog);
            AllOpenedDrivers.Remove(driver);
            SwitchCurrentDriver(0);
        }

        /// <summary>
        /// Closes the current web driver and reopens it with the same arguments.
        /// </summary>
        public void ReopenCurrentDriver()
        {
            var browser = Driver.GetBrowser();
            var timeout = Driver.Timeout;
            CloseCurrentDriver();
            CreateAndSwitchToNewDriver(browser, timeout);
        }

        /// <inheritdoc/>
        public override string TakeAndAppendScreenshot(string name)
        {
            string browserTag = CurrentBrowser switch
            {
                Browser.Firefox => "FF",
                Browser.Chrome => "CH",
                Browser.InternetExplorer => "IE",
                Browser.Edge => "ED",
                _ => "__",
            };
            browserTag += $"-{CurrentBrowserLanguage.Name}";

            return base.TakeAndAppendScreenshot($"{browserTag}_{name}");
        }

        /// <inheritdoc/>
        protected override void OnContextCreated(TestContextBase lastContext)
        {
            base.OnContextCreated(lastContext);
            CurrentContext = this;
        }
        #endregion

        #region Public Functions

#pragma warning disable CS1572 // XML comment has a param tag, but there is no parameter by that name
#pragma warning disable SA1612 // Element parameter documentation should match element parameters
        /// <summary>
        /// Creates a new <see cref="SeleniumTestContext"/> as uses it as the current test context. Please make sure to dispose any preexisting <see cref="TestContextBase"/> instances beforehand.
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
            where T : SeleniumTestContext
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
                ctx.AllOpenedDrivers = new List<WebDriver>();
                Waiter.DefaultWaitBetweenChecks = settings.CurrentBrowser == Browser.InternetExplorer ? 1000 : 100;
                initAction?.Invoke(ctx);
            }
        }

        /// <summary>
        /// Creates a new <see cref="SeleniumTestContext"/> for the given arguments.
        /// </summary>
        /// <param name="settings">The settings for the test run.</param>
        /// <param name="context">The test context of the test run.</param>
        /// <returns>The created context.</returns>
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = "SLIM check")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1111:Closing parenthesis should be on line of last parameter", Justification = "SLIM check")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1115:Parameter should follow comma", Justification = "SLIM check")]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1001:Commas should be spaced correctly", Justification = "SLIM check")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1113:Comma should be on the same line as previous parameter", Justification = "SLIM check")]
        public static SeleniumTestContext CreateContext(
            Settings settings
#if !SLIM
          , TestContext context
#endif
            )
        {
            return CreateContext(
                () => new SeleniumTestContext(),
                settings,
#if !SLIM
                context,
#endif
                null);
        }
#pragma warning restore SA1612 // Element parameter documentation should match element parameters
#pragma warning restore CS1572 // XML comment has a param tag, but there is no parameter by that name

        /// <summary>
        /// Creates a new <see cref="IWebDriver"/>-Object for the given browser.
        /// </summary>
        /// <param name="browser">The browser for the <see cref="IWebDriver"/>.</param>
        /// <param name="language">The language for the browser.</param>
        /// <param name="settings">The settings for the test run.</param>
        /// <param name="driverConfiguration">An object that is used to confiure the selenium driver that is created.</param>
        /// <returns>The created driver.</returns>
        public static IWebDriver GetDriver(Browser browser, CultureInfo language, Settings settings, IDriverConfiguration driverConfiguration = null)
        {
            string host = new Uri(settings.LoginUrl).Host;
            var browserTimeout = settings.Timeout.TotalSeconds < 60 ? TimeSpan.FromSeconds(60) : settings.Timeout;

            if (DriverProviders.TryGetValue(browser, out var provider))
            {
                if (!IgnoredPidsOnClose.ContainsKey(browser))
                    IgnoredPidsOnClose.Add(browser, provider.GetProcessIds().ToArray());
                return provider.CreateDriver(host, browserTimeout, language, settings, driverConfiguration);
            }
            else
            {
                throw new WebDriverException("Unknown browser: " + browser);
            }
        }

        #endregion

        /// <inheritdoc />
        protected override void SaveScreenshot(string path)
        {
            Screenshot screenshot;
            try
            {
                screenshot = Driver.GetScreenshot();
            }
            catch (UnhandledAlertException)
            {
                Driver.SwitchTo().Alert().Dismiss();
                screenshot = Driver.GetScreenshot();
            }

            screenshot.SaveAsFile(path);
        }

        #region Private Functions

        /// <summary>
        /// Kills all driver processes that are still open after disposing the drivers.
        /// Browser Processes that where open before the test are not killed.
        /// </summary>
        /// <param name="killAll">Determines whether all browser processes should be killed.</param>
        internal static void KillAllDrivers(bool killAll)
        {
            foreach (var kv in IgnoredPidsOnClose)
            {
                var pids = DriverProviders.TryGetValue(kv.Key, out var provider) ? provider.GetProcessIds() : Array.Empty<int>();
                foreach (var pid in killAll ? pids : pids.Except(kv.Value))
                {
                    try
                    {
                        Process.GetProcessById(pid).Kill();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning("Could not kill process with id " + pid + ": " + ex);
                    }
                }
            }
        }

        #endregion

        #region Cleanup

        /// <inheritdoc />
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = "SLIM check")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1111:Closing parenthesis should be on line of last parameter", Justification = "SLIM check")]
        protected override void Dispose(bool disposing)
        {
            var disposeTasks = AllOpenedDrivers.Select(
                x => Task.Run(
                    () => DisposeDriver(
                        x,
                        Settings,
#if SLIM
                        true
#else
                        TestContext?.CurrentTestOutcome != UnitTestOutcome.Passed
#endif
                        ))).ToArray();
            Task.WaitAll(disposeTasks, 120000);
            AllOpenedDrivers.Clear();
            Driver = null;
            KillAllDrivers(Settings.KillAllBrowserProcessesOnCleanup);
            IgnoredPidsOnClose.Clear();

            if (CurrentContext == this)
            {
                CurrentContext = null;
                RaiseContextChangedEvent(this, null);
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Disposes a web driver.
        /// </summary>
        /// <param name="driver">The driver to dispose.</param>
        /// <param name="settings">The current settings.</param>
        /// <param name="writeLog">Determines wether to write the browser log to the test log.</param>
        public void DisposeDriver(WebDriver driver, Settings settings, bool writeLog)
        {
            try
            {
                if (writeLog)
                {
                    var entries = TestHelper.Try(() => driver.Manage()?.Logs?.GetLog(LogType.Browser));
                    Trace.WriteLine(string.Empty);
                    Trace.WriteLine($"Browser Log for Browser {AllOpenedDrivers.IndexOf(driver)} ({driver.GetBrowser()}):");
                    if (entries.IsNullOrEmpty())
                    {
                        Trace.WriteLine("No logs available!");
                    }
                    else
                    {
                        foreach (var entry in entries)
                            Trace.WriteLine($"{entry.Timestamp}: {entry.Level} - {entry.Message}");
                    }
                }

                driver.Quit();

                if (DriverProviders.TryGetValue(driver.GetBrowser(), out var provider) && provider is IDriverCleaner cleaner)
                    cleaner.CleanupDriver(driver, settings);

                driver.Dispose();
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Error while disposing driver: " + ex);
            }
        }

        #endregion
    }
}
