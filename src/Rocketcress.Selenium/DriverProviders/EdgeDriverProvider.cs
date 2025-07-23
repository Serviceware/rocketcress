using Microsoft.Win32;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;
using Rocketcress.Core;
using System.Globalization;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace Rocketcress.Selenium.DriverProviders;

/// <summary>
/// Represents a class which provides logic to create web drivers for Microsoft Edge.
/// </summary>
public class EdgeDriverProvider : IDriverProvider
{
    private static readonly object _driverDownloadLock = new();

    /// <inheritdoc />
    public IWebDriver CreateDriver(string host, TimeSpan browserTimeout, CultureInfo language, Settings settings, IDriverConfiguration driverConfiguration)
    {
        Guard.NotNull(language);
        Guard.NotNull(settings);

        var options = new EdgeOptions();
        var arguments = new List<string>
            {
                $"--lang={language.Name}",                   // Set browser language
                $"--auth-server-whitelist=*{host}",     // Add host as trusted machines for NT Authentication
                "--disable-extensions",                 // Disable extensions (faster)
                "--inprivate",
                "--no-sandbox",
                "--disable-infobars",
            };
        driverConfiguration?.ConfigureEdgeArguments(arguments);
        options.AddArguments(arguments);
        options.UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore;
        options.AcceptInsecureCertificates = true;
        options.SetLoggingPreference(LogType.Browser, OpenQA.Selenium.LogLevel.All);
        driverConfiguration?.ConfigureEdgeDriverOptions(options);

        if (string.IsNullOrEmpty(settings.RemoteDriverUrl))
        {
            var (driverPath, driverExecutableName) = GetEdgeDriverPath();
            return this.RetryCreateDriver(() =>
                new EdgeDriver(
                    EdgeDriverService.CreateDefaultService(driverPath, driverExecutableName),
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
        return Process.GetProcessesByName("msedge")
            .Concat(Process.GetProcessesByName("msedgedriver"))
            .Select(x => x.Id).ToArray();
    }

    private static (string DriverPath, string DriverExecutableName) GetEdgeDriverPath()
    {
        string urlFormat;
        string driverFileName;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            urlFormat = "https://msedgedriver.microsoft.com/{0}/edgedriver_win32.zip";
            driverFileName = "msedgedriver.exe";
        }
        else
        {
            throw new NotSupportedException("The Web Driver for Edge cannot be retrieved for this operating system.");
        }

        var edgeVersion = (string)(Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Edge\BLBeacon")?.GetValue("version")
            ?? throw new InvalidOperationException("The version of Microsoft Edge (Chromium) could not be obatined. Please verify that Edge is installed."));
        Logger.LogInfo("Detected installed version of Microsoft Edge: " + edgeVersion);

        var driverTempPath = Path.Combine(SeleniumTestContext.DriverCachePath, "Edge " + edgeVersion);
        var driverFilePath = Path.Combine(driverTempPath, driverFileName);

        if (!File.Exists(driverFilePath))
        {
            lock (_driverDownloadLock)
            {
                if (!File.Exists(driverFilePath))
                    DownloadDriver(urlFormat, driverFileName, edgeVersion, driverTempPath, driverFilePath);
            }
        }
        else
        {
            Logger.LogInfo("Driver already exists in Cache.");
        }

        Logger.LogInfo($"Using driver \"{driverFileName}\" in folder \"{driverTempPath}\"");
        return (driverTempPath, driverFileName);
    }

    private static void DownloadDriver(string urlFormat, string driverFileName, string edgeVersion, string driverTempPath, string driverFilePath)
    {
        Directory.CreateDirectory(driverTempPath);
        var driverZipUrl = string.Format(urlFormat, edgeVersion);
        var zipPath = Path.Combine(driverTempPath, "edgedriver_win32.zip");

        Logger.LogInfo($"Driver does not exist in Cache. Downloading correct driver from {driverZipUrl}...");
        using (var client = new HttpClient())
        using (var response = client.GetAsync(driverZipUrl).Result)
        using (var driverZipStream = new FileStream(zipPath, FileMode.Create))
        {
            response.EnsureSuccessStatusCode();
            response.Content.CopyToAsync(driverZipStream).Wait();
        }

        using (var zip = ZipFile.OpenRead(zipPath))
        {
            var driverEntry = zip.GetEntry(driverFileName)
                ?? throw new InvalidOperationException($"The downloaded file from \"{driverZipUrl}\" does not contain a \"{driverFileName}\" file.");
            driverEntry.ExtractToFile(driverFilePath);
        }

        File.Delete(zipPath);

        Logger.LogInfo("Successfully downloaded and unzipped driver to: " + driverFilePath);
    }
}
