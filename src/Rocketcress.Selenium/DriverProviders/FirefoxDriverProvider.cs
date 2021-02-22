using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Rocketcress.Selenium.DriverProviders
{
    /// <summary>
    /// Represents a class which provides logic to create web drivers for Mozilla Firefox.
    /// </summary>
    public class FirefoxDriverProvider : IDriverProvider
    {
        /// <inheritdoc />
        public IWebDriver CreateDriver(string host, TimeSpan browserTimeout, CultureInfo language, Settings settings, IDriverConfiguration driverConfiguration)
        {
            var profile = new OpenQA.Selenium.Firefox.FirefoxProfile();
            profile.SetPreference("intl.accept_languages", language.IetfLanguageTag);   // Set browser language
            profile.SetPreference("network.automatic-ntlm-auth.trusted-uris", host);    // Add host as trusted machines for NT Authentication
            profile.SetPreference("dom.ipc.plugins.enabled", false);    // Disable process separation due to error messages after test run: https://bugzilla.mozilla.org/show_bug.cgi?id=1027222
            var fOptions = new OpenQA.Selenium.Firefox.FirefoxOptions();
            fOptions.SetLoggingPreference(LogType.Browser, OpenQA.Selenium.LogLevel.All);
            fOptions.Profile = profile;
            fOptions.AcceptInsecureCertificates = true;
            fOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore;
            driverConfiguration?.ConfigureFirefoxDriverOptions(fOptions);

            if (string.IsNullOrEmpty(settings.RemoteDriverUrl))
            {
                var driverPath = Path.Combine(Path.GetDirectoryName(typeof(SeleniumTestContext).Assembly.Location));
                var service = OpenQA.Selenium.Firefox.FirefoxDriverService.CreateDefaultService(driverPath, "geckodriver.exe");
                var firefoxPath = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";
                if (!File.Exists(firefoxPath))
                    firefoxPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
                service.FirefoxBinaryPath = firefoxPath;  // Firefox installation
                return this.RetryCreateDriver(() => new OpenQA.Selenium.Firefox.FirefoxDriver(service, fOptions, browserTimeout));
            }
            else
                return this.RetryCreateDriver(() => new OpenQA.Selenium.Remote.RemoteWebDriver(new Uri(settings.RemoteDriverUrl), fOptions));
        }

        /// <inheritdoc />
        public IEnumerable<int> GetProcessIds()
        {
            return Process.GetProcessesByName("firefox")
                .Concat(Process.GetProcessesByName("geckodriver"))
                .Select(x => x.Id).ToArray();
        }
    }
}
