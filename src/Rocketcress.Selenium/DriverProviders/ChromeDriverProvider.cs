﻿using Microsoft.Win32;
using Newtonsoft.Json.Linq;
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
            return this.RetryCreateDriver(() =>
                new ChromeDriver(
                    ChromeDriverService.CreateDefaultService(driverPath, driverExecutableName),
                    options,
                    browserTimeout));
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
        string driverFileName;
        string chromeVersion;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            driverFileName = "chromedriver.exe";
            chromeVersion = Registry.CurrentUser.OpenSubKey(@"Software\Google\Chrome\BLBeacon")?.GetValue("version") as string;
        }
#if !NETFRAMEWORK
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
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
                    DownloadDriver(chromeMajorVersion, driverTempPath, driverFilePath);
            }
        }
        else
        {
            Logger.LogInfo("Driver already exists in Cache.");
        }

        Logger.LogInfo($"Using driver \"{driverFileName}\" in folder \"{driverTempPath}\"");
        return (driverTempPath, driverFileName);
    }

    private static (string Url, string DriverFileEntry) GetChromeDriverUrl(int chromeMajorVersion)
    {
        var platform = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "win32" : "linux64";

        if (chromeMajorVersion < 115)
        {
            const string urlLatestVersionFormat = "https://chromedriver.storage.googleapis.com/LATEST_RELEASE_{0}";
            var driverVersionUrl = string.Format(urlLatestVersionFormat, chromeMajorVersion);

            using var client = new HttpClient();
            var driverVersion = client.GetStringAsync(driverVersionUrl).Result;
            return (
                $"https://chromedriver.storage.googleapis.com/{driverVersion}/chromedriver_{platform}.zip",
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "chromedriver.exe" : "chromedriver");
        }
        else
        {
            using var client = new HttpClient();
            return (
                JObject
                    .Parse(client.GetStringAsync("https://googlechromelabs.github.io/chrome-for-testing/known-good-versions-with-downloads.json").Result)
                    .GetValue("versions").Values<JObject>()
                    .Select(x => new
                    {
                        Version = Version.Parse(x["version"].Value<string>()),
                        Url = (x["downloads"]?["chromedriver"] as JArray)?.FirstOrDefault(y => y?["platform"]?.Value<string>() == platform)?["url"]?.Value<string>(),
                    })
                    .Where(x => x.Version.Major == chromeMajorVersion && x.Url is not null)
                    .OrderByDescending(x => x.Version)
                    .FirstOrDefault()
                    ?.Url
                    ?? throw new KeyNotFoundException($"No chrome driver was found for version {chromeMajorVersion}."),
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? $"chromedriver-{platform}/chromedriver.exe" : $"chromedriver-{platform}/chromedriver");
        }
    }

    private static void DownloadDriver(int chromeMajorVersion, string driverTempPath, string driverFilePath)
    {
        Logger.LogInfo("Driver does not exist in Cache.");
        Directory.CreateDirectory(driverTempPath);

        Logger.LogInfo($"Determining correct chrome driver version for version {chromeMajorVersion}...");
        var (driverZipUrl, driverZipEntry) = GetChromeDriverUrl(chromeMajorVersion);
        var zipPath = Path.Combine(driverTempPath, "chromedriver.zip");

        using (var client = new HttpClient())
        {
            using var response = client.GetAsync(driverZipUrl).Result;
            using var driverZipStream = new FileStream(zipPath, FileMode.Create);
            response.EnsureSuccessStatusCode();
            response.Content.CopyToAsync(driverZipStream).Wait();
        }

        using (var zip = ZipFile.OpenRead(zipPath))
        {
            var driverEntry = zip.GetEntry(driverZipEntry) ?? throw new InvalidOperationException($"The downloaded file from \"{driverZipUrl}\" does not contain a \"{driverZipEntry}\" file.");
            driverEntry.ExtractToFile(driverFilePath);
        }

        File.Delete(zipPath);

        Logger.LogInfo("Successfully downloaded and unzipped driver to: " + driverFilePath);
    }
}
