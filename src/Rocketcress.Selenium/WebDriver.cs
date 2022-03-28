using OpenQA.Selenium.Support.UI;
using Rocketcress.Core;
using Rocketcress.Selenium.Interactions;
using Rocketcress.Selenium.SeleniumWrappers;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Rocketcress.Selenium;

/// <summary>
/// Represents a driver that controls a browser.
/// </summary>
public class WebDriver : OpenQA.Selenium.Support.UI.IWait<WebDriver>, IWebDriver, IJavaScriptExecutor, ITakesScreenshot
{
    private readonly Type[] _allowedExceptionsGetWebDriverProp = new[]
    {
        typeof(WebDriverTimeoutException),
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="WebDriver"/> class.
    /// </summary>
    /// <param name="context">The current conotext.</param>
    /// <param name="driver">The driver to wrap.</param>
    /// <param name="timeout">The timeout to use.</param>
    /// <param name="language">The language for which the driver was created.</param>
    public WebDriver(SeleniumTestContext context, IWebDriver driver, TimeSpan timeout, CultureInfo language)
    {
        Context = Guard.NotNull(context);
        Driver = Guard.NotNull(driver);
        Language = Guard.NotNull(language);
        JavaScriptExecutor = (IJavaScriptExecutor)driver;
        WaitDriver = new DefaultWait<WebDriver>(this);
        WaitDriver.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException), typeof(WebDriverException));
        Timeout = timeout;
        KnownWindowHandles = new List<string>();
        Wait = CreateWaitEntry();
        if (!string.IsNullOrEmpty(driver.CurrentWindowHandle))
            KnownWindowHandles.Add(driver.CurrentWindowHandle);

        Core.Wait.DefaultOptions.PropertyChanged += WaitDefaultOptions_PropertyChanged;
    }

    /// <summary>
    /// Gets the context on which this driver has been created on.
    /// </summary>
    public SeleniumTestContext Context { get; }

    /// <summary>
    /// Gets the underlying driver.
    /// </summary>
    public IWebDriver Driver { get; private set; }

    /// <summary>
    /// Gets the language of the currently opened browser.
    /// </summary>
    public CultureInfo Language { get; }

    /// <summary>
    /// Gets the underlying javascript executor.
    /// </summary>
    public IJavaScriptExecutor JavaScriptExecutor { get; private set; }

    /// <summary>
    /// Gets or sets the underlying wait driver.
    /// </summary>
    public DefaultWait<WebDriver> WaitDriver { get; set; }

    /// <summary>
    /// Gets or sets a list of all the known window handles.
    /// </summary>
    public List<string> KnownWindowHandles { get; set; }

    /// <summary>
    /// Gets a value indicating whether the current window handle is open.
    /// </summary>
    public bool IsCurrentHandleOpen
    {
        get
        {
            try
            {
                return WindowHandles.Contains(CurrentWindowHandle);
            }
            catch (UnhandledAlertException)
            {
                return true; /* Danke IE für diese Ausnahme -.- */
            }
            catch (NoSuchWindowException)
            {
                return false;
            }
            catch (WebDriverException ex) when (ex.Message?.ToLower().Contains("timed out") == true)
            {
                return false;
            }
            catch (WebDriverException ex) when (ex.Message?.ToLower().Contains("null response") == true)
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Gets the wait entry point for this <see cref="WebDriver"/>.
    /// </summary>
    public virtual WaitEntry Wait { get; }

    /// <summary>
    /// Check wether the page has been loaded completely.
    /// </summary>
    /// <returns>Return true if the readyState of the document is complete; otherwise false.</returns>
    public bool IsPageLoadComplete()
    {
        try
        {
            return JavaScriptExecutor.ExecuteScript("return document.readyState").Equals("complete");
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Waits until the current page is loaded.
    /// </summary>
    /// <param name="assert">Determines wether to assert if the timeout has been reached.</param>
    /// <returns>Returns true if the page has loaded; otherwise false.</returns>
    [Obsolete("Use driver.Wait.UntilPageLoaded.Start() instead. If assert is set to true add .ThrowOnFailure() before starting.")]
    public bool UntilPageLoaded(bool assert = true) => UntilPageLoaded(Core.Wait.DefaultOptions.Timeout, assert);

    /// <summary>
    /// Waits until the current page is loaded.
    /// </summary>
    /// <param name="timeout">The timeout in miliseconds for this wait operation.</param>
    /// <param name="assert">Determines wether to assert if the timeout has been reached.</param>
    /// <returns>Returns true if the page has loaded; otherwise false.</returns>
    [Obsolete("Use driver.Wait.UntilPageLoaded.WithTimeout(timeout).Start() instead. If assert is set to true add .ThrowOnFailure() before starting.")]
    public bool UntilPageLoaded(int timeout, bool assert = true) => UntilPageLoaded(TimeSpan.FromMilliseconds(timeout), assert);

    /// <summary>
    /// Waits until the current page is loaded.
    /// </summary>
    /// <param name="timeout">The timeout for this wait operation.</param>
    /// <param name="assert">Determines wether to assert if the timeout has been reached.</param>
    /// <returns>Returns true if the page has loaded; otherwise false.</returns>
    [Obsolete("Use driver.Wait.UntilPageLoaded.WithTimeout(timeout).Start() instead. If assert is set to true add .ThrowOnFailure() before starting.")]
    public bool UntilPageLoaded(TimeSpan timeout, bool assert = true)
    {
        return Wait.UntilPageLoaded.WithTimeout(timeout).OnFailure(assert).Start().Value;
    }

    /// <summary>
    /// Waits until a specific amount of windows are open in the current browser instance.
    /// </summary>
    /// <param name="count">The expected number of open windows.</param>
    /// <returns>Return true if the amount of windows did exist in time; otherwise false.</returns>
    [Obsolete("Use driver.Wait.UntilWindowsExists(count).Start() instead.")]
    public bool UntilWindowsExists(int count) => UntilWindowsExists(count, Core.Wait.DefaultOptions.TimeoutMs);

    /// <summary>
    /// Waits until a specific amount of windows are open in the current browser instance.
    /// </summary>
    /// <param name="count">The expected number of open windows.</param>
    /// <param name="timeout">The timeout in miliseconds for this wait operation.</param>
    /// <returns>Return true if the amount of windows did exist in time; otherwise false.</returns>
    [Obsolete("Use driver.Wait.UntilWindowsExists(count).WithTimeout(timeout).Start() instead.")]
    public bool UntilWindowsExists(int count, int timeout)
    {
        return Wait.UntilWindowsExists(count).WithTimeout(timeout).ThrowOnFailure().Start().Value;
    }

    /// <summary>
    /// Closes all windows that are unknown.
    /// </summary>
    public void CloseAllUnknownWindows()
    {
        var unknownHandles = WindowHandles.Except(KnownWindowHandles);
        var prevHandle = CurrentWindowHandle;
        foreach (var handle in unknownHandles)
        {
            SwitchTo().Window(handle);
            if (GetBrowser() == Browser.InternetExplorer)
                TestHelper.Try(() => SwitchTo().Alert().Dismiss());
            Close();
        }

        if (unknownHandles.Any())
            SwitchTo().Window(prevHandle);
    }

    /// <summary>
    /// Waits until a new unknown window was found that matches a given View and switches to it.
    /// </summary>
    /// <param name="browserWindow">The expected view.</param>
    /// <param name="timeout">The timeout in miliseconds for this wait operation. If null the default timeout is used.</param>
    /// <param name="closeCurrent">Determines wether to close the current window before switching.</param>
    /// <param name="assert">Determines wether to assert if the timeout has been reached.</param>
    /// <returns>Returns true if the windows was found in time; otherwise false.</returns>
    public bool WaitAndSwitchToWindow(View browserWindow, int? timeout = null, bool closeCurrent = false, bool assert = true)
    {
        Guard.NotNull(browserWindow);
        if (!string.IsNullOrEmpty(browserWindow.WindowHandle))
            return true;
        int activeTimeout = timeout ?? (int)Timeout.TotalMilliseconds;
        if (closeCurrent)
            Close();
        if (!Wait.Until(() => WindowHandles.Except(KnownWindowHandles).Any()).WithTimeout(activeTimeout).OnFailure(assert, "No new window was found.").Start().Value)
            return false;
        var unknwonHandle = WindowHandles.Except(KnownWindowHandles).First();
        if (browserWindow != null)
        {
            browserWindow.WindowHandle = unknwonHandle;
            KnownWindowHandles.Add(unknwonHandle);
        }

        if (Driver.CurrentWindowHandle != unknwonHandle)
            SwitchTo(browserWindow);
        return browserWindow.Wait.UntilExists.WithTimeout(activeTimeout).OnFailure(assert).Start().Value;
    }

    /// <summary>
    /// Waits until a view has closed and switches to another one.
    /// </summary>
    /// <param name="view">The view expected to close.</param>
    /// <param name="nextView">The view to switch to after closing.</param>
    /// <param name="timeout">The timeout in miliseconds for this wait operation. If null the default timeout is used.</param>
    /// <param name="assert">Determines wether to assert when the timeout has been reached.</param>
    /// <returns><c>true</c> when the handle has been closed; otherwise <c>false</c>.</returns>
    [Obsolete("Use driver.Wait.UntilWindowHandleClosed(view, nextView).Start().Value instead. If time is specified add .WithTimeout(timeout). If assert is omitted or set to true, add .ThrowOnFailure().")]
    public bool WaitForHandleToClose(View view, View nextView, int? timeout = null, bool assert = true)
    {
        return Wait.UntilWindowHandleClosed(view, nextView).WithTimeout(timeout ?? Core.Wait.DefaultOptions.TimeoutMs).OnFailure(assert).Start().Value;
    }

    /// <summary>
    /// Closes a view, waits for it to close and switches to another one.
    /// </summary>
    /// <param name="currentWindow">The view to close.</param>
    /// <param name="nextView">The view to switch to after closing.</param>
    /// <param name="timeout">The timeout in miliseconds for this wait operation. If null the default timeout is used.</param>
    public void CloseAndSwitchToWindow(View currentWindow, View nextView, int? timeout = null)
    {
        Close(currentWindow);
        SwitchTo(nextView);
        Wait.UntilPageLoaded.WithTimeout(timeout ?? (int)Timeout.TotalMilliseconds).ThrowOnFailure().Start();
    }

    /// <summary>
    /// Switches to another window.
    /// </summary>
    /// <param name="window">The index of the target window.</param>
    public void SwitchTo(int window)
    {
        Driver.SwitchTo().Window(Driver.WindowHandles[window]);
    }

    /// <summary>
    /// Switches to another window.
    /// </summary>
    /// <param name="browserWindow">The target view.</param>
    public void SwitchTo(View browserWindow)
    {
        Guard.NotNull(browserWindow);
        Driver.SwitchTo().Window(browserWindow.WindowHandle);
    }

    /// <summary>
    /// Removes a given handle from the knwon window handles.
    /// </summary>
    /// <param name="handle">The handle to remove.</param>
    public void RemoveKnownHandle(string handle)
    {
        if (KnownWindowHandles.Contains(handle))
            KnownWindowHandles.Remove(handle);
    }

    /// <summary>
    /// Checks if all KnownWindowHandles are still open. All closed handles will be removed from the list.
    /// </summary>
    public void RefreshKnownHandles()
    {
        var allHandles = WindowHandles;
        KnownWindowHandles = KnownWindowHandles.Where(x => allHandles.Contains(x)).ToList();
    }

    /// <summary>
    /// Executes an action and returnes an alert if one has popped up.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <param name="alertExpected">Determines wether an alert is expected. If true, the method will wait for an alert to pop up.</param>
    /// <param name="autoDismiss">Determines wether to automatically dismiss the alert.</param>
    /// <returns>Returns an alert if one has popped up; otherwise null.</returns>
    public IAlert GetAlertFromAction(Action action, bool alertExpected, bool autoDismiss)
    {
        Guard.NotNull(action);
        try
        {
            action();
        }
        catch (UnhandledAlertException)
        {
        }

        return GetAlert(alertExpected, autoDismiss);
    }

    /// <summary>
    /// Get an open alert if one is displayed.
    /// </summary>
    /// <param name="alertExpected">Determines wether an alert is expected. If true, the method will wait for an alert to pop up.</param>
    /// <param name="autoDismiss">Determines wether to automatically dismiss the alert.</param>
    /// <returns>Returns an alert if one was displayed; otherwise null.</returns>
    public IAlert GetAlert(bool alertExpected, bool autoDismiss)
    {
        IAlert GetAlert()
        {
            try
            {
                return SwitchTo().Alert();
            }
            catch (NoAlertPresentException)
            {
                return null;
            }
        }

        IAlert alert;
        if (alertExpected)
            alert = IsCurrentHandleOpen ? Wait.Until(GetAlert).Start().Value : null;
        else
            alert = IsCurrentHandleOpen ? GetAlert() : null;

        if (alert != null)
        {
            try
            {
                Logger.LogDebug("Alert appeared with text: {0}", alert.Text);
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Alert appeared but text could not be retrieved: " + ex.Message);
            }

            if (autoDismiss)
                alert.Dismiss();
        }

        return alert;
    }

    /// <summary>
    /// Creates a new Action object that is used to execute a bunch of actions on the browser.
    /// </summary>
    /// <returns>Returns an instance of the ActionsEx class that is mapped to this driver.</returns>
    public ActionsEx GetActions()
    {
        return new ActionsEx(Driver);
    }

    /// <summary>
    /// Skips an open certificate warning if one exists.
    /// </summary>
    public void SkipCertificateWarning()
    {
        bool waitForPageLoad = false;
        if (Driver is OpenQA.Selenium.IE.InternetExplorerDriver)
        {
            ExecuteScript("try {document.getElementById('overridelink').click();} catch (err) {console.log('probably cert warn already accepted');}");
            waitForPageLoad = true;
        }

        if (waitForPageLoad)
            Wait.UntilPageLoaded.ThrowOnFailure().Start();
    }

    /// <summary>
    /// Determines the browser that the current driver is controlling.
    /// </summary>
    /// <returns>Returns the browser.</returns>
    public Browser GetBrowser()
    {
        if (Driver is OpenQA.Selenium.Chrome.ChromeDriver)
            return Browser.Chrome;
        if (Driver is OpenQA.Selenium.Edge.EdgeDriver)
            return Browser.Edge;
        if (Driver is OpenQA.Selenium.Firefox.FirefoxDriver)
            return Browser.Firefox;
        if (Driver is OpenQA.Selenium.IE.InternetExplorerDriver)
            return Browser.InternetExplorer;
        return Browser.Unknown;
    }

    /// <summary>
    /// Creates the wait entry used for this instance.
    /// </summary>
    /// <returns>The wait entry used for this instance.</returns>
    protected virtual WaitEntry CreateWaitEntry()
    {
        return new WaitEntry(this);
    }

    private void WaitDefaultOptions_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IWaitOptions.Timeout))
            WaitDriver.Timeout = Core.Wait.DefaultOptions.Timeout;
    }

    #region IWait<WebDriver> Members

    /// <summary>
    /// Gets or sets how long to wait for the evaluated condition to be true.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Interface implementation region")]
    public TimeSpan Timeout
    {
        get => WaitDriver.Timeout;
        set
        {
            Core.Wait.DefaultOptions.Timeout = value;
            WaitDriver.Timeout = value;
        }
    }

    /// <summary>
    /// Gets or sets how often the condition should be evaluated.
    /// </summary>
    public TimeSpan PollingInterval
    {
        get => WaitDriver.PollingInterval;
        set
        {
            Core.Wait.DefaultOptions.TimeGap = value;
            WaitDriver.PollingInterval = value;
        }
    }

    /// <summary>
    /// Gets or sets the message to be displayed when time expires.
    /// </summary>
    public string Message
    {
        get => WaitDriver.Message;
        set => WaitDriver.Message = value;
    }

    /// <summary>
    /// Configures this instance to ignore specific types of exceptions while waiting for a condition. Any exceptions not whitelisted will be allowed to propagate, terminating the wait.
    /// </summary>
    /// <param name="exceptionTypes">The types of exceptions to ignore.</param>
    public void IgnoreExceptionTypes(params Type[] exceptionTypes)
    {
        WaitDriver.IgnoreExceptionTypes(exceptionTypes);
    }

    /// <summary>
    /// Waits until a condition is true or times out.
    /// </summary>
    /// <typeparam name="TResult">The type of result to expect from the condition.</typeparam>
    /// <param name="condition">A delegate taking a TSource as its parameter, and returning a TResult.</param>
    /// <returns>If TResult is a boolean, the method returns true when the condition is true, and false otherwise. If TResult is an object, the method returns the object when the condition evaluates to a value other than null.</returns>
    TResult OpenQA.Selenium.Support.UI.IWait<WebDriver>.Until<TResult>(Func<WebDriver, TResult> condition)
    {
        return Wait.Until(() => condition(this)).WithTimeout(Timeout).WithTimeGap(PollingInterval).ThrowOnFailure().Start().Value;
    }

    #endregion

    #region ITakesScreenshot Member

    /// <summary>
    /// Gets a OpenQA.Selenium.Screenshot object representing the image of the page on the screen.
    /// </summary>
    /// <returns>A OpenQA.Selenium.Screenshot object containing the image.</returns>
    public Screenshot GetScreenshot()
    {
        return ((ITakesScreenshot)Driver).GetScreenshot();
    }

    #endregion

    #region IJavaScriptExecutor Member

    /// <summary>
    /// Executes JavaScript asynchronously in the context of the currently selected frame or window.
    /// </summary>
    /// <param name="script">The JavaScript code to execute.</param>
    /// <param name="args">The arguments to the script.</param>
    /// <returns>The value returned by the script.</returns>
    public object ExecuteScript(string script, params object[] args)
    {
        return JavaScriptExecutor.ExecuteScript(script, args);
    }

    /// <summary>
    /// Executes JavaScript asynchronously in the context of the currently selected frame or window.
    /// </summary>
    /// <param name="script">The JavaScript code to execute.</param>
    /// <param name="args">The arguments to the script.</param>
    /// <returns>The value returned by the script.</returns>
    public object ExecuteScript(PinnedScript script, params object[] args)
    {
        return JavaScriptExecutor.ExecuteScript(script, args);
    }

    /// <summary>
    /// Executes JavaScript in the context of the currently selected frame or window.
    /// </summary>
    /// <param name="script">The JavaScript code to execute.</param>
    /// <param name="args">The arguments to the script.</param>
    /// <returns>The value returned by the script.</returns>
    public object ExecuteAsyncScript(string script, params object[] args)
    {
        return JavaScriptExecutor.ExecuteAsyncScript(script, args);
    }

    #endregion

    #region IWebDriver Member

    /// <summary>
    /// Gets the current window handle, which is an opaque handle to this window that uniquely identifies it within this driver instance.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Interface implementation region")]
    public string CurrentWindowHandle => GetWebDriverProperty(x => x.CurrentWindowHandle, false, null);

    /// <summary>
    /// Gets the source of the page last loaded by the browser.
    /// </summary>
    public string PageSource => Driver.PageSource;

    /// <summary>
    /// Gets the title of the current browser window.
    /// </summary>
    public string Title => Driver.Title;

    /// <summary>
    /// Gets or sets the URL the browser is currently displaying.
    /// </summary>
    public string Url
    {
        get => Driver.Url;
        set => Driver.Url = value;
    }

    /// <summary>
    /// Gets the window handles of open browser windows.
    /// </summary>
    public ReadOnlyCollection<string> WindowHandles => GetWebDriverProperty(x => x.WindowHandles, true);

    /// <summary>
    /// Close the current window, quitting the browser if it is the last window currently open.
    /// </summary>
    public void Close()
    {
        RemoveKnownHandle(CurrentWindowHandle);
        Driver.Close();
    }

    /// <summary>
    /// Close a window, quitting the browser if it is the last window currently open.
    /// </summary>
    /// <param name="handle">The handle of the window to close.</param>
    public void Close(string handle)
    {
        if (!string.IsNullOrEmpty(handle) && WindowHandles.Contains(handle))
        {
            RemoveKnownHandle(handle);
            if (CurrentWindowHandle != handle)
                SwitchTo().Window(handle);
            Close();
        }
    }

    /// <summary>
    /// Close a window, quitting the browser if it is the last window currently open.
    /// </summary>
    /// <param name="browserView">The window to close.</param>
    public void Close(View browserView)
    {
        if (browserView != null)
        {
            Close(browserView.WindowHandle);
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <param name="disposing">Determines wether this method was executed by the <see cref="IDisposable.Dispose"/> method.</param>
    public virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Driver.Dispose();
            Core.Wait.DefaultOptions.PropertyChanged -= WaitDefaultOptions_PropertyChanged;
        }
    }

    /// <summary>
    /// Finds the first OpenQA.Selenium.IWebElement using the given method.
    /// </summary>
    /// <param name="by">The locating mechanism to use.</param>
    /// <returns>The first matching OpenQA.Selenium.IWebElement on the current context.</returns>
    public IWebElement FindElement(OpenQA.Selenium.By by)
    {
        return Driver.FindElement(by);
    }

    /// <summary>
    /// Finds all IWebElements within the current context using the given mechanism.
    /// </summary>
    /// <param name="by">The locating mechanism to use.</param>
    /// <returns>A System.Collections.ObjectModel.ReadOnlyCollection`1 of all WebElements matching the current criteria, or an empty list if nothing matches.</returns>
    public ReadOnlyCollection<IWebElement> FindElements(OpenQA.Selenium.By by)
    {
        try
        {
            return Driver.FindElements(by);
        }
        catch (Exception)
        {
            return new ReadOnlyCollection<IWebElement>(Array.Empty<IWebElement>());
        }
    }

    /// <summary>
    /// Instructs the driver to change its settings.
    /// </summary>
    /// <returns>An OpenQA.Selenium.IOptions object allowing the user to change the settings of the driver.</returns>
    public IOptions Manage()
    {
        return Driver.Manage();
    }

    /// <inheritdoc/>
    INavigation IWebDriver.Navigate() => Navigate();

    /// <summary>
    /// Instructs the driver to navigate the browser to another location.
    /// </summary>
    /// <returns>An OpenQA.Selenium.INavigation object allowing the user to access the browser's history and to navigate to a given URL.</returns>
    public NavigationWrapper Navigate()
    {
        return new NavigationWrapper(this, Driver.Navigate());
    }

    /// <summary>
    /// Quits this driver, closing every associated window.
    /// </summary>
    public void Quit()
    {
        Driver.Quit();
    }

    /// <summary>
    /// Refreshes the page.
    /// </summary>
    public void RefreshPage()
    {
        Driver.Navigate().GoToUrl(Driver.Url);
    }

    /// <summary>
    /// Instructs the driver to send future commands to a different frame or window.
    /// </summary>
    /// <returns>An OpenQA.Selenium.ITargetLocator object which can be used to select a frame or window.</returns>
    public ITargetLocator SwitchTo()
    {
        return Driver.SwitchTo();
    }

    #endregion

    private T GetWebDriverProperty<T>(Func<IWebDriver, T> propertyFunction, bool throwEx, T returnOnError = default)
    {
        T result = default;
        Exception lastException = null;

        void OnException(Exception ex)
        {
            lastException = ex;
            if (!_allowedExceptionsGetWebDriverProp.Contains(ex.GetType()))
                throw new WaitAbortedException();
        }

        var retryResult = Retry
            .Action(() => result = propertyFunction(Driver))
            .WithMaxRetryCount(5)
            .WithTimeGap(1000)
            .OnError().Call(OnException)
            .Start();
        if (!retryResult.Value)
        {
            if (throwEx && lastException != null)
                throw lastException;
            result = returnOnError;
        }

        return result;
    }

    /// <summary>
    /// Wait entry for the <see cref="WebDriver"/> class.
    /// </summary>
    public class WaitEntry : Core.WaitEntry
    {
        private readonly WebDriver _driver;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitEntry"/> class.
        /// </summary>
        /// <param name="driver">The driver.</param>
        public WaitEntry(WebDriver driver)
        {
            _driver = driver;
        }

        /// <summary>
        /// Gets a wait operation that waits until the web page has been loaded.
        /// </summary>
        public Core.IWait<bool> UntilPageLoaded => Until(OnCheckPageLoaded).WithDefaultErrorMessage("Page has not been loaded.");

        /// <summary>
        /// Gets a wait operation that waits until a specified amount of windows exist.
        /// </summary>
        /// <param name="count">The expected window count.</param>
        /// <returns>The wait operation.</returns>
        public Core.IWait<bool> UntilWindowsExists(int count)
        {
            return Until(() => _driver.WindowHandles.Count == count).WithDefaultErrorMessage($"{count} window(s) were not present in time.");
        }

        /// <summary>
        /// Gets a wait operation that waits until a specified windows handle is closed that switches to another one after waiting.
        /// </summary>
        /// <param name="view">The view expected to be closed.</param>
        /// <param name="nextView">The next view to switch to.</param>
        /// <returns>The wait operation.</returns>
        public Core.IWait<bool> UntilWindowHandleClosed(View view, View nextView)
        {
            return Until(
                () =>
                {
                    try
                    {
                        return !_driver.WindowHandles.Contains(view.WindowHandle);
                    }
                    catch (WebDriverException ex) when (ex.Message?.ToLower().Contains("timed out") == true)
                    {
                        return true;
                    }
                })
                .WithDefaultErrorMessage("Windows handle was not closed.")
                .ContinueWith(ctx =>
                {
                    if (ctx.Result.Value)
                    {
                        _driver.RemoveKnownHandle(view.WindowHandle);
                        nextView.SetFocus();
                    }
                });
        }

        /// <summary>
        /// Called when the <see cref="UntilPageLoaded"/> wait operations checks whether to continue waiting or not.
        /// </summary>
        /// <returns>When <c>true</c> is returned, the wait operation is completed; otherwisem, the waiting continues.</returns>
        protected virtual bool OnCheckPageLoaded() => _driver.IsPageLoadComplete();
    }
}
