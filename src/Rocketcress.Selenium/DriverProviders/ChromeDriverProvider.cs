using Microsoft.Win32;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Rocketcress.Core;
using System.Globalization;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace Rocketcress.Selenium.DriverProviders;

/// <summary>
/// Represents a class which provides logic to create web drivers for Google Chrome.
/// </summary>
public class ChromeDriverProvider : IDriverProvider
{
    private const string UrlLatestVersionFormat = "https://chromedriver.storage.googleapis.com/LATEST_RELEASE_{0}";
    private static readonly object _driverDownloadLock = new();

    /// <inheritdoc />
    public IWebDriver CreateDriver(string host, TimeSpan browserTimeout, CultureInfo language, Settings settings, IDriverConfiguration driverConfiguration)
    {
        Guard.NotNull(language);
        Guard.NotNull(settings);

        var options = new ChromeOptions();
        var arguments = new List<string>
            {
                $"--lang={language.Name}",                   // Set browser language
                $"--auth-server-whitelist=*{host}",     // Add host as trusted machines for NT Authentication
                "--disable-extensions",                 // Disable extensions (faster)
                "--incognito",
                "no-sandbox",
                "disable-infobars",
            };
        driverConfiguration?.ConfigureChromeArguments(arguments);
        options.AddArguments(arguments);
        options.UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore;
        options.AcceptInsecureCertificates = true;
        options.SetLoggingPreference(LogType.Browser, OpenQA.Selenium.LogLevel.All);
        driverConfiguration?.ConfigureChromeDriverOptions(options);

        if (string.IsNullOrEmpty(settings.RemoteDriverUrl))
        {
            var (driverPath, driverExecutableName) = GetChromeDriverPath();
            var driverService = ChromeDriverService.CreateDefaultService(driverPath, driverExecutableName);
            return this.RetryCreateDriver(() => new ChromeDriver(driverService, options, browserTimeout));
        }
        else
        {
            return this.RetryCreateDriver(() => new RemoteWebDriver(new Uri(settings.RemoteDriverUrl), options));
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
        string urlFormat;
        string driverFileName;
        string chromeVersion;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            urlFormat = "https://chromedriver.storage.googleapis.com/{0}/chromedriver_win32.zip";
            driverFileName = "chromedriver.exe";
            chromeVersion = Registry.CurrentUser.OpenSubKey(@"Software\Google\Chrome\BLBeacon")?.GetValue("version") as string;
        }
#if !NETFRAMEWORK
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            urlFormat = "https://chromedriver.storage.googleapis.com/{0}/chromedriver_linux64.zip";
            driverFileName = "chromedriver";
            chromeVersion = Core.Utilities.ScriptUtility.RunBashCommand("google-chrome --version | grep -iE \"[0-9.]{10,20}\"");
        }
#endif
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
            lock (_driverDownloadLock)
            {
                if (!File.Exists(driverFilePath))
                    DownloadDriver(urlFormat, driverFileName, chromeMajorVersion, driverTempPath, driverFilePath);
            }
        }
        else
        {
            Logger.LogInfo("Driver already exists in Cache.");
        }

        Logger.LogInfo($"Using driver \"{driverFileName}\" in folder \"{driverTempPath}\"");
        return (driverTempPath, driverFileName);
    }

    private static void DownloadDriver(string urlFormat, string driverFileName, int chromeMajorVersion, string driverTempPath, string driverFilePath)
    {
        Logger.LogInfo("Driver does not exist in Cache.");
        Directory.CreateDirectory(driverTempPath);
        string driverVersionUrl = string.Format(UrlLatestVersionFormat, chromeMajorVersion);
        string driverVersion;
        string driverZipUrl;
        var zipPath = Path.Combine(driverTempPath, "chromedriver.zip");

        using (var client = new HttpClient())
        {
            driverVersion = client.GetStringAsync(driverVersionUrl).Result;

            driverZipUrl = string.Format(urlFormat, driverVersion);
            using (var response = client.GetAsync(driverZipUrl).Result)
            using (var driverZipStream = new FileStream(zipPath, FileMode.Create))
            {
                response.EnsureSuccessStatusCode();
                response.Content.CopyToAsync(driverZipStream).Wait();
            }
        }

        using (var zip = ZipFile.OpenRead(zipPath))
        {
            var driverEntry = zip.GetEntry(driverFileName) ?? throw new InvalidOperationException($"The downloaded file from \"{driverZipUrl}\" does not contain a \"{driverFileName}\" file.");
            driverEntry.ExtractToFile(driverFilePath);
        }

        File.Delete(zipPath);

        Logger.LogInfo("Successfully downloaded and unzipped driver to: " + driverFilePath);
    }
}
