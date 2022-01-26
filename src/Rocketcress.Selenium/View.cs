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
    /// Gets a wait operation that is not started and waits until this view exists.
    /// </summary>
    public virtual IWait<bool> UntilExists => Wait
        .Until(() => Driver.IsPageLoadComplete() && Exists)
        .WithDefaultErrorMessage($"View could not be found. RepresentedBy: {RepresentedBy}")
        .PrecedeWith(ctx =>
        {
            Driver.UntilPageLoaded.WithTimeout(ctx.Options.Timeout).OnFailure(ctx.ThrowOnFailure).Start();
            Driver.SkipCertificateWarning();
        });

    /// <summary>
    /// Waits until this view exists in the current window.
    /// </summary>
    /// <param name="assert">Determines wether to assert when the timeout has been reached.</param>
    /// <returns>Returns true if the view existed in time; otherwise false.</returns>
    [Obsolete("Use UntilExists.Start() instead. If assert is set to true add .ThrowOnFailure() before starting.")]
    public virtual bool WaitUntilExists(bool assert = true) => WaitUntilExists(Wait.DefaultOptions.Timeout, assert);

    /// <summary>
    /// Waits until this view exists in the current window.
    /// </summary>
    /// <param name="timeout">The timeout in miliseconds for this waiting operation.</param>
    /// <param name="assert">Determines wether to assert when the timeout has been reached.</param>
    /// <returns>Returns true if the view existed in time; otherwise false.</returns>
    [Obsolete("Use UntilExists.WithTimeout(timeout).Start() instead. If assert is set to true add .ThrowOnFailure() before starting.")]
    public virtual bool WaitUntilExists(int timeout, bool assert = true) => WaitUntilExists(TimeSpan.FromMilliseconds(timeout), assert);

    /// <summary>
    /// Waits until this view exists in the current window.
    /// </summary>
    /// <param name="timeout">The timeout for this waiting operation.</param>
    /// <param name="assert">Determines wether to assert when the timeout has been reached.</param>
    /// <returns>Returns true if the view existed in time; otherwise false.</returns>
    [Obsolete("Use UntilExists.WithTimeout(timeout).Start() instead. If assert is set to true add .ThrowOnFailure() before starting.")]
    public virtual bool WaitUntilExists(TimeSpan timeout, bool assert = true)
    {
        Wait.Until(() => Driver.IsPageLoadComplete()).WithTimeout(timeout).OnFailure(assert).Start();
        Driver.SkipCertificateWarning();
        return Wait.Until(() => Driver.IsPageLoadComplete() && Exists).WithTimeout(timeout).OnFailure(assert).Start().Value;
    }

    /// <summary>
    /// Waits until this view has been closed.
    /// </summary>
    /// <param name="nextWindow">The window to switch to after closing.</param>
    /// <param name="timeout">The timeout in miliseconds for this waiting operation. If null the default timeout is used.</param>
    /// <param name="assert">Determines wether to assert when the timeout has been reached.</param>
    /// <returns><c>true</c> when the view closed; otherwise <c>false</c>.</returns>
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
}
