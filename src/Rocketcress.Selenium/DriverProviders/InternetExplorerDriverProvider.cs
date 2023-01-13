using Microsoft.Win32;
using OpenQA.Selenium;
using Rocketcress.Core;
using Rocketcress.Core.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Rocketcress.Selenium.DriverProviders
{
    /// <summary>
    /// Represents a class which provides logic to create and clean up web drivers for Microsoft Internet Explorer.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class InternetExplorerDriverProvider : IDriverProvider, IDriverCleaner
    {
        private const string IEPrevLang = "IEPrevLang";
        private const string IEPrevPopupMgr = "IEPrevPopupMgr";
        private const string IEPrevSecAuth = "IEPrevSecAuth";

        /// <inheritdoc />
        public IWebDriver CreateDriver(string host, TimeSpan browserTimeout, CultureInfo language, Settings settings, IDriverConfiguration driverConfiguration)
        {
            var ieOptions = new OpenQA.Selenium.IE.InternetExplorerOptions
            {
                EnablePersistentHover = true,
                IgnoreZoomLevel = true,
                EnsureCleanSession = true,
                UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore,
            };
            ieOptions.SetLoggingPreference(LogType.Browser, OpenQA.Selenium.LogLevel.All);
            driverConfiguration?.ConfigureIEDriverOptions(ieOptions);

            if (string.IsNullOrEmpty(settings.RemoteDriverUrl))
            {
                var driverPath = Path.Combine(Path.GetDirectoryName(typeof(SeleniumTestContext).Assembly.Location));
                var iePaths = GetInternetExplorerDriverPath(driverPath);

                // Set browser language
                SetRegistryValue(Registry.CurrentUser, @"Software\Microsoft\Internet Explorer\International", "AcceptLanguage", language.IetfLanguageTag, RegistryValueKind.String, settings, IEPrevLang);

                // Disable Popup blocker
                SetRegistryValue(Registry.CurrentUser, @"Software\Microsoft\Internet Explorer\New Windows", "PopupMgr", "no", RegistryValueKind.String, settings, IEPrevPopupMgr);

                // Enable automatic NT Authentication
                SetRegistryValue(Registry.CurrentUser, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings\Zones\2", "1A00", 0, RegistryValueKind.DWord, settings, IEPrevSecAuth);

                // Add host as trusted machines for NT Authentication
                SetRegistryValue(Registry.CurrentUser, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings\ZoneMap\Domains\" + host, "*", 2, RegistryValueKind.DWord);

                // Disable first run wizard
                try
                {
                    SetInternetExplorerFirstPage();
                }
                catch (Exception ex)
                {
                    Logger.LogWarning("An error occurred while setting the first page of IE: " + ex);
                }

                return this.RetryCreateDriver(() =>
                    new OpenQA.Selenium.IE.InternetExplorerDriver(
                        OpenQA.Selenium.IE.InternetExplorerDriverService.CreateDefaultService(iePaths.DriverPath, iePaths.DriverExecutableName),
                        ieOptions,
                        browserTimeout));
            }
            else
            {
                return this.RetryCreateDriver(() => new OpenQA.Selenium.Remote.RemoteWebDriver(new Uri(settings.RemoteDriverUrl), ieOptions));
            }
        }

        /// <inheritdoc />
        public IEnumerable<int> GetProcessIds()
        {
            return Process.GetProcessesByName("iexplore")
                .Concat(Process.GetProcessesByName("IEDriverServer"))
                .Select(x => x.Id).ToArray();
        }

        /// <inheritdoc />
        public void CleanupDriver(IWebDriver driver, Settings settings)
        {
            driver.Quit(); // Double Quit because of the IE Driver

            // Rollback all changes made to the registry -->
            RestoreRegistryValue(Registry.CurrentUser, @"Software\Microsoft\Internet Explorer\International", "AcceptLanguage", settings, IEPrevLang);
            RestoreRegistryValue(Registry.CurrentUser, @"Software\Microsoft\Internet Explorer\New Windows", "PopupMgr", settings, IEPrevPopupMgr);
            RestoreRegistryValue(Registry.CurrentUser, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings\Zones\2", "1A00", settings, IEPrevSecAuth);
        }

        private static (string DriverPath, string DriverExecutableName) GetInternetExplorerDriverPath(string driverSourcePath)
        {
            const string driverFileName = "IEDriverServer.exe";

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new NotSupportedException("The Internet Explorer is only supported on Windows.");

            var driverSourceFilePath = Path.Combine(driverSourcePath, driverFileName);
            var driverTempPath = Path.Combine(SeleniumTestContext.DriverCachePath, "Internet Explorer");
            var driverFilePath = Path.Combine(driverTempPath, driverFileName);

            Directory.CreateDirectory(driverTempPath);

            for (int i = 2; !CopyDriver(driverSourceFilePath, driverFilePath); i++)
            {
                driverFilePath = Path.Combine(driverTempPath, $"{Path.GetFileNameWithoutExtension(driverFileName)}_{i:000}{Path.GetExtension(driverFileName)}");
            }

            Logger.LogInfo($"Using driver \"{driverFilePath}\"");
            return (Path.GetDirectoryName(driverFilePath), Path.GetFileName(driverFilePath));

            bool CopyDriver(string source, string target)
            {
                try
                {
                    File.Copy(source, target, true);
                    return true;
                }
                catch (IOException ex)
                {
                    Logger.LogWarning($"Could no copy IE driver to \"{driverFilePath}\": {ex.Message}");
                    return false;
                }
            }
        }

        private static RegistryKey GetRegistrySubKey(RegistryKey parent, string path)
        {
            var key = parent.OpenSubKey(path, true);
            if (key == null)
                key = parent.CreateSubKey(path);
            return key;
        }

        private static void SetRegistryValue(RegistryKey parent, string path, string valueName, object value, RegistryValueKind valueKind) => SetRegistryValue(parent, path, valueName, value, valueKind, null, null);
        private static void SetRegistryValue(RegistryKey parent, string path, string valueName, object value, RegistryValueKind valueKind, SettingsBase settings, string backupKeyName)
        {
            var key = GetRegistrySubKey(parent, path);
            if (settings != null && !string.IsNullOrEmpty(backupKeyName) && !settings.OtherSettings.ContainsKey(backupKeyName))
                settings[backupKeyName] = key.GetValue(valueName, null);
            key.SetValue(valueName, value, valueKind);
            key.Close();
        }

        private static void SetInternetExplorerFirstPage()
        {
            /*PHKO: This key is in one of four locations on every machine, based on the initial configuration. (see: https://www.geoffchappell.com/notes/windows/ie/firstrun.htm)
            Therefore very dirty hack is required*/
            try
            {
                SetRegistryValue(Registry.LocalMachine, @"Software\Policies\Microsoft\Internet Explorer\Main", "DisableFirstRunCustomize", 1, RegistryValueKind.DWord);
            }
            catch (UnauthorizedAccessException)
            {
                try
                {
                    SetRegistryValue(Registry.CurrentUser, @"Software\Policies\Microsoft\Internet Explorer\Main", "DisableFirstRunCustomize", 1, RegistryValueKind.DWord);
                }
                catch (UnauthorizedAccessException)
                {
                    try
                    {
                        SetRegistryValue(Registry.CurrentUser, @"Software\Microsoft\Internet Explorer\Main", "DisableFirstRunCustomize", 1, RegistryValueKind.DWord);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        SetRegistryValue(Registry.LocalMachine, @"Software\Microsoft\Internet Explorer\Main", "DisableFirstRunCustomize", 1, RegistryValueKind.DWord);
                    }
                }
            }
        }

        private static void RestoreRegistryValue(RegistryKey parent, string path, string valueName, SettingsBase settings, string backupKeyName)
        {
            var key = GetRegistrySubKey(parent, path);
            if (settings != null && settings.OtherSettings.TryGetValue(backupKeyName, out object value))
            {
                if (value != null)
                    key.SetValue(valueName, value);
                else
                    key.DeleteValue(valueName);
            }

            key.Close();
        }
    }
}
