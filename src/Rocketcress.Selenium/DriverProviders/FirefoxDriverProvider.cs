using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System.Globalization;

namespace Rocketcress.Selenium.DriverProviders;

/// <summary>
/// Represents a class which provides logic to create web drivers for Mozilla Firefox.
/// </summary>
public class FirefoxDriverProvider : IDriverProvider
{
    /// <inheritdoc />
    public IWebDriver CreateDriver(string host, TimeSpan browserTimeout, CultureInfo language, Settings settings, IDriverConfiguration driverConfiguration)
    {
        Guard.NotNull(language);
        Guard.NotNull(settings);

        var profile = new FirefoxProfile();
        profile.SetPreference("intl.accept_languages", language.Name);   // Set browser language
        profile.SetPreference("network.automatic-ntlm-auth.trusted-uris", host);    // Add host as trusted machines for NT Authentication
        profile.SetPreference("dom.ipc.plugins.enabled", false);    // Disable process separation due to error messages after test run: https://bugzilla.mozilla.org/show_bug.cgi?id=1027222
        var options = new FirefoxOptions();
        options.SetLoggingPreference(LogType.Browser, LogLevel.All);
        options.Profile = profile;
        options.AcceptInsecureCertificates = true;
        options.UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore;
        driverConfiguration?.ConfigureFirefoxDriverOptions(options);

        if (string.IsNullOrEmpty(settings.RemoteDriverUrl))
        {
            return this.RetryCreateDriver(() => new FirefoxDriver(CreateDriverService(), options, browserTimeout));
        }
        else
        {
            return this.RetryCreateDriver(() => new RemoteWebDriver(new Uri(settings.RemoteDriverUrl), options));
        }
    }

    /// <inheritdoc />
    public IEnumerable<int> GetProcessIds()
    {
        return Process.GetProcessesByName("firefox")
            .Concat(Process.GetProcessesByName("geckodriver"))
            .Select(x => x.Id).ToArray();
    }

    private static FirefoxDriverService CreateDriverService()
    {
        var driverPath = Path.Combine(Path.GetDirectoryName(typeof(SeleniumTestContext).Assembly.Location));
        var driverService = FirefoxDriverService.CreateDefaultService(driverPath, "geckodriver.exe");
        var firefoxPath = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";
        if (!File.Exists(firefoxPath))
            firefoxPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
        driverService.FirefoxBinaryPath = firefoxPath;  // Firefox installation
        return driverService;
    }
}
