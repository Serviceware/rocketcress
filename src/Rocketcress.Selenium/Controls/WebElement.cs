using Rocketcress.Core;
using Rocketcress.Core.Base;
using Rocketcress.Core.Utilities;
using Rocketcress.Selenium.Base;
using Rocketcress.Selenium.Extensions;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Runtime.InteropServices;
using SeleniumBy = OpenQA.Selenium.By;

namespace Rocketcress.Selenium.Controls;

/// <summary>
/// Represents an element on a web page controlled by selenium.
/// </summary>
public class WebElement : WebElementContainer, IWebElement, IWrapsElement
{
    private IWebElement _wrappedElement;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebElement"/> class as lazy element.
    /// </summary>
    /// <param name="driver">The driver to which this element is attached.</param>
    /// <param name="locationKey">The location key.</param>
    public WebElement(WebDriver driver, SeleniumBy locationKey)
        : this(driver, locationKey, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebElement"/> class as lazy element.
    /// </summary>
    /// <param name="driver">The driver to which this element is attached.</param>
    /// <param name="locationKey">The location key.</param>
    /// <param name="searchContext">The search context.</param>
    public WebElement(WebDriver driver, SeleniumBy locationKey, ISearchContext searchContext)
        : this(driver)
    {
        if (searchContext is not null)
            SearchContext = searchContext;
        LocationKey = locationKey;
        Initialize();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebElement"/> class as non-lazy element.
    /// </summary>
    /// <param name="driver">The driver to which this element is attached.</param>
    /// <param name="element">The wrapped element.</param>
    public WebElement(WebDriver driver, IWebElement element)
        : this(driver)
    {
        WrappedElement = element;
        Initialize();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebElement"/> class.
    /// </summary>
    /// <param name="driver">The driver to which this element is attached.</param>
    protected WebElement(WebDriver driver)
        : base(false)
    {
        Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        Wait = CreateWaitEntry();
        SearchContext = Driver;
        ComputedStyle = new Style(this);
    }

    /// <summary>
    /// Event that is fired when the wrapped native selenium element has changed.
    /// </summary>
    public event EventHandler<IWebElement> WrappedElementChanged;

    /// <summary>
    /// Gets or sets the <see cref="T:OpenQA.Selenium.IWebElement" /> wrapped by this object.
    /// </summary>
    public virtual IWebElement WrappedElement
    {
        get
        {
            if (IsLazy && IsStale(_wrappedElement))
            {
                _wrappedElement = Wait.Until(() => SearchContext.FindElement(LocationKey)).Start().Value;
                if (_wrappedElement == null)
                    throw new NotFoundException("The requested element was not found: " + Environment.NewLine + "    " + GetSearchDescription(true));
                WrappedElementChanged?.Invoke(this, _wrappedElement);
            }

            return _wrappedElement;
        }
        protected set
        {
            _wrappedElement = value;
            LocationKey = null;
            SearchContext = null;
            WrappedElementChanged?.Invoke(this, _wrappedElement);
        }
    }

    /// <summary>
    /// Gets or sets the <see cref="ISearchContext"/> for this element. Set null to use the current driver.
    /// </summary>
    public ISearchContext SearchContext { get; protected set; }

    /// <summary>
    /// Gets the driver that is used to find this <see cref="WebElement"/>.
    /// </summary>
    public WebDriver Driver { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is lazy.
    /// </summary>
    public virtual bool IsLazy => LocationKey != null;

    /// <summary>
    /// Gets a value indicating whether the wrapped element was loaded.
    /// </summary>
    public virtual bool IsWrappedElementLoaded => _wrappedElement != null;

    /// <summary>
    /// Gets a value indicating whether the current element exists on the current page.
    /// </summary>
    public virtual bool Exists
    {
        get
        {
            try
            {
                if (SearchContext is WebElement element && !element.Exists)
                    return false;
                if (SearchContext is WebDriver driver && !driver.IsCurrentHandleOpen)
                    return false;
                if (IsLazy)
                {
                    _wrappedElement = SearchContext.TryFindElement(LocationKey);
                    WrappedElementChanged?.Invoke(this, _wrappedElement);
                }

                return !IsStale(_wrappedElement);
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Gets the value of the id-attribute.
    /// </summary>
    public virtual string Id => GetAttribute("id");

    /// <summary>
    /// Gets the value of the class-attribute.
    /// </summary>
    public virtual string Class => GetAttribute("class");

    /// <summary>
    /// Gets the value of the name-attribute.
    /// </summary>
    public virtual string Name => GetAttribute("name");

    /// <summary>
    /// Gets the innerText of this element, without any leading or trailing whitespace,
    /// and with other whitespace collapsed.
    /// </summary>
    public virtual string InnerText => Text;

    /// <summary>
    /// Gets the style of the wrapped element.
    /// </summary>
    public virtual Style Style => new(GetAttribute("style"));

    /// <summary>
    /// Gets the computed style object of the wrapped element.
    /// </summary>
    public Style ComputedStyle { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the control is currently clickable.
    /// </summary>
    public bool IsClickable
    {
        get
        {
            try
            {
                return WrappedElement != null && WrappedElement.Displayed && WrappedElement.Enabled;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Gets or sets the location key that is used to search for the control on the UI.
    /// </summary>
    public virtual SeleniumBy LocationKey { get; protected set; }

    /// <summary>
    /// Gets the wait entry point for this <see cref="WebElement"/>.
    /// </summary>
    public virtual WaitEntry Wait { get; }

    /// <summary>
    /// Gets a value indicating whether or not this element is displayed.
    /// </summary>
    /// <remarks>
    /// The <see cref="P:OpenQA.Selenium.IWebElement.Displayed" /> property avoids the problem
    /// of having to parse an element's "style" attribute to determine
    /// visibility of an element.
    /// </remarks>
    public virtual bool Displayed => Exists && _wrappedElement.Displayed;

    /// <summary>
    /// Gets a value indicating whether or not this element is enabled.
    /// </summary>
    /// <remarks>
    /// The <see cref="P:OpenQA.Selenium.IWebElement.Enabled" /> property will generally
    /// return <see langword="true" /> for everything except explicitly disabled input elements.
    /// </remarks>
    public virtual bool Enabled => WrappedElement.Enabled;

    /// <summary>
    /// Gets a <see cref="T:System.Drawing.Point" /> object containing the coordinates of the upper-left corner
    /// of this element relative to the upper-left corner of the page.
    /// </summary>
    public virtual Point Location => WrappedElement.Location;

    /// <summary>
    /// Gets a value indicating whether or not this element is selected.
    /// </summary>
    /// <remarks>
    /// This operation only applies to input elements such as checkboxes,
    /// options in a select element and radio buttons.
    /// </remarks>
    public virtual bool Selected => WrappedElement.Selected;

    /// <summary>
    /// Gets a <see cref="P:OpenQA.Selenium.IWebElement.Size" /> object containing the height and width of this element.
    /// </summary>
    public virtual Size Size => WrappedElement.Size;

    /// <summary>
    /// Gets the tag name of this element.
    /// </summary>
    /// <remarks>
    /// The <see cref="P:OpenQA.Selenium.IWebElement.TagName" /> property returns the tag name of the
    /// element, not the value of the name attribute. For example, it will return
    /// "input" for an element specified by the HTML markup &lt;input name="foo" /&gt;.
    /// </remarks>
    public virtual string TagName => WrappedElement.TagName;

    /// <summary>
    /// Gets the innerText of this element, without any leading or trailing whitespace,
    /// and with other whitespace collapsed.
    /// </summary>
    public virtual string Text => WrappedElement.Text;

    /// <summary>
    /// Gets an element.
    /// </summary>
    /// <typeparam name="T">The type of the element to get.</typeparam>
    /// <param name="locationKey">The location key to find the control.</param>
    /// <returns>A element of type <typeparamref name="T"/>.</returns>
    public static T GetElement<T>(SeleniumBy locationKey)
        where T : WebElement
    {
        var ctor = typeof(T).GetConstructor(new[] { typeof(SeleniumBy) });
        if (ctor == null)
            throw new InvalidOperationException("The class " + typeof(T).FullName + " needs to implement a constructor with one By-Parameter");
        return ctor.Invoke(new object[] { locationKey }) as T;
    }

    /// <summary>
    /// Gets an element.
    /// </summary>
    /// <typeparam name="T">The type of the element to get.</typeparam>
    /// <param name="locationKey">The location key to find the control.</param>
    /// <param name="searchContext">The element to which the element is relative to.</param>
    /// <returns>A element of type <typeparamref name="T"/>.</returns>
    public static T GetElement<T>(SeleniumBy locationKey, ISearchContext searchContext)
        where T : WebElement
    {
        var ctor = typeof(T).GetConstructor(new[] { typeof(SeleniumBy), typeof(ISearchContext) });
        if (ctor == null)
            throw new InvalidOperationException("The class " + typeof(T).FullName + " needs to implement a constructor with one By-Parameter and one ISearchContext-Parameter");
        return ctor.Invoke(new object[] { locationKey, searchContext }) as T;
    }

    /// <summary>
    /// Determines wether the element is stale (so not interactable).
    /// </summary>
    /// <param name="element">The element to check.</param>
    /// <returns>True if the element is stale; otherwise false.</returns>
    public static bool IsStale(IWebElement element)
    {
        try
        {
            return element == null || !element.Enabled;
        }
        catch (StaleElementReferenceException)
        {
            return true;
        }
        catch (NoSuchElementException)
        {
            return true;
        }
    }

    /// <summary>
    /// Reloads the wrapped element.
    /// </summary>
    /// <returns>The <see cref="IWebElement"/> that has been found.</returns>
    /// <exception cref="System.NotSupportedException">The wrapped element can only be reloaded if the element is lazy.</exception>
    public virtual IWebElement ReloadWrappedElement()
    {
        if (!IsLazy)
            throw new NotSupportedException("The wrapped element can only be reloaded if the element is lazy.");
        _wrappedElement = null;
        Initialize();
        return WrappedElement;
    }

    /// <summary>
    /// Returns the current instance of the <see cref="WrappedElement"/> property without trying to find the control.
    /// </summary>
    /// <returns>The current wrapped <see cref="IWebElement"/>.</returns>
    public IWebElement GetCurrentWrappedElement()
    {
        return _wrappedElement;
    }

    /// <summary>
    /// Reloads the wrapped element if this element is lazy.
    /// </summary>
    /// <returns>The <see cref="IWebElement"/> that has been found.</returns>
    public virtual IWebElement TryReloadWrappedElement()
    {
        if (IsLazy)
            return ReloadWrappedElement();
        return WrappedElement;
    }

    /// <summary>
    /// Tries to find the wrapped native selenium element.
    /// </summary>
    /// <returns>Returns the wrapped element if the element exists; otherwise null.</returns>
    public virtual IWebElement TryFindWrappedElement()
    {
        return Exists ? _wrappedElement : null;
    }

    /// <summary>
    /// Scrolls until the element is in view.
    /// </summary>
    public virtual void ScrollIntoView()
    {
        Driver.ExecuteScript("arguments[0].scrollIntoView(true);", WrappedElement);
    }

    /// <summary>
    /// Performs a double click on this element.
    /// </summary>
    public virtual void DoubleClick()
    {
        try
        {
            Driver.GetActions().DoubleClick(WrappedElement).Perform();
        }
        catch (InvalidOperationException)
        {
            ScrollIntoView();
            Driver.GetActions().DoubleClick(WrappedElement).Perform();
        }
    }

    /// <summary>
    /// Performs a right click on this element.
    /// </summary>
    public virtual void RightClick()
    {
        Driver.GetActions().ContextClick(WrappedElement).Perform();
    }

    /// <summary>
    /// Tries to get the given attribute value as integer.
    /// </summary>
    /// <param name="attributeName">Name of the attribute.</param>
    /// <returns>The attribute value parsed to <see cref="int"/>.</returns>
    public int? TryGetAttributeAsInt32(string attributeName)
    {
        return TryGetAttribute(attributeName, Convert.ToInt32);
    }

    /// <summary>
    /// Tries to get the given attribute value as double.
    /// </summary>
    /// <param name="attributeName">Name of the attribute.</param>
    /// <returns>The attribute value parsed to <see cref="double"/>.</returns>
    public double? TryGetAttributeAsDouble(string attributeName)
    {
        return TryGetAttribute(attributeName, Convert.ToDouble);
    }

    /// <summary>
    /// Tries to get the given attribute value as boolean.
    /// </summary>
    /// <param name="attributeName">Name of the attribute.</param>
    /// <returns>The attribute value parsed to <see cref="bool"/>.</returns>
    public bool? TryGetAttributeAsBoolean(string attributeName)
    {
        return TryGetAttribute(attributeName, Convert.ToBoolean);
    }

    /// <summary>
    /// Tries to get the given attribute value as DateTime.
    /// </summary>
    /// <param name="attributeName">Name of the attribute.</param>
    /// <returns>The attribute value parsed to <see cref="DateTime"/>.</returns>
    public DateTime? TryGetAttributeAsDateTime(string attributeName)
    {
        return TryGetAttribute(attributeName, Convert.ToDateTime);
    }

    /// <summary>
    /// Gets the given attribute value and tries to parse it with the given function.
    /// </summary>
    /// <typeparam name="T">Type to parse into.</typeparam>
    /// <param name="attributeName">Name of the attribute.</param>
    /// <param name="converter">The converter.</param>
    /// <returns>The attribute value parsed to <typeparamref name="T"/>.</returns>
    public T? TryGetAttribute<T>(string attributeName, Func<string, T> converter)
        where T : struct
    {
        Guard.NotNull(converter);
        try
        {
            var value = GetAttribute(attributeName);
            if (string.IsNullOrEmpty(value))
                return null;
            return converter(value);
        }
        catch (FormatException)
        {
            return null;
        }
        catch (NoSuchWindowException)
        {
            return null;
        }
    }

    /// <summary>
    /// Gets a new <see cref="WebElement"/> representing a parent of this element.
    /// </summary>
    /// <param name="levels">The number of levels to go up.</param>
    /// <returns>A <see cref="WebElement"/> representing the parent of this element.</returns>
    public WebElement GetParentElement(int levels = 1)
    {
        if (levels < 1)
            throw new ArgumentOutOfRangeException(nameof(levels), "The argument 'levels' has to be greater than 0!");
        var path = string.Join("/", Enumerable.Range(0, levels).Select(x => ".."));
        return new WebElement(Driver, SeleniumBy.XPath(path), this);
    }

    /// <summary>
    /// Gets a new <see cref="WebElement"/> representing a parent of this element.
    /// </summary>
    /// <typeparam name="T">The type of the parent element.</typeparam>
    /// <param name="levels">The number of levels to go up.</param>
    /// <returns>A element of type <typeparamref name="T"/> representing the parent of this element.</returns>
    public T GetParentElement<T>(int levels = 1)
        where T : WebElement
    {
        if (levels < 1)
            throw new ArgumentOutOfRangeException(nameof(levels), "The Argument 'levels' has to be greater than 0!");
        var path = string.Join("/", Enumerable.Range(0, levels).Select(x => ".."));
        return GetElement<T>(SeleniumBy.XPath(path), this);
    }

    /// <summary>
    /// Moves the mouse to the center of this element.
    /// </summary>
    public void MoveMouseToElement()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            DesktopUtility.SetCursorPosition(0, 0);
        Driver.GetActions().MoveToElement(this).Perform();
    }

    /// <summary>
    /// Moves the mouse to the element.
    /// </summary>
    /// <param name="offsetX">The x offset.</param>
    /// <param name="offsetY">The y offset.</param>
    public void MoveMouseToElement(int offsetX, int offsetY)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            DesktopUtility.SetCursorPosition(0, 0);
        Driver.GetActions().MoveToElement(this, offsetX, offsetY).Perform();
    }

    /// <summary>
    /// Gets a string representing the search path that is used to find this element.
    /// </summary>
    /// <param name="multiline">Determines wether to wrap the description to multiple lines.</param>
    /// <returns>A string representing the search path that is used to find this element.</returns>
    public string GetSearchDescription(bool multiline = false)
    {
        if (!IsLazy)
            return "Not lazy: " + (_wrappedElement == null ? "(null)" : _wrappedElement.ToString());
        var result = string.Empty;
        if (SearchContext is WebDriver driver)
        {
            var knownHandles = driver.KnownWindowHandles ?? new List<string>();
            var openedDrivers = driver.Context.AllOpenedDrivers;
            var handle = driver.CurrentWindowHandle;
            result += string.Format("Driver {0} (wdw:{1} - {2})", openedDrivers.IndexOf(driver), handle, knownHandles.IndexOf(handle));
        }
        else if (SearchContext is IWebDriver idriver)
        {
            result += "Driver (wdw:" + idriver.CurrentWindowHandle + ")";
        }
        else if (SearchContext is WebElement element)
        {
            result += element.GetSearchDescription(multiline);
        }
        else if (SearchContext is IWebElement ielement)
        {
            result += GetStringForElement(ielement, Driver);
        }

        string o = IsLazy ? LocationKey.ToString() : GetStringForElement(_wrappedElement, Driver);
        return result + (multiline ? Environment.NewLine + "   " : string.Empty) + " => " + (o ?? "(null)");
    }

    /// <summary>
    /// Gets the value of a java script property of this element.
    /// </summary>
    /// <param name="propertyName">The name of the property to get the value of.</param>
    /// <returns>The value of the given java script property.</returns>
    public string GetProperty(string propertyName)
    {
        return Driver.ExecuteScript("return arguments[0]." + propertyName, this)?.ToString();
    }

    /// <summary>
    /// Sets a DOM attribute to this element.
    /// </summary>
    /// <param name="attributeName">The name of the attribute to set.</param>
    /// <param name="value">The value to set.</param>
    public void SetAttribute(string attributeName, object value)
    {
        Driver.ExecuteScript("arguments[0].setAttribute(arguments[1], arguments[2]);", this, attributeName, value);
    }

    /// <summary>
    /// Searches through the search contexts until a context of the specified type is found.
    /// </summary>
    /// <typeparam name="T">The desired search context type.</typeparam>
    /// <param name="matchExactType">Determines wether the type has to match exactly.</param>
    /// <returns>Returns the wanted search context if one was found; otherwise null.</returns>
    public T FindAncestorSearchContext<T>(bool matchExactType)
        where T : class, ISearchContext
    {
        var currentContext = SearchContext;
        while (currentContext != null && ((matchExactType && currentContext.GetType() != typeof(T)) || (!matchExactType && !typeof(T).IsInstanceOfType(currentContext))))
        {
            currentContext = (currentContext as WebElement)?.SearchContext;
        }

        return currentContext as T;
    }

    /// <summary>
    /// Updates the location key that is used to find this element. This will also remove the current element cache and resets child controls.
    /// </summary>
    /// <param name="newLocationKey">The location key that should be used to search the element.</param>
    /// <param name="searchContext">The search context in which the element should be searched in.</param>
    public void UpdateLocationKey(SeleniumBy newLocationKey, ISearchContext searchContext)
    {
        LocationKey = newLocationKey;
        SearchContext = searchContext;
        _wrappedElement = null;
        Initialize();
    }

    /// <summary>
    /// Waits until this element exists.
    /// </summary>
    /// <returns>True if the element existed in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilExists.ThrowOnFailure().Start() instead.")]
    public bool WaitUntilExists() => WaitUntilExists(Core.Wait.DefaultOptions.TimeoutMs, true);

    /// <summary>
    /// Waits until this element exists.
    /// </summary>
    /// <param name="assert">Determines wether to throw an AssertFailedException if the timeout expires.</param>
    /// <returns>True if the element existed in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilExists.Start() instead. If assert is true add .ThrowOnFailure() before starting.")]
    public bool WaitUntilExists(bool assert) => WaitUntilExists(Core.Wait.DefaultOptions.TimeoutMs, assert);

    /// <summary>
    /// Waits until this element exists.
    /// </summary>
    /// <param name="timeout">The timeout in miliseconds.</param>
    /// <returns>True if the element existed in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilExists.WithTimeout(timeout).ThrowOnFailure().Start() instead.")]
    public bool WaitUntilExists(int timeout) => WaitUntilExists(timeout, true);

    /// <summary>
    /// Waits until this element exists.
    /// </summary>
    /// <param name="timeout">The timeout in miliseconds.</param>
    /// <param name="assert">Determines wether to throw an AssertFailedException if the timeout expires.</param>
    /// <returns>True if the element existed in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilExists.WithTimeout(timeout).Start() instead. If assert is true add .ThrowOnFailure() before starting.")]
    public bool WaitUntilExists(int timeout, bool assert)
        => Wait.UntilExists.WithTimeout(timeout).OnFailure(assert, "Element could not be found: " + GetSearchDescription()).Start().Value;

    /// <summary>
    /// Waits until this element is displayed.
    /// </summary>
    /// <returns>True if the element is displayed in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilDisplayed.ThrowOnFailure().Start() instead.")]
    public bool WaitUntilDisplayed() => WaitUntilDisplayed(Core.Wait.DefaultOptions.TimeoutMs, true);

    /// <summary>
    /// Waits until this element is displayed.
    /// </summary>
    /// <param name="assert">Determines wether to throw an AssertFailedException if the timeout expires.</param>
    /// <returns>True if the element is displayed in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilDisplayed.Start() instead. If assert is true add .ThrowOnFailure() before starting.")]
    public bool WaitUntilDisplayed(bool assert) => WaitUntilDisplayed(Core.Wait.DefaultOptions.TimeoutMs, assert);

    /// <summary>
    /// Waits until this element is displayed and Assert.
    /// </summary>
    /// <param name="timeout">The timeout in miliseconds.</param>
    /// <returns>True if the element is displayed in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilDisplayed.WithTimeout(timeout).ThrowOnFailure().Start() instead.")]
    public bool WaitUntilDisplayed(int timeout) => WaitUntilDisplayed(timeout, true);

    /// <summary>
    /// Waits until this element is displayed.
    /// </summary>
    /// <param name="timeout">The timeout in miliseconds.</param>
    /// <param name="assert">Determines wether to throw an AssertFailedException if the timeout expires.</param>
    /// <returns>True if the element is displayed in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilDisplayed.WithTimeout(timeout).Start() instead. If assert is true add .ThrowOnFailure() before starting.")]
    public bool WaitUntilDisplayed(int timeout, bool assert)
        => Wait.UntilDisplayed.WithTimeout(timeout).OnFailure(assert, "Element does not exist or is not displayed: " + GetSearchDescription()).Start().Value;

    /// <summary>
    /// Waits until this element does not exist.
    /// </summary>
    /// <returns>True if the element vanished in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilNotExists.ThrowOnFailure().Start() instead.")]
    public bool WaitUntilNotExists() => WaitUntilNotExists(Core.Wait.DefaultOptions.TimeoutMs, true);

    /// <summary>
    /// Waits until this element does not exist.
    /// </summary>
    /// <param name="assert">Determines wether to throw an AssertFailedException if the timeout expires.</param>
    /// <returns>True if the element vanished in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilNotExists.Start() instead. If assert is true add .ThrowOnFailure() before starting.")]
    public bool WaitUntilNotExists(bool assert) => WaitUntilNotExists(Core.Wait.DefaultOptions.TimeoutMs, assert);

    /// <summary>
    /// Waits until this element does not exist.
    /// </summary>
    /// <param name="timeout">The timeout in miliseconds.</param>
    /// <returns>True if the element vanished in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilNotExists.WithTimeout(timeout).ThrowOnFailure().Start() instead.")]
    public bool WaitUntilNotExists(int timeout) => WaitUntilNotExists(timeout, true);

    /// <summary>
    /// Waits until this element does not exist.
    /// </summary>
    /// <param name="timeout">The timeout in miliseconds.</param>
    /// <param name="assert">Determines wether to throw an AssertFailedException if the timeout expires.</param>
    /// <returns>True if the element vanished in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilNotExists.WithTimeout(timeout).Start() instead. If assert is true add .ThrowOnFailure() before starting.")]
    public bool WaitUntilNotExists(int timeout, bool assert)
        => Wait.UntilNotExists.WithTimeout(timeout).OnFailure(assert, "Element does still exist: " + GetSearchDescription()).Start().Value;

    /// <summary>
    /// Waits until this element is not displayed.
    /// </summary>
    /// <returns>True if the element disappeared in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilNotDisplayed.ThrowOnFailure().Start() instead.")]
    public bool WaitUntilNotDisplayed() => WaitUntilNotDisplayed(Core.Wait.DefaultOptions.TimeoutMs, true);

    /// <summary>
    /// Waits until this element is not displayed.
    /// </summary>
    /// <param name="assert">Determines wether to throw an AssertFailedException if the timeout expires.</param>
    /// <returns>True if the element disappeared in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilNotDisplayed.Start() instead. If assert is true add .ThrowOnFailure() before starting.")]
    public bool WaitUntilNotDisplayed(bool assert) => WaitUntilNotDisplayed(Core.Wait.DefaultOptions.TimeoutMs, assert);

    /// <summary>
    /// Waits until this element is not displayed.
    /// </summary>
    /// <param name="timeout">The timeout in miliseconds.</param>
    /// <returns>True if the element disappeared in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilNotDisplayed.WithTimeout(timeout).ThrowOnFailure().Start() instead.")]
    public bool WaitUntilNotDisplayed(int timeout) => WaitUntilNotDisplayed(timeout, true);

    /// <summary>
    /// Waits until this element is not displayed.
    /// </summary>
    /// <param name="timeout">The timeout in miliseconds.</param>
    /// <param name="assert">Determines wether to throw an AssertFailedException if the timeout expires.</param>
    /// <returns>True if the element disappeared in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilNotDisplayed.WithTimeout(timeout).Start() instead. If assert is true add .ThrowOnFailure() before starting.")]
    public bool WaitUntilNotDisplayed(int timeout, bool assert)
        => Wait.UntilNotDisplayed.WithTimeout(timeout).OnFailure(assert, "Element is still displayed: " + GetSearchDescription()).Start().Value;

    /// <summary>
    /// Waits until this element is clickable.
    /// </summary>
    /// <returns>True if the element is clickable in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilClickable.ThrowOnFailure().Start() instead.")]
    public bool WaitUntilClickable() => WaitUntilClickable(Core.Wait.DefaultOptions.TimeoutMs, true);

    /// <summary>
    /// Waits until this element is clickable.
    /// </summary>
    /// <param name="assert">Determines wether to throw an AssertFailedException if the timeout expires.</param>
    /// <returns>True if the element is clickable in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilClickable.Start() instead. If assert is true add .ThrowOnFailure() before starting.")]
    public bool WaitUntilClickable(bool assert) => WaitUntilClickable(Core.Wait.DefaultOptions.TimeoutMs, assert);

    /// <summary>
    /// Waits until this element is clickable.
    /// </summary>
    /// <param name="timeout">The timeout in miliseconds.</param>
    /// <returns>True if the element is clickable in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilClickable.WithTimeout(timeout).ThrowOnFailure().Start() instead.")]
    public bool WaitUntilClickable(int timeout) => WaitUntilClickable(timeout, true);

    /// <summary>
    /// Waits until this element is clickable.
    /// </summary>
    /// <param name="timeout">The timeout in miliseconds.</param>
    /// <param name="assert">Determines wether to throw an AssertFailedException if the timeout expires.</param>
    /// <returns>True if the element is clickable in time; otherwise false.</returns>
    [Obsolete("Use element.Wait.UntilClickable.WithTimeout(timeout).Start() instead. If assert is true add .ThrowOnFailure() before starting.")]
    public bool WaitUntilClickable(int timeout, bool assert)
        => Wait.UntilClickable.WithTimeout(timeout).OnFailure(assert, "Element is not clickable: " + GetSearchDescription()).Start().Value;

    /// <summary>
    /// Clears the content of this element.
    /// </summary>
    /// <remarks>
    /// If this element is a text entry element, the <see cref="M:OpenQA.Selenium.IWebElement.Clear" />
    /// method will clear the value. It has no effect on other elements. Text entry elements
    /// are defined as elements with INPUT or TEXTAREA tags.
    /// </remarks>
    public virtual void Clear()
    {
        WrappedElement.Clear();
    }

    /// <summary>
    /// Clicks this element.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Click this element. If the click causes a new page to load, the <see cref="M:OpenQA.Selenium.IWebElement.Click" />
    /// method will attempt to block until the page has loaded. After calling the
    /// <see cref="M:OpenQA.Selenium.IWebElement.Click" /> method, you should discard all references to this
    /// element unless you know that the element and the page will still be present.
    /// Otherwise, any further operations performed on this element will have an undefined.
    /// behavior.
    /// </para>
    /// <para>
    /// If this element is not clickable, then this operation is ignored. This allows you to
    /// simulate a users to accidentally missing the target when clicking.
    /// </para>
    /// </remarks>
    public virtual void Click() => Click(false);

    /// <summary>
    /// Clicks this element.
    /// </summary>
    /// <param name="forceClick">
    /// Determines whether to force click by using javascript function.
    /// </param>
    /// <remarks>
    /// <para>
    /// Click this element. If the click causes a new page to load, the <see cref="M:OpenQA.Selenium.IWebElement.Click" />
    /// method will attempt to block until the page has loaded. After calling the
    /// <see cref="M:OpenQA.Selenium.IWebElement.Click" /> method, you should discard all references to this
    /// element unless you know that the element and the page will still be present.
    /// Otherwise, any further operations performed on this element will have an undefined.
    /// behavior.
    /// </para>
    /// <para>
    /// If this element is not clickable, then this operation is ignored. This allows you to
    /// simulate a users to accidentally missing the target when clicking.
    /// </para>
    /// </remarks>
    public virtual void Click(bool forceClick)
    {
        Wait.UntilExists.ThrowOnFailure().Start();
        bool clickSuccessfull = false;
        if (!forceClick)
        {
            if (!Wait.UntilDisplayed.WithTimeout(5000).Start().Value || !Wait.UntilClickable.WithTimeout(5000).Start().Value)
                ScrollIntoView();

            if (Wait.UntilClickable.WithTimeout(5000).Start().Value)
            {
                try
                {
                    WrappedElement.Click();
                    clickSuccessfull = true;
                }
                catch (InvalidOperationException)
                {
                }
                catch (ElementClickInterceptedException)
                {
                }
            }
        }

        if (!clickSuccessfull)
            Driver.ExecuteScript("arguments[0].click();", WrappedElement);
    }

    /// <summary>
    /// Finds the first <see cref="T:OpenQA.Selenium.IWebElement" /> using the given method.
    /// </summary>
    /// <param name="by">The locating mechanism to use.</param>
    /// <returns>
    /// The first matching <see cref="T:OpenQA.Selenium.IWebElement" /> on the current context.
    /// </returns>
    public virtual IWebElement FindElement(SeleniumBy by)
    {
        return WrappedElement.FindElement(by);
    }

    /// <summary>
    /// Finds all <see cref="T:OpenQA.Selenium.IWebElement">IWebElements</see> within the current context
    /// using the given mechanism.
    /// </summary>
    /// <param name="by">The locating mechanism to use.</param>
    /// <returns>
    /// A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> of all <see cref="T:OpenQA.Selenium.IWebElement">WebElements</see>
    /// matching the current criteria, or an empty list if nothing matches.
    /// </returns>
    public virtual ReadOnlyCollection<IWebElement> FindElements(SeleniumBy by)
    {
        return WrappedElement.FindElements(by);
    }

    /// <summary>
    /// Gets the value of the specified attribute for this element.
    /// </summary>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <returns>
    /// The attribute's current value. Returns a <see langword="null" /> if the
    /// value is not set.
    /// </returns>
    /// <remarks>
    /// The <see cref="M:OpenQA.Selenium.IWebElement.GetAttribute(System.String)" /> method will return the current value
    /// of the attribute, even if the value has been modified after the page has been
    /// loaded. Note that the value of the following attributes will be returned even if
    /// there is no explicit attribute on the element:
    /// <list type="table"><listheader><term>Attribute name</term><term>Value returned if not explicitly specified</term><term>Valid element types</term></listheader><item><description>checked</description><description>checked</description><description>Check Box</description></item><item><description>selected</description><description>selected</description><description>Options in Select elements</description></item><item><description>disabled</description><description>disabled</description><description>Input and other UI elements</description></item></list>
    /// </remarks>
    public virtual string GetAttribute(string attributeName)
    {
        return WrappedElement.GetAttribute(attributeName);
    }

    /// <summary>
    /// Gets the value of a CSS property of this element.
    /// </summary>
    /// <param name="propertyName">The name of the CSS property to get the value of.</param>
    /// <returns>
    /// The value of the specified CSS property.
    /// </returns>
    /// <remarks>
    /// The value returned by the <see cref="M:OpenQA.Selenium.IWebElement.GetCssValue(System.String)" />
    /// method is likely to be unpredictable in a cross-browser environment.
    /// Color values should be returned as hex strings. For example, a
    /// "background-color" property set as "green" in the HTML source, will
    /// return "#008000" for its value.
    /// </remarks>
    public virtual string GetCssValue(string propertyName)
    {
        return WrappedElement.GetCssValue(propertyName);
    }

    /// <summary>
    /// Simulates typing text into the element.
    /// </summary>
    /// <param name="text">The text to type into the element.</param>
    /// <remarks>
    /// The text to be typed may include special characters like arrow keys,
    /// backspaces, function keys, and so on. Valid special keys are defined in
    /// <see cref="T:OpenQA.Selenium.Keys" />.
    /// </remarks>
    /// <seealso cref="T:OpenQA.Selenium.Keys" />
    public virtual void SendKeys(string text)
    {
        WrappedElement.SendKeys(text);
    }

    /// <summary>
    /// Submits this element to the web server.
    /// </summary>
    /// <remarks>
    /// If this current element is a form, or an element within a form,
    /// then this will be submitted to the web server. If this causes the current
    /// page to change, then this method will block until the new page is loaded.
    /// </remarks>
    public virtual void Submit()
    {
        WrappedElement.Submit();
    }

    /// <inheritdoc/>
    public string GetDomAttribute(string attributeName)
    {
        return WrappedElement.GetDomAttribute(attributeName);
    }

    /// <inheritdoc/>
    public string GetDomProperty(string propertyName)
    {
        return WrappedElement.GetDomProperty(propertyName);
    }

    /// <inheritdoc/>
    public ISearchContext GetShadowRoot()
    {
        return WrappedElement.GetShadowRoot();
    }

    /// <summary>
    /// Creates the wait entry used for this instance.
    /// </summary>
    /// <returns>The wait entry used for this instance.</returns>
    protected virtual WaitEntry CreateWaitEntry()
    {
        return new WaitEntry(this);
    }

    private static string GetStringForElement(IWebElement element, IJavaScriptExecutor javaScript)
    {
        ICollection<string> attributes;
        try
        {
            attributes = javaScript.ExecuteScript("var items = {}; for (index = 0; index < arguments[0].attributes.length; ++index) { items[arguments[0].attributes[index].name] = arguments[0].attributes[index].value }; return items;", element) as ICollection<string> ?? Array.Empty<string>();
        }
        catch
        {
            attributes = Array.Empty<string>();
        }

        return string.Format("<{0}{1}>...</{0}>", element.TagName, attributes.Count > 0 ? " " + string.Join(" ", attributes) : string.Empty);
    }

    /// <summary>
    /// Wait entry for the <see cref="WebElement"/> class.
    /// </summary>
    public class WaitEntry : Core.WaitEntry
    {
        private readonly WebElement _element;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitEntry"/> class.
        /// </summary>
        /// <param name="element">The web element.</param>
        public WaitEntry(WebElement element)
        {
            _element = element;
        }

        /// <summary>
        /// Gets a wait operation that waits until this element exists.
        /// </summary>
        public virtual IWait<bool> UntilExists
            => Until(OnCheckExists).WithDefaultErrorMessage($"Element could not be found: {_element.GetSearchDescription()}");

        /// <summary>
        /// Gets a wait operation that waits until this element does not exists.
        /// </summary>
        public virtual IWait<bool> UntilNotExists
            => Until(OnCheckNotExists).WithDefaultErrorMessage($"Element does still exist: {_element.GetSearchDescription()}");

        /// <summary>
        /// Gets a wait operation that waits until this element is displayed.
        /// </summary>
        public virtual IWait<bool> UntilDisplayed
            => Until(OnCheckDisplayed).WithDefaultErrorMessage($"Element does not exist or is not displayed: {_element.GetSearchDescription()}");

        /// <summary>
        /// Gets a wait operation that waits until this element is not displayed.
        /// </summary>
        public virtual IWait<bool> UntilNotDisplayed
            => Until(OnCheckNotDisplayed).WithDefaultErrorMessage($"Element is still displayed: {_element.GetSearchDescription()}");

        /// <summary>
        /// Gets a wait operation that waits until this element is clickable.
        /// </summary>
        public virtual IWait<bool> UntilClickable
            => Until(OnCheckClickable).WithDefaultErrorMessage($"Element is not clickable: {_element.GetSearchDescription()}");

        /// <summary>
        /// Gets a wait operation that waits until this element is not clickable.
        /// </summary>
        public virtual IWait<bool> UntilNotClickable
            => Until(OnCheckNotClickable).WithDefaultErrorMessage($"Element is still clickable: {_element.GetSearchDescription()}");

        /// <summary>
        /// Called when the <see cref="UntilExists"/> wait operations checks whether to continue waiting or not.
        /// </summary>
        /// <returns>When <c>true</c> is returned, the wait operation is completed; otherwisem, the waiting continues.</returns>
        protected virtual bool OnCheckExists() => _element.Exists;

        /// <summary>
        /// Called when the <see cref="UntilNotExists"/> wait operations checks whether to continue waiting or not.
        /// </summary>
        /// <returns>When <c>true</c> is returned, the wait operation is completed; otherwisem, the waiting continues.</returns>
        protected virtual bool OnCheckNotExists() => !_element.Exists;

        /// <summary>
        /// Called when the <see cref="UntilDisplayed"/> wait operations checks whether to continue waiting or not.
        /// </summary>
        /// <returns>When <c>true</c> is returned, the wait operation is completed; otherwisem, the waiting continues.</returns>
        protected virtual bool OnCheckDisplayed() => _element.Displayed;

        /// <summary>
        /// Called when the <see cref="UntilNotDisplayed"/> wait operations checks whether to continue waiting or not.
        /// </summary>
        /// <returns>When <c>true</c> is returned, the wait operation is completed; otherwisem, the waiting continues.</returns>
        protected virtual bool OnCheckNotDisplayed() => !_element.Displayed;

        /// <summary>
        /// Called when the <see cref="UntilClickable"/> wait operations checks whether to continue waiting or not.
        /// </summary>
        /// <returns>When <c>true</c> is returned, the wait operation is completed; otherwisem, the waiting continues.</returns>
        protected virtual bool OnCheckClickable() => _element.IsClickable;

        /// <summary>
        /// Called when the <see cref="UntilNotClickable"/> wait operations checks whether to continue waiting or not.
        /// </summary>
        /// <returns>When <c>true</c> is returned, the wait operation is completed; otherwisem, the waiting continues.</returns>
        protected virtual bool OnCheckNotClickable() => !_element.IsClickable;
    }
}
