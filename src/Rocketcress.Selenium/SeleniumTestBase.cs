using Rocketcress.Core.Base;
#if !SLIM
#endif

namespace Rocketcress.Selenium
{
    /// <summary>
    /// Base class for a test class containing Selenium tests.
    /// </summary>
#if !SLIM
    [DeploymentItem("geckodriver.exe")]
    [DeploymentItem("IEDriverServer.exe")]
    [DeploymentItem("webdriver_prefs.json")]
#endif
    [AddKeysClass("SettingKeys")]
    public abstract class SeleniumTestBase : SeleniumTestBase<Settings, SeleniumTestContext>
    {
    }

    /// <summary>
    /// Generic base class for a test class containing Selenium tests.
    /// </summary>
    /// <typeparam name="TSettings">The type of the settings.</typeparam>
    /// <typeparam name="TContext">The type of the test context.</typeparam>
#if !SLIM
    [DeploymentItem("geckodriver.exe")]
    [DeploymentItem("IEDriverServer.exe")]
    [DeploymentItem("webdriver_prefs.json")]
#endif
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic representation")]
    public abstract class SeleniumTestBase<TSettings, TContext> : TestBase<TSettings, TContext>, IDriverConfiguration
        where TSettings : Settings
        where TContext : SeleniumTestContext
    {
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
