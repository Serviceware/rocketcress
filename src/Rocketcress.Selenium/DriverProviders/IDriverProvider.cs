using OpenQA.Selenium;
using Rocketcress.Core;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Rocketcress.Selenium.DriverProviders
{
    /// <summary>
    /// Represents a class that provides logic to create a new Selenium Driver.
    /// </summary>
    public interface IDriverProvider
    {
        /// <summary>
        /// Retrieves the currently open process ids of the browser and driver for which the <see cref="IDriverProvider"/> is implemented for.
        /// </summary>
        /// <returns>The process ids of the open browsers.</returns>
        IEnumerable<int> GetProcessIds();

        /// <summary>
        /// Create a new web driver for the browser for which the <see cref="IDriverProvider"/> is implemented for.
        /// </summary>
        /// <param name="host">The host that should be trusted by the browser.</param>
        /// <param name="browserTimeout">The default browser timeout.</param>
        /// <param name="language">The language in which to start the browser.</param>
        /// <param name="settings">The settings to use.</param>
        /// <param name="driverConfiguration">The <see cref="IDriverConfiguration"/> that should be used for further configuration.</param>
        /// <returns>Returns a web driver for the specified browser.</returns>
        IWebDriver CreateDriver(string host, TimeSpan browserTimeout, CultureInfo language, Settings settings, IDriverConfiguration driverConfiguration);
    }

    /// <summary>
    /// Represents a class that provides logic for cleaning up Selenium Drivers.
    /// </summary>
    public interface IDriverCleaner
    {
        /// <summary>
        /// Cleans up a driver.
        /// </summary>
        /// <param name="driver">The driver to clean up.</param>
        /// <param name="settings">The settings to use.</param>
        void CleanupDriver(IWebDriver driver, Settings settings);
    }

    /// <summary>
    /// Provides internal extension methods for the <see cref="IDriverProvider"/> interface.
    /// </summary>
    internal static class InternalDriverProviderExtensions
    {
        /// <summary>
        /// Tries to create a driver and retries 4 times.
        /// </summary>
        /// <param name="provider">The driver provider.</param>
        /// <param name="createFunction">The function that is used to create a web driver.</param>
        /// <returns>Returns the created web driver.</returns>
        public static IWebDriver RetryCreateDriver(this IDriverProvider provider, Func<IWebDriver> createFunction, Func<Exception> onError = null)
        {
            IWebDriver result = default;
            var success = TestHelper.RetryAction(
                () => result = createFunction(),
                maxRetryCount: 4,
                onException: ex =>
                {
                    result?.Dispose();
                    onError?.Invoke();
                    SeleniumTestContext.KillAllDrivers(false);
                });
            if (!success)
                throw new WebDriverException("Could not create driver. See earlier exceptions.");
            return result;
        }
    }
}
