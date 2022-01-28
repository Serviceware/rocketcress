using Microsoft.Win32;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;
using Rocketcress.Core;
using System.Globalization;
using System.IO.Compression;
using System.Net;
using System.Runtime.InteropServices;

namespace Rocketcress.Selenium.DriverProviders;

/// <summary>
/// Represents a class which provides logic to create web drivers for Microsoft Edge.
/// </summary>
public class EdgeDriverProvider : IDriverProvider
{
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
            var driverService = EdgeDriverService.CreateDefaultService(driverPath, driverExecutableName);
            return this.RetryCreateDriver(() => new EdgeDriver(driverService, options, browserTimeout));
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
            urlFormat = "https://msedgedriver.azureedge.net/{0}/edgedriver_win32.zip";
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
            Directory.CreateDirectory(driverTempPath);
            var driverZipUrl = string.Format(urlFormat, edgeVersion);
            var zipPath = Path.Combine(driverTempPath, "edgedriver_win32.zip");

            Logger.LogInfo($"Driver does not exist in Cache. Downloading correct driver from {driverZipUrl}...");
            using (var client = new WebClient())
            {
                client.DownloadFile(driverZipUrl, zipPath);
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
        else
        {
            Logger.LogInfo("Driver already exists in Cache.");
        }

        Logger.LogInfo($"Using driver \"{driverFileName}\" in folder \"{driverTempPath}\"");
        return (driverTempPath, driverFileName);
    }
}
