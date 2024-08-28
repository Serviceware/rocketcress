using OpenQA.Selenium;
using System;
using System.Threading.Tasks;

namespace Rocketcress.Selenium.SeleniumWrappers
{
    /// <summary>
    /// Wrapper for an <see cref="INavigation"/> object.
    /// </summary>
    public class NavigationWrapper : INavigation
    {
        /// <summary>
        /// Gets the driver that is wrapped.
        /// </summary>
        protected WebDriver Driver { get; }

        /// <summary>
        /// Gets the navigation instance that is wrapped.
        /// </summary>
        protected INavigation WrappedNavigation { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationWrapper"/> class.
        /// </summary>
        /// <param name="driver">The driver that is wrapped.</param>
        /// <param name="wrappedNavigation">The <see cref="INavigation"/> that is wrapped.</param>
        public NavigationWrapper(WebDriver driver, INavigation wrappedNavigation)
        {
            Driver = driver;
            WrappedNavigation = wrappedNavigation;
        }

        /// <summary>
        /// Move back a single entry in the browser's history.
        /// </summary>
        public virtual void Back() => WrappedNavigation.Back();

        /// <summary>
        /// Move a single "item" forward in the browser's history.
        /// </summary>
        /// <remarks>Does nothing if we are on the latest page viewed.</remarks>
        public virtual void Forward() => WrappedNavigation.Forward();

        /// <summary>
        /// Refreshes the current page.
        /// </summary>
        public virtual void Refresh() => WrappedNavigation.Refresh();

        /// <summary>
        /// Load a new web page in the current browser window.
        /// </summary>
        /// <param name="url">The URL to load. It is best to use a fully qualified URL.</param>
        public void GoToUrl(string url) => GoToUrlImpl(url, null);

        /// <summary>
        /// Load a new web page in the current browser window.
        /// </summary>
        /// <param name="url">The URL to load. It is best to use a fully qualified URL.</param>
        /// <param name="timeout">The timeout in miliseconds.</param>
        public void GoToUrl(string url, int timeout) => GoToUrlImpl(url, TimeSpan.FromMilliseconds(timeout));

        /// <summary>
        /// Load a new web page in the current browser window.
        /// </summary>
        /// <param name="url">The URL to load. It is best to use a fully qualified URL.</param>
        /// <param name="timeout">The timeout.</param>
        public void GoToUrl(string url, TimeSpan timeout) => GoToUrlImpl(url, timeout);

        /// <summary>
        /// Load a new web page in the current browser window.
        /// </summary>
        /// <param name="url">The URL to load.</param>
        public void GoToUrl(Uri url) => GoToUrlImpl(url, null);

        /// <summary>
        /// Load a new web page in the current browser window.
        /// </summary>
        /// <param name="url">The URL to load.</param>
        /// <param name="timeout">The timeout in miliseconds.</param>
        public void GoToUrl(Uri url, int timeout) => GoToUrlImpl(url, TimeSpan.FromMilliseconds(timeout));

        /// <summary>
        /// Load a new web page in the current browser window.
        /// </summary>
        /// <param name="url">The URL to load.</param>
        /// <param name="timeout">The timeout.</param>
        public void GoToUrl(Uri url, TimeSpan timeout) => GoToUrlImpl(url, timeout);

        /// <inheritdoc />
        public Task BackAsync() => WrappedNavigation.BackAsync();

        /// <inheritdoc />
        public Task ForwardAsync() => WrappedNavigation.ForwardAsync();

        /// <inheritdoc />
        public Task GoToUrlAsync(string url) => GoToUrlAsyncImpl(url, null);

        /// <inheritdoc />
        public Task GoToUrlAsync(Uri url) => GoToUrlAsyncImpl(url, null);

        /// <inheritdoc />
        public Task RefreshAsync() => WrappedNavigation.RefreshAsync();

        /// <summary>
        /// Navigates to the a given URL.
        /// </summary>
        /// <param name="url">The URL to navigate to.</param>
        /// <param name="timeout">The timeout for the navigation action.</param>
        protected virtual void GoToUrlImpl(object url, TimeSpan? timeout)
        {
            if (timeout.HasValue)
                Driver.Manage().Timeouts().PageLoad = timeout.Value;

            try
            {
                if (url == null)
                    WrappedNavigation.GoToUrl((string)null);
                else if (url is string sUrl)
                    WrappedNavigation.GoToUrl(sUrl);
                else if (url is Uri uUrl)
                    WrappedNavigation.GoToUrl(uUrl);
                else
                    throw new ArgumentException("Url has to be string or Uri.", nameof(url));
            }
            finally
            {
                if (timeout.HasValue)
                    Driver.Manage().Timeouts().PageLoad = SeleniumTestContext.CurrentContext.Settings.Timeout;
            }

            Driver.SkipCertificateWarning();
        }

        /// <summary>
        /// Navigates to the a given URL.
        /// </summary>
        /// <param name="url">The URL to navigate to.</param>
        /// <param name="timeout">The timeout for the navigation action.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected virtual async Task GoToUrlAsyncImpl(object url, TimeSpan? timeout)
        {
            if (timeout.HasValue)
                Driver.Manage().Timeouts().PageLoad = timeout.Value;

            try
            {
                if (url == null)
                    await WrappedNavigation.GoToUrlAsync((string)null);
                else if (url is string sUrl)
                    await WrappedNavigation.GoToUrlAsync(sUrl);
                else if (url is Uri uUrl)
                    await WrappedNavigation.GoToUrlAsync(uUrl);
                else
                    throw new ArgumentException("Url has to be string or Uri.", nameof(url));
            }
            finally
            {
                if (timeout.HasValue)
                    Driver.Manage().Timeouts().PageLoad = SeleniumTestContext.CurrentContext.Settings.Timeout;
            }

            Driver.SkipCertificateWarning();
        }
    }
}
