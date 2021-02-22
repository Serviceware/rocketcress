using Rocketcress.Selenium.Extensions;
using Rocketcress.Core;
using Rocketcress.Core.Base;
using OpenQA.Selenium;
using System;

namespace Rocketcress.Selenium
{
    /// <summary>
    /// Base class for a view.
    /// </summary>
    public abstract class View : TestObjectBase
    {
        #region Properties
        /// <summary>
        /// The driver to which the view is assigned to.
        /// </summary>
        public WebDriver Driver { get; private set; }

        /// <summary>
        /// Location key that is used to uniquely identify the view.
        /// </summary>
        public abstract By RepresentedBy { get; }

        /// <summary>
        /// Determines if the view exists.
        /// </summary>
        public virtual bool Exists => Driver.IsElementExistent(RepresentedBy);

        /// <summary>
        /// Gets the window handle of the views window.
        /// </summary>
        public virtual string WindowHandle { get; set; }
        #endregion

        #region Initialization
        /// <summary>
        /// Initialized a new instance of the View class.
        /// </summary>
        public View() : this(SeleniumTestContext.CurrentContext.Driver) { }

        /// <summary>
        /// Initialized a new instance of the View class.
        /// </summary>
        /// <param name="driver"></param>
        public View(WebDriver driver) { Driver = driver; InitializeControls(); }
        

        /// <summary>
        /// Initializes all controls of this view.
        /// </summary>
        protected virtual void InitializeControls() { }
        #endregion

        /// <summary>
        /// Waits until this view exists in the current window.
        /// </summary>
        /// <param name="assert">Determines wether to assert when the timeout has been reached.</param>
        /// <returns>Returns true if the view existed in time; otherwise false.</returns>
        public virtual bool WaitUntilExists(bool assert = true) => WaitUntilExists(Waiter.DefaultTimeout, assert);

        /// <summary>
        /// Waits until this view exists in the current window.
        /// </summary>
        /// <param name="timeout">The timeout in miliseconds for this waiting operation.</param>
        /// <param name="assert">Determines wether to assert when the timeout has been reached.</param>
        /// <returns>Returns true if the view existed in time; otherwise false.</returns>
        public virtual bool WaitUntilExists(int timeout, bool assert = true) => WaitUntilExists(TimeSpan.FromMilliseconds(timeout), assert);

        /// <summary>
        /// Waits until this view exists in the current window.
        /// </summary>
        /// <param name="timeout">The timeout for this waiting operation.</param>
        /// <param name="assert">Determines wether to assert when the timeout has been reached.</param>
        /// <returns>Returns true if the view existed in time; otherwise false.</returns>
        public virtual bool WaitUntilExists(TimeSpan timeout, bool assert = true)
        {
            Waiter.WaitUntil(() => Driver.IsPageLoadComplete(), timeout, assert);
            Driver.SkipCertificateWarning();
            return Waiter.WaitUntil(() => Driver.IsPageLoadComplete() && Exists, timeout, assert);
        }

        /// <summary>
        /// Waits until this view has been closed.
        /// </summary>
        /// <param name="nextWindow">The window to switch to after closing.</param>
        /// <param name="timeout">The timeout in miliseconds for this waiting operation. If null the default timeout is used.</param>
        /// <param name="assert">Determines wether to assert when the timeout has been reached.</param>
        public virtual bool WaitForClose(View nextWindow, int? timeout = null, bool assert = true)
        {
            return Driver.WaitForHandleToClose(this, nextWindow, timeout, assert);
        }

        /// <summary>
        /// Closes the view and switches to another.
        /// </summary>
        /// <param name="nextWindow">The window to switch to after closing.</param>
        /// <param name="timeout">The timeout in miliseconds for this waiting operation. If null the default timeout is used.</param>
        public virtual void Close(View nextWindow, int? timeout = null)
        {
            Driver.CloseAndSwitchToWindow(this, nextWindow, timeout);
        }

        /// <summary>
        /// Waits until a new unknown window was found that matches the view and switches to it.
        /// </summary>
        /// <param name="timeout">The timeout in miliseconds for this waiting operation. If null the default timeout is used.</param>
        /// <param name="assert">Determines wether to assert when the timeout has been reached.</param>
        /// <returns>Returns true if the view existed in time; otherwise false.</returns>
        public virtual bool WaitForOpen(int? timeout = null, bool assert = true)
        {
            return Driver.WaitAndSwitchToWindow(this, timeout, assert: assert);
        }

        /// <summary>
        /// Switches to the views window if neccessary and sets the window as the foreground window.
        /// </summary>
        public void SetFocus()
        {
            var context = SeleniumTestContext.CurrentContext;
            if (context.Driver != Driver)
                context.SwitchCurrentDriver(Driver);
            Driver.SwitchTo(this);
            SetWindowToForeground();
        }

        /// <summary>
        /// Sets the windows of this view as the foreground window.
        /// </summary>
        public void SetWindowToForeground()
        {
            Driver.ExecuteScript("window.focus();");
        }
    }
}
