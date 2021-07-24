using Microsoft.Win32;
using OpenQA.Selenium;
using Rocketcress.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;

namespace Rocketcress.Selenium.DriverProviders
{
    /// <summary>
    /// Represents a class which provides logic to create web drivers for Google Chrome.
    /// </summary>
    public class ChromeDriverProvider : IDriverProvider
    {
        /// <inheritdoc />
        public IWebDriver CreateDriver(string host, TimeSpan browserTimeout, CultureInfo language, Settings settings, IDriverConfiguration driverConfiguration)
        {
            var cOptions = new OpenQA.Selenium.Chrome.ChromeOptions();
            var cArgs = new List<string>
            {
                "--lang=" + language.IetfLanguageTag,   // Set browser language
                "--auth-server-whitelist=*" + host,     // Add host as trusted machines for NT Authentication
                "--disable-extensions",                 // Disable extensions (faster)
                "--incognito",
                "no-sandbox",
                "disable-infobars",
            };
            driverConfiguration?.ConfigureChromeArguments(cArgs);
            cOptions.AddArguments(cArgs);
            cOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore;
            cOptions.AcceptInsecureCertificates = true;
            cOptions.SetLoggingPreference(LogType.Browser, OpenQA.Selenium.LogLevel.All);
            driverConfiguration?.ConfigureChromeDriverOptions(cOptions);

            if (string.IsNullOrEmpty(settings.RemoteDriverUrl))
            {
                var (driverPath, driverExecutableName) = GetChromeDriverPath();
                var cService = OpenQA.Selenium.Chrome.ChromeDriverService.CreateDefaultService(driverPath, driverExecutableName);
                return this.RetryCreateDriver(() => new OpenQA.Selenium.Chrome.ChromeDriver(cService, cOptions, browserTimeout));
            }
            else
            {
                return this.RetryCreateDriver(() => new OpenQA.Selenium.Remote.RemoteWebDriver(new Uri(settings.RemoteDriverUrl), cOptions));
            }
        }

        /// <inheritdoc />
        public IEnumerable<int> GetProcessIds()
        {
            return Process.GetProcessesByName("chrome")
                .Concat(Process.GetProcessesByName("chromedriver"))
                .Concat(Process.GetProcessesByName("google-chrome"))
                .Select(x => x.Id).ToArray();
        }

        private static (string DriverPath, string DriverExecutableName) GetChromeDriverPath()
        {
            const string urlLatestVersionFormat = "https://chromedriver.storage.googleapis.com/LATEST_RELEASE_{0}";
            string urlFormat;
            string driverFileName;
            string chromeVersion;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                urlFormat = "https://chromedriver.storage.googleapis.com/{0}/chromedriver_win32.zip";
                driverFileName = "chromedriver.exe";
                chromeVersion = Registry.CurrentUser.OpenSubKey(@"Software\Google\Chrome\BLBeacon")?.GetValue("version") as string;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                urlFormat = "https://chromedriver.storage.googleapis.com/{0}/chromedriver_linux64.zip";
                driverFileName = "chromedriver";
                chromeVersion = DesktopUtility.RunBashCommand("google-chrome --version | grep -iE \"[0-9.]{10,20}\"");
            }
            else
            {
                throw new NotSupportedException("The Web Driver for Chrome cannot be retrieved for this operating system.");
            }

            if (string.IsNullOrWhiteSpace(chromeVersion))
                throw new InvalidOperationException("The version of Google Chrome could not be obatined. Please verify that Chrome is installed.");
            var chromeMajorVersion = new Version(chromeVersion).Major;
            Logger.LogInfo("Detected installed version of Google Chrome: " + chromeVersion);

            var driverTempPath = Path.Combine(SeleniumTestContext.DriverCachePath, $"Chrome {chromeMajorVersion}.x");
            var driverFilePath = Path.Combine(driverTempPath, driverFileName);

            if (!File.Exists(driverFilePath))
            {
                Logger.LogInfo("Driver does not exist in Cache.");
                Directory.CreateDirectory(driverTempPath);
                string driverVersionUrl = string.Format(urlLatestVersionFormat, chromeMajorVersion);
                string driverVersion;
                string driverZipUrl;
                var zipPath = Path.Combine(driverTempPath, "chromedriver.zip");

                using (var client = new WebClient())
                {
                    Logger.LogInfo($"Downloading correct driver version from {driverVersionUrl}...");
                    driverVersion = client.DownloadString(driverVersionUrl);

                    driverZipUrl = string.Format(urlFormat, driverVersion);
                    Logger.LogInfo($"Downloading correct driver from {driverZipUrl}...");
                    client.DownloadFile(driverZipUrl, zipPath);
                }

                using (var zip = ZipFile.OpenRead(zipPath))
                {
                    var driverEntry = zip.GetEntry(driverFileName) ?? throw new InvalidOperationException($"The downloaded file from \"{driverZipUrl}\" does not contain a \"{driverFileName}\" file.");
                    driverEntry.ExtractToFile(driverFilePath);
                }

                File.Delete(zipPath);

                Logger.LogInfo("Successfully downloaded and unzipped driver to: " + driverFilePath);
            }
            else
            {
                Logger.LogInfo("Driver already exists in Cache.");
            }

            Logger.LogInfo($"Using driver \"{driverFileName}\" in folder \"{driverTempPath}\"");
            return (driverTempPath, driverFileName);
        }
    }
}
