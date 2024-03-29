﻿using Rocketcress.Core;
using Rocketcress.Core.Base;
using Rocketcress.Core.Extensions;
using Rocketcress.Core.Utilities;
using Rocketcress.Selenium.DriverProviders;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Rocketcress.Selenium;

/// <summary>
/// The context of a Selenium test.
/// </summary>
public class SeleniumTestContext : TestContextBase
{
    internal static readonly string DriverCachePath = Path.Combine(Path.GetTempPath(), "SeleniumDriverCache");

    private static readonly Dictionary<Browser, IDriverProvider> DriverProviders;
    private readonly IDriverConfiguration _driverConfiguration;

    static SeleniumTestContext()
    {
        DriverProviders = new Dictionary<Browser, IDriverProvider>
        {
            [Browser.Chrome] = new ChromeDriverProvider(),
            [Browser.Firefox] = new FirefoxDriverProvider(),
            [Browser.Edge] = new EdgeDriverProvider(),
        };

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            DriverProviders.Add(Browser.InternetExplorer, new InternetExplorerDriverProvider());
    }

#if !SLIM
    /// <summary>
    /// Initializes a new instance of the <see cref="SeleniumTestContext"/> class.
    /// </summary>
    /// <param name="testContext">The current MSTest test context.</param>
    /// <param name="settings">The test settings.</param>
    /// <param name="driverConfiguration">The driver configuration to use when creating web drivers.</param>
    public SeleniumTestContext(TestContext testContext, Settings settings, IDriverConfiguration driverConfiguration)
        : base(testContext, settings)
#else
    /// <summary>
    /// Initializes a new instance of the <see cref="SeleniumTestContext"/> class.
    /// </summary>
    /// <param name="settings">The test settings.</param>
    /// <param name="driverConfiguration">The driver configuration to use when creating web drivers.</param>
    public SeleniumTestContext(Settings settings, IDriverConfiguration driverConfiguration)
        : base(settings)
#endif
    {
        _driverConfiguration = driverConfiguration;
    }

    /// <summary>
    /// Gets a list of all <see cref="WebDriver"/>s that are currently running.
    /// </summary>
    public List<WebDriver> AllOpenedDrivers { get; } = new();

    /// <summary>
    /// Gets the currently selected <see cref="WebDriver"/>.
    /// </summary>
    public WebDriver Driver { get; private set; }

    /// <summary>
    /// Gets the test settings.
    /// </summary>
    public new Settings Settings => (Settings)base.Settings;

    /// <inheritdoc/>
    public override bool CanTakeScreenshot { get; } = true;

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
        Guard.NotNull(language);
        Guard.NotNull(settings);

        string host = new Uri(settings.LoginUrl).Host;
        var browserTimeout = settings.Timeout.TotalSeconds < 60 ? TimeSpan.FromSeconds(60) : settings.Timeout;

        if (DriverProviders.TryGetValue(browser, out var provider))
        {
            return provider.CreateDriver(host, browserTimeout, language, settings, driverConfiguration);
        }
        else
        {
            throw new WebDriverException("Unknown browser: " + browser);
        }
    }

    /// <inheritdoc/>
    public override sealed void Initialize()
    {
        Initialize(Browser.Unknown, null);
    }

    /// <summary>
    /// Initializes this test context.
    /// </summary>
    /// <param name="browser">The browser to launch.</param>
    public void Initialize(Browser browser)
    {
        Initialize(browser, null);
    }

    /// <summary>
    /// Initializes this test context.
    /// </summary>
    /// <param name="browser">The browser to launch.</param>
    /// <param name="browserLanguage">The language of the browser to launch.</param>
    public virtual void Initialize(Browser browser, CultureInfo browserLanguage)
    {
        base.Initialize();

        Logger.LogDebug("Initializing test with browser {0}", browser);
        Logger.LogDebug("Running selenium test with selenium version {0}", typeof(IWebElement)?.Assembly?.GetName()?.Version?.ToString() ?? "(null)");

        if (browser != Browser.Unknown)
        {
            Settings.CurrentBrowser = browser;
        }
        else
        {
            string browserName = null;
#if !SLIM
            browserName = Convert.ToString(TestContext.Properties["TestConfiguration"])?.ToLower();
            if (string.IsNullOrEmpty(browserName))
                browserName = Convert.ToString(TestContext.Properties["__Tfs_TestConfigurationName__"])?.ToLower();
#endif

            Logger.LogDebug("TestConfiguration = '{0}'", browserName ?? "(null)");
            browser = Settings.DefaultBrowser;
            if (browserName is not null)
            {
                if (browserName.Contains("chrome"))
                    browser = Browser.Chrome;
                else if (browserName.Contains("firefox"))
                    browser = Browser.Firefox;
                else if (browserName.Contains("ie") || browserName.Contains("internet explorer") || browserName.Contains("internetexplorer"))
                    browser = Browser.InternetExplorer;
                else if (browserName.Contains("edge"))
                    browser = Browser.Edge;
            }

            Settings.CurrentBrowser = browser;
        }

        if (browserLanguage is not null)
            Settings.CurrentBrowserLanguage = browserLanguage;
        else
            Settings.CurrentBrowserLanguage = Settings.DefaultBrowserLanguage;

        try
        {
            OnInitialize(browser, browserLanguage);
        }
        catch (Exception ex)
        {
            Logger.LogCritical("Error while initializing test: {0}", ex);
            throw;
        }
    }

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
    /// <param name="language">The language that was used to create the <paramref name="driver"/>.</param>
    public void SwitchCurrentDriver(IWebDriver driver, TimeSpan timeout, CultureInfo language)
    {
        SwitchCurrentDriver(new WebDriver(this, driver, timeout, language));
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
        if (driver?.GetBrowser() == Browser.InternetExplorer)
            Wait.DefaultOptions.TimeGap = TimeSpan.FromSeconds(1);
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
        var language = Settings.CurrentBrowserLanguage;
        var driver = GetDriver(browser, language, Settings, driverConfiguration);
        driver.Manage().Window.Size = Settings.Resolution;
        SwitchCurrentDriver(driver, timeout ?? Settings.Timeout, language);
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
        Guard.NotNull(driver);
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
        if (!CanTakeScreenshot)
            return null;

        var browser = Driver.GetBrowser();
        var browserLang = Settings.CurrentBrowserLanguage;
        string browserTag = browser switch
        {
            Browser.Firefox => "FF",
            Browser.Chrome => "CH",
            Browser.InternetExplorer => "IE",
            Browser.Edge => "ED",
            _ => "__",
        };
        browserTag += $"-{browserLang.Name}";

        return base.TakeAndAppendScreenshot($"{browserTag}_{name}");
    }

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

    /// <inheritdoc/>
    protected override void OnContextCreated()
    {
        base.OnContextCreated();
    }

    /// <inheritdoc/>
    protected override sealed void OnInitialize()
    {
        base.OnInitialize();
    }

    /// <summary>
    /// This method is called when the context is initializing.
    /// </summary>
    /// <param name="browser">The browser to launch.</param>
    /// <param name="browserLanguage">The language of the browser to launch.</param>
    protected virtual void OnInitialize(Browser browser, CultureInfo browserLanguage)
    {
#if !SLIM
        Logger.LogDebug("Running test \"{0}\" with browser \"{1}\" (lang: {2})...", TestContext.TestName, Settings.CurrentBrowser, Settings.CurrentBrowserLanguage);
#else
        Logger.LogDebug("Running with browser \"{0}\" (lang: {1})...", Settings.CurrentBrowser, Settings.CurrentBrowserLanguage);
#endif

        CreateAndSwitchToNewDriver(Settings.CurrentBrowser, null, _driverConfiguration);
        Driver.Manage().Timeouts().PageLoad = Settings.Timeout;
        Driver.Manage().Timeouts().AsynchronousJavaScript = Settings.Timeout;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && browser == Browser.InternetExplorer)
            DesktopUtility.SetCursorPosition(0, 0);
    }

    /// <inheritdoc />
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = "SLIM check")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1111:Closing parenthesis should be on line of last parameter", Justification = "SLIM check")]
    protected override void Dispose(bool disposing)
    {
#if !SLIM
        if (disposing)
        {
            bool isFailed = TestContext.CurrentTestOutcome != UnitTestOutcome.Passed;
            if (isFailed)
            {
                TakeAndAppendScreenshot();
            }
        }
#endif

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
        if (disposing)
        {
            AllOpenedDrivers.Clear();
            Driver = null;
        }

        base.Dispose(disposing);
    }

    /// <summary>
    /// Disposes a web driver.
    /// </summary>
    /// <param name="driver">The driver to dispose.</param>
    /// <param name="settings">The current settings.</param>
    /// <param name="writeLog">Determines wether to write the browser log to the test log.</param>
    private void DisposeDriver(WebDriver driver, Settings settings, bool writeLog)
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
}
