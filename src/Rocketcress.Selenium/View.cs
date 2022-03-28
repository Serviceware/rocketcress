using Rocketcress.Core;
using Rocketcress.Core.Base;
using Rocketcress.Selenium.Extensions;

namespace Rocketcress.Selenium;

/// <summary>
/// Base class for a view.
/// </summary>
public abstract class View : TestObjectBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="View"/> class.
    /// </summary>
    /// <param name="driver">The driver to which this view is attached.</param>
    public View(WebDriver driver)
    {
        Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        Wait = CreateWaitEntry();
        InitializeControls();
    }

    /// <summary>
    /// Gets the driver to which the view is assigned to.
    /// </summary>
    public WebDriver Driver { get; }

    /// <summary>
    /// Gets the Location key that is used to uniquely identify the view.
    /// </summary>
    public abstract By RepresentedBy { get; }

    /// <summary>
    /// Gets a value indicating whether the view exists.
    /// </summary>
    public virtual bool Exists => Driver.IsElementExistent(RepresentedBy);

    /// <summary>
    /// Gets or sets the window handle of the views window.
    /// </summary>
    public virtual string WindowHandle { get; set; }

    /// <summary>
    /// Gets the wait entry point for this <see cref="WebElement"/>.
    /// </summary>
    public virtual WaitEntry Wait { get; }

    /// <summary>
    /// Waits until this view exists in the current window.
    /// </summary>
    /// <param name="assert">Determines wether to assert when the timeout has been reached.</param>
    /// <returns>Returns true if the view existed in time; otherwise false.</returns>
    [Obsolete("Use view.Wait.UntilExists.Start() instead. If assert is set to true add .ThrowOnFailure() before starting.")]
    public virtual bool WaitUntilExists(bool assert = true) => WaitUntilExists(Core.Wait.DefaultOptions.Timeout, assert);

    /// <summary>
    /// Waits until this view exists in the current window.
    /// </summary>
    /// <param name="timeout">The timeout in miliseconds for this waiting operation.</param>
    /// <param name="assert">Determines wether to assert when the timeout has been reached.</param>
    /// <returns>Returns true if the view existed in time; otherwise false.</returns>
    [Obsolete("Use view.Wait.UntilExists.WithTimeout(timeout).Start() instead. If assert is set to true add .ThrowOnFailure() before starting.")]
    public virtual bool WaitUntilExists(int timeout, bool assert = true) => WaitUntilExists(TimeSpan.FromMilliseconds(timeout), assert);

    /// <summary>
    /// Waits until this view exists in the current window.
    /// </summary>
    /// <param name="timeout">The timeout for this waiting operation.</param>
    /// <param name="assert">Determines wether to assert when the timeout has been reached.</param>
    /// <returns>Returns true if the view existed in time; otherwise false.</returns>
    [Obsolete("Use view.Wait.UntilExists.WithTimeout(timeout).Start() instead. If assert is set to true add .ThrowOnFailure() before starting.")]
    public virtual bool WaitUntilExists(TimeSpan timeout, bool assert = true)
    {
        return Wait.UntilExists.WithTimeout(timeout).OnFailure(assert).Start().Value;
    }

    /// <summary>
    /// Waits until this view has been closed.
    /// </summary>
    /// <param name="nextWindow">The window to switch to after closing.</param>
    /// <param name="timeout">The timeout in miliseconds for this waiting operation. If null the default timeout is used.</param>
    /// <param name="assert">Determines wether to assert when the timeout has been reached.</param>
    /// <returns><c>true</c> when the view closed; otherwise <c>false</c>.</returns>
    [Obsolete("Use view.Wait.UntilClosed(nextView).Start() instead. If timeout is set add .WithTimeout(timeout) before starting. If assert is set to true add .ThrowOnFailure() before starting.")]
    public virtual bool WaitForClose(View nextWindow, int? timeout = null, bool assert = true)
    {
        return Wait.UntilClosed(nextWindow).WithTimeout(timeout ?? Core.Wait.DefaultOptions.TimeoutMs).OnFailure(assert).Start().Value;
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
        if (Driver.Context.Driver != Driver)
            Driver.Context.SwitchCurrentDriver(Driver);
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

    /// <summary>
    /// Initializes all controls of this view.
    /// </summary>
    protected virtual void InitializeControls()
    {
    }

    /// <summary>
    /// Creates the wait entry used for this instance.
    /// </summary>
    /// <returns>The wait entry used for this instance.</returns>
    protected virtual WaitEntry CreateWaitEntry()
    {
        return new WaitEntry(this);
    }

    /// <summary>
    /// Wait entry for the <see cref="View"/> class.
    /// </summary>
    /// <seealso cref="Rocketcress.Core.WaitEntry" />
    public class WaitEntry : Core.WaitEntry
    {
        private readonly View _view;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitEntry"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public WaitEntry(View view)
        {
            _view = view;
        }

        /// <summary>
        /// Gets a wait operation that waits until this view exists.
        /// </summary>
        public virtual IWait<bool> UntilExists
            => Until(OnCheckExists).WithDefaultErrorMessage($"View could not be found. RepresentedBy: {_view.RepresentedBy}").PrecedeWith(OnUntilExistsStarting);

        /// <summary>
        /// Gets a wait operation that waits until this view closed and switches to another view after waiting.
        /// </summary>
        /// <param name="nextView">The next view.</param>
        /// <returns>The wait operation.</returns>
        public virtual IWait<bool> UntilClosed(View nextView)
        {
            return _view.Driver.Wait.UntilWindowHandleClosed(_view, nextView);
        }

        /// <summary>
        /// Called when a wait operation retrieved from <see cref="UntilExists"/> has been started.
        /// </summary>
        /// <param name="context">The context of the wait operation.</param>
        protected virtual void OnUntilExistsStarting(WaitContext context)
        {
            Guard.NotNull(context);
            _view.Driver.Wait.UntilPageLoaded.WithTimeout(context.Options.Timeout).OnFailure(context.ThrowOnFailure).Start();
            _view.Driver.SkipCertificateWarning();
        }

        /// <summary>
        /// Called when the <see cref="UntilExists"/> wait operations checks whether to continue waiting or not.
        /// </summary>
        /// <returns>When <c>true</c> is returned, the wait operation is completed; otherwisem, the waiting continues.</returns>
        protected virtual bool OnCheckExists() => _view.Driver.IsPageLoadComplete() && _view.Exists;
    }
}
