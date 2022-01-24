using Rocketcress.Core;
using Rocketcress.Core.Base;
using Rocketcress.Core.Extensions;
using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.ControlSearch;
using Rocketcress.UIAutomation.Exceptions;
using Rocketcress.UIAutomation.Extensions;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Mouse = Rocketcress.UIAutomation.Interaction.Mouse;

namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// UITestControl provides the ability to locate controls on a User Interface.
/// It provides properties and methods which are generic to controls across technologies.
/// </summary>
public class UITestControl : TestObjectBase, IUITestControl
{
    internal const int ShortControlActionTimeout = 1000;
    internal const int LongControlActionTimeout = 5000;

    private AutomationElement _automationElement;
    private UITestControl _parent;
    private UITestControl _window;

    /// <summary>
    /// Initializes a new instance of the <see cref="UITestControl"/> class as lazy element.
    /// </summary>
    /// <param name="application">The application which hosts this control.</param>
    /// <param name="locationKey">The location key.</param>
    public UITestControl(Application application, By locationKey)
        : this(application, locationKey, (UITestControl)null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UITestControl"/> class as lazy element.
    /// </summary>
    /// <param name="application">The application which hosts this control.</param>
    /// <param name="parent">The parent control.</param>
    public UITestControl(Application application, IUITestControl parent)
        : this(application, By.Empty, parent)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UITestControl"/> class as lazy element.
    /// </summary>
    /// <param name="application">The application which hosts this control.</param>
    /// <param name="locationKey">The location key.</param>
    /// <param name="parent">The parent control.</param>
    public UITestControl(Application application, By locationKey, AutomationElement parent)
        : this(application, locationKey, new UITestControl(application, parent))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UITestControl"/> class as lazy element.
    /// </summary>
    /// <param name="application">The application which hosts this control.</param>
    /// <param name="locationKey">The location key.</param>
    /// <param name="parent">The parent control.</param>
    public UITestControl(Application application, By locationKey, IUITestControl parent)
        : this(application)
    {
        if (parent is not null)
        {
            if (parent.Application != application)
                throw new ArgumentException("The parent needs to be part of the same application.", nameof(parent));
            parent.AutomationElementChanged += (s, e) => ClearAutomationElementCache();
        }

        LocationKey = (BaseLocationKey ?? By.Empty).Append(locationKey, true, true);
        SearchContext = parent;
        Initialize();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UITestControl"/> class as non-lazy element.
    /// </summary>
    /// <param name="application">The application which hosts this control.</param>
    /// <param name="element">The underlying <see cref="System.Windows.Automation.AutomationElement"/>.</param>
    public UITestControl(Application application, AutomationElement element)
        : this(application)
    {
        LocationKey = null;
        SearchContext = null;
        _automationElement = element ?? throw new ArgumentNullException(nameof(element), "The automation element cannot be null.");
        Initialize();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UITestControl"/> class as lazy element.
    /// </summary>
    /// <param name="application">The application which hosts this control.</param>
    protected UITestControl(Application application)
    {
        Application = application ?? throw new ArgumentNullException(nameof(application));
    }

    /// <summary>
    /// Event that is triggered whenever the underlying <see cref="System.Windows.Automation.AutomationElement"/> changed.
    /// </summary>
    public event EventHandler AutomationElementChanged;

    /// <summary>
    /// Gets the location key that is used to find the underlying <see cref="System.Windows.Automation.AutomationElement"/>.
    /// </summary>
    public virtual By LocationKey { get; private set; }

    /// <summary>
    /// Gets the parent control.
    /// </summary>
    public virtual IUITestControl SearchContext { get; private set; }

    /// <summary>
    /// Gets the application to which the current control is associated with.
    /// </summary>
    public Application Application { get; }

    /// <summary>
    /// Gets a value indicating whether the underlying <see cref="System.Windows.Automation.AutomationElement"/> is loaded dynamically.
    /// </summary>
    public virtual bool IsLazy => LocationKey != null;

    /// <summary>
    /// gets the underlying <see cref="System.Windows.Automation.AutomationElement"/>.
    /// </summary>
    public virtual AutomationElement AutomationElement
    {
        get
        {
            if (IsLazy && _automationElement.IsStale())
                Find();
            return _automationElement;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the control exists.
    /// </summary>
    public virtual bool Exists
    {
        get
        {
            try
            {
                if (IsLazy && _automationElement.IsStale())
                    TryFind();
                return _automationElement?.IsStale() == false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether the control exists and is displayed (not offscreen).
    /// </summary>
    public virtual bool Displayed => Exists && !IsOffscreen;

    /// <summary>
    /// Gets a lazy <see cref="IUITestControl"/> object representing the parent of this control.
    /// </summary>
    public virtual IUITestControl Parent
    {
        get
        {
            if (_parent == null)
                _parent = new UITestControl(Application, By.Scope(TreeScope.Parent), this);
            return _parent;
        }
    }

    /// <summary>
    /// Gets a lazy <see cref="IUITestControl"/> object representing the window, in which the current control is contained in.
    /// </summary>
    public virtual IUITestControl Window
    {
        get
        {
            if (_window == null)
                _window = new UITestControl(Application, By.ControlType(ControlType.Window).AndScope(TreeScope.Ancestors | TreeScope.Element), this);
            return _window;
        }
    }

    /// <summary>
    /// Gets the automation id of this control.
    /// </summary>
    public virtual string AutomationId => GetPropertyValue<string>(AutomationElement.AutomationIdProperty);

    /// <summary>
    /// Gets a value indicating whether this control is enabled.
    /// </summary>
    public virtual bool Enabled => GetPropertyValue<bool>(AutomationElement.IsEnabledProperty);

    /// <summary>
    /// Gets the Bounding rectangle for this control.
    /// </summary>
    public virtual Rect BoundingRectangle => GetPropertyValue<Rect>(AutomationElement.BoundingRectangleProperty);

    /// <summary>
    /// Gets a value indicating whether the control has a bounding rectangle.
    /// </summary>
    public virtual bool HasBoundingRectangle => BoundingRectangle != new Rect(0, 0, 0, 0);

    /// <summary>
    /// Gets the screen location for this control.
    /// </summary>
    public virtual Point Location => BoundingRectangle.Location;

    /// <summary>
    /// Gets the size of this control.
    /// </summary>
    public virtual Size Size => BoundingRectangle.Size;

    /// <summary>
    /// Gets a value indicating whether this control can have keyboard focus.
    /// </summary>
    public virtual bool IsFocusable => GetPropertyValue<bool>(AutomationElement.IsKeyboardFocusableProperty);

    /// <summary>
    /// Gets a value indicating whether this control has keyboard focus.
    /// </summary>
    public virtual bool HasFocus => GetPropertyValue<bool>(AutomationElement.HasKeyboardFocusProperty);

    /// <summary>
    /// Gets the control type of this control.
    /// </summary>
    public virtual ControlType ControlType => GetPropertyValue<ControlType>(AutomationElement.ControlTypeProperty);

    /// <summary>
    /// Gets the name of this control.
    /// </summary>
    public virtual string Name => GetPropertyValue<string>(AutomationElement.NameProperty);

    /// <summary>
    /// Gets the id of the process, from which this control was created from.
    /// </summary>
    public virtual int ProcessId => GetPropertyValue<int>(AutomationElement.ProcessIdProperty);

    /// <summary>
    /// Gets a value indicating whether this control is offscreen.
    /// </summary>
    public virtual bool IsOffscreen => GetPropertyValue<bool>(AutomationElement.IsOffscreenProperty);

    /// <summary>
    /// Gets the native window handle from this control.
    /// </summary>
    public virtual IntPtr WindowHandle => new(GetPropertyValue<int>(AutomationElement.NativeWindowHandleProperty));

    /// <summary>
    /// Gets a point on screen that can be used to click on this control.
    /// </summary>
    public virtual Point ClickablePoint
    {
        get
        {
            if (!AutomationElement.TryGetClickablePoint(out Point point))
                point = GetPropertyValue<Point?>(AutomationElement.ClickablePointProperty) ?? new Point(int.MinValue, int.MinValue);
            if (point.X == int.MinValue || point.Y == int.MinValue)
            {
                if (!Wait.Until(() => BoundingRectangle != new Rect(0, 0, 0, 0)).WithTimeout(5000).Start().Value)
                    throw new UIAutomationControlException("A clickable point could not be obtained", this);
                point = BoundingRectangle.GetAbsoluteCenter();
            }

            return point;
        }
    }

    /// <summary>
    /// Gets the class name of this control.
    /// </summary>
    public virtual string ClassName => GetPropertyValue<string>(AutomationElement.ClassNameProperty);

    /// <summary>
    /// Gets the help text of this control.
    /// </summary>
    public virtual string HelpText => GetPropertyValue<string>(AutomationElement.HelpTextProperty);

    /// <summary>
    /// Gets a base location key that is prepended to the location key provided to the constructor.
    /// </summary>
    protected virtual By BaseLocationKey => By.Empty;

    /// <summary>
    /// Clears the underlying AutomationElement, so that on the next acces to the <see cref="AutomationElement"/> property, this control is searched again.
    /// </summary>
    public virtual void ClearAutomationElementCache()
    {
        if (!IsLazy)
            throw new UIActionNotSupportedException("The cache can only be cleared on a lazy control.", this);
        _automationElement = null;
        AutomationElementChanged?.Invoke(this, new EventArgs());
    }

    /// <summary>
    /// Tries to find the control defined by this <see cref="IUITestControl"/>.
    /// </summary>
    /// <returns>Return true, if the control was found; otherwise false.</returns>
    public virtual bool TryFind()
    {
        if (!IsLazy)
            throw new UIActionNotSupportedException("The control cannot be search on a lazy control.", this);

        var parent = SearchContext == null ? AutomationElement.RootElement : SearchContext.GetAutomationElementFailFast();
        if (parent == null)
            return false;

        _automationElement = SearchEngine.FindFirst(LocationKey, parent);
        AutomationElementChanged?.Invoke(this, new EventArgs());
        return !_automationElement.IsStale();
    }

    /// <summary>
    /// Searches for the control defined by this <see cref="IUITestControl"/> and throws an exception if it is not found.
    /// </summary>
    public virtual void Find()
    {
        if (!IsLazy)
            throw new UIActionNotSupportedException("The control cannot be search on a lazy control.", this);
        var parent = SearchContext?.AutomationElement ?? AutomationElement.RootElement;
        if (Wait.Until(() => _automationElement = SearchEngine.FindFirst(LocationKey, parent)).Start().Value == null)
            throw new NoSuchElementException(this);
        AutomationElementChanged?.Invoke(this, new EventArgs());
    }

    /// <summary>
    /// Tries to find the control defined by this <see cref="IUITestControl"/> and returns the <see cref="AutomationElement"/> if it was found.
    /// </summary>
    /// <returns>Returns the <see cref="AutomationElement"/> if a control was found; otherwise null.</returns>
    public AutomationElement GetAutomationElementFailFast() => Exists ? _automationElement : null;

    /// <summary>
    /// Tries to find the control defined by this <see cref="IUITestControl"/> and returns the <see cref="AutomationElement"/> if it was found.
    /// </summary>
    /// <param name="element">The element to return to.</param>
    /// <returns>Return true, if the control was found; otherwise false.</returns>
    public bool TryGetAutomationElementFailFast(out AutomationElement element) => (element = GetAutomationElementFailFast()) != null;

    /// <summary>
    /// Clicks the control on a clickable point.
    /// </summary>
    public virtual void Click()
    {
        EnsureClickable();
        if (!Enabled)
            LogWarning("Clicking on disabled control.");
        Mouse.Click(this);
    }

    /// <summary>
    /// Clicks on the point relative to the top left corner of the control.
    /// </summary>
    /// <param name="relativePoint">The relative point to click on.</param>
    public virtual void Click(Point relativePoint)
    {
        EnsureClickable();
        if (!Enabled)
            LogWarning("Clicking on disabled control.");
        Mouse.Click(this, relativePoint);
    }

    /// <summary>
    /// Right-Clicks the control on a clickable point.
    /// </summary>
    public virtual void RightClick()
    {
        EnsureClickable();
        if (!Enabled)
            LogWarning("Right clicking on disabled control.");
        Mouse.Click(this, MouseButtons.Right, ModifierKeys.None);
    }

    /// <summary>
    /// Right-Clicks on the point relative to the top left corner of the control.
    /// </summary>
    /// <param name="relativePoint">The relative point to click on.</param>
    public virtual void RightClick(Point relativePoint)
    {
        EnsureClickable();
        if (!Enabled)
            LogWarning("Right clicking on disabled control.");
        Mouse.Click(this, MouseButtons.Right, ModifierKeys.None, relativePoint);
    }

    /// <summary>
    /// Double-Clicks the control on a clickable point.
    /// </summary>
    public virtual void DoubleClick()
    {
        EnsureClickable();
        if (!Enabled)
            LogWarning("Double clicking on disabled control.");
        Mouse.DoubleClick(this);
    }

    /// <summary>
    /// Double-Clicks on the point relative to the top left corner of the control.
    /// </summary>
    /// <param name="relativePoint">The relative point to click on.</param>
    public virtual void DoubleClick(Point relativePoint)
    {
        EnsureClickable();
        if (!Enabled)
            LogWarning("Double clicking on disabled control.");
        Mouse.DoubleClick(this, relativePoint);
    }

    /// <summary>
    /// Drags from a clickable point on this control and drops on a clickable point on the specified target control.
    /// </summary>
    /// <param name="toControl">The drag target control.</param>
    public virtual void DragDrop(IUITestControl toControl)
    {
        Guard.NotNull(toControl);

        EnsureClickable();
        toControl.EnsureClickable();
        if (!Enabled)
            LogWarning("Dragging from a disabled control.");
        if (!toControl.Enabled)
            LogWarning("Dropping on a disabled control.");

        Mouse.StartDragging(this);
        Mouse.StopDragging(toControl);
    }

    /// <summary>
    /// Drags from a clickable point on this control and drops on the speficied relative point to this control.
    /// </summary>
    /// <param name="relativePoint">The relative point to drop.</param>
    public virtual void DragDropRelative(Point relativePoint)
    {
        EnsureClickable();
        if (!Enabled)
            LogWarning("Dragging from a disabled control.");
        Mouse.StartDragging(this);
        Mouse.StopDragging(this, relativePoint);
    }

    /// <summary>
    /// Drags from a clickable point on this control and drops on the specified absolute point.
    /// </summary>
    /// <param name="absolutePoint">The absolute point.</param>
    public virtual void DragDropAbsolute(Point absolutePoint)
    {
        EnsureClickable();
        if (!Enabled)
            LogWarning("Dragging from a disabled control.");
        Mouse.StartDragging(this);
        Mouse.StopDragging(absolutePoint);
    }

    /// <summary>
    /// Moves the mouse to a clickable point on this control.
    /// </summary>
    /// <returns><c>true</c> when the mouse has been moved; otherwise <c>false</c>.</returns>
    public virtual bool MoveMouseToClickablePoint() => MoveMouseSlowlyToClickablePoint(0, Wait.DefaultOptions.TimeoutMs, true);

    /// <summary>
    /// Moves the mouse to a clickable point on this control.
    /// </summary>
    /// <param name="assert">Determined wether to throw an exception if the control is not displayed in time.</param>
    /// <returns><c>true</c> when the mouse has been moved; otherwise <c>false</c>.</returns>
    public virtual bool MoveMouseToClickablePoint(bool assert) => MoveMouseSlowlyToClickablePoint(0, Wait.DefaultOptions.TimeoutMs, assert);

    /// <summary>
    /// Moves the mouse to a clickable point on this control.
    /// </summary>
    /// <param name="timeout">The time to wait for the control to be displayed before moving.</param>
    /// <param name="assert">Determined wether to throw an exception if the control is not displayed in time.</param>
    /// <returns><c>true</c> when the mouse has been moved; otherwise <c>false</c>.</returns>
    public virtual bool MoveMouseToClickablePoint(int timeout, bool assert) => MoveMouseSlowlyToClickablePoint(0, timeout, assert);

    /// <summary>
    /// Moves the mouse to a clickable point on this control.
    /// </summary>
    /// <param name="duration">The duration of the mouse travel.</param>
    /// <returns><c>true</c> when the mouse has been moved; otherwise <c>false</c>.</returns>
    public virtual bool MoveMouseSlowlyToClickablePoint(int duration) => MoveMouseSlowlyToClickablePoint(duration, Wait.DefaultOptions.TimeoutMs, true);

    /// <summary>
    /// Moves the mouse to a clickable point on this control.
    /// </summary>
    /// <param name="duration">The duration of the mouse travel.</param>
    /// <param name="assert">Determined wether to throw an exception if the control is not displayed in time.</param>
    /// <returns><c>true</c> when the mouse has been moved; otherwise <c>false</c>.</returns>
    public virtual bool MoveMouseSlowlyToClickablePoint(int duration, bool assert) => MoveMouseSlowlyToClickablePoint(duration, Wait.DefaultOptions.TimeoutMs, assert);

    /// <summary>
    /// Moves the mouse to a clickable point on this control.
    /// </summary>
    /// <param name="duration">The duration of the mouse travel.</param>
    /// <param name="timeout">The time to wait for the control to be displayed before moving.</param>
    /// <param name="assert">Determined wether to throw an exception if the control is not displayed in time.</param>
    /// <returns><c>true</c> when the mouse has been moved; otherwise <c>false</c>.</returns>
    public virtual bool MoveMouseSlowlyToClickablePoint(int duration, int timeout, bool assert)
    {
        if (!WaitUntilDisplayed(timeout))
            return false;
        Mouse.Hover(this, duration);
        return true;
    }

    /// <summary>
    /// Scrolls the user interface to make sure that the control is clickable.
    /// </summary>
    public virtual void EnsureClickable()
    {
        foreach (var element in new AutomationElement[] { AutomationElement }.Concat(GetAllParents()))
        {
            if (element.TryGetCurrentPattern(VirtualizedItemPattern.Pattern, out object virtualizedItemPatternObj) && virtualizedItemPatternObj is VirtualizedItemPattern virtualizedItemPattern)
            {
                virtualizedItemPattern.Realize();
            }
            else if (element.TryGetCurrentPattern(ScrollItemPattern.Pattern, out object scrollItemPatternObj) && scrollItemPatternObj is ScrollItemPattern scrollItemPattern)
            {
                scrollItemPattern.ScrollIntoView();
            }
        }

        WaitUntilDisplayed(1000, false);
    }

    /// <summary>
    /// Send the specified keys to the control.
    /// </summary>
    /// <param name="keys">The keys to press.</param>
    public virtual void SendKeys(string keys)
    {
        SetFocus();
        System.Windows.Forms.SendKeys.SendWait(keys);
    }

    /// <summary>
    /// Sets the keyboard focus to the control.
    /// </summary>
    public virtual void SetFocus()
    {
        var result = Wait.Until(() =>
        {
            AutomationElement.SetFocus();
            return true;
        }).WithTimeGap(1000).Start();

        if (!result.Value)
            throw new UIAutomationControlException("Setting the focus on the control failed.", this, result.Exceptions.LastOrDefault());
    }

    /// <summary>
    /// Searches for a descendant control that matches the given location key.
    /// </summary>
    /// <param name="locationKey">The location key that is used for the search.</param>
    /// <exception cref="NoSuchElementException">A control with the given location key was not found.</exception>
    /// <returns>A <see cref="UITestControl"/> that represents the first found control.</returns>
    public virtual IUITestControl FindElement(By locationKey) => ControlUtility.GetControl(Application, SearchEngine.FindFirst(locationKey, AutomationElement) ?? throw new NoSuchElementException(this, locationKey));

    /// <summary>
    /// Searches for all descemdamt controls that matches the given location key.
    /// </summary>
    /// <param name="locationKey">The location key that is used for the search.</param>
    /// <returns>An enumerable of UITestControls, which contains all found controls.</returns>
    public virtual IEnumerable<IUITestControl> FindElements(By locationKey) => SearchEngine.FindAll(locationKey, AutomationElement).Select(x => ControlUtility.GetControl(Application, x));

    /// <summary>
    /// Returns the parent of the current control.
    /// </summary>
    /// <returns>The parent of the current control.</returns>
    public virtual IUITestControl FindWindow() => FindElements(By.ControlType(ControlType.Window).AndScope(TreeScope.Ancestors)).FirstOrDefault();

    /// <summary>
    /// Returns the window in which the current control is located in.
    /// </summary>
    /// <returns>The window in which the current control is located in.</returns>
    public virtual IUITestControl FindParent() => FindElements(By.Scope(TreeScope.Parent)).FirstOrDefault();

    /// <summary>
    /// Returns an enumerable of all first-level children of the current control.
    /// </summary>
    /// <returns>An enumerable of all first-level children of the current control.</returns>
    public virtual IEnumerable<IUITestControl> GetChildren() => FindElements(By.Scope(TreeScope.Children));

    /// <summary>
    /// Gets the value of a specified PropertyValue of the underlying <see cref="System.Windows.Automation.AutomationElement"/>.
    /// Waits until the element exists.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="property">The property to retrieve the value from.</param>
    /// <returns>The value for the specified property on the underlying <see cref="System.Windows.Automation.AutomationElement"/>.</returns>
    public virtual T GetPropertyValue<T>(AutomationProperty property) => (T)AutomationElement.GetCurrentPropertyValue(property);

    /// <summary>
    /// Gets the value of a specified PropertyValue of the underlying <see cref="System.Windows.Automation.AutomationElement"/>.
    /// Does not wait for the element to exist.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="property">The property to retrieve the value from.</param>
    /// <param name="assert">Determines wether an exception should be thrown when the element could not be found.</param>
    /// <param name="fallbackValue">The value to return when the element could not be found.</param>
    /// <returns>
    /// The value for the specified property on the underlying <see cref="System.Windows.Automation.AutomationElement"/>.
    /// If the element does not exists <paramref name="fallbackValue"/> is returned.
    /// </returns>
    public virtual T GetPropertyValueFailFast<T>(AutomationProperty property, bool assert, T fallbackValue)
    {
        if (IsLazy)
        {
            if (TryFind())
                return (T)_automationElement.GetCurrentPropertyValue(property);
            else if (assert)
                throw new NoSuchElementException(this);
            else
                return fallbackValue;
        }
        else
        {
            return (T)AutomationElement.GetCurrentPropertyValue(property);
        }
    }

    /// <summary>
    /// Checks wether the specified pattern is available on this control.
    /// </summary>
    /// <typeparam name="T">The type of the pattern to check.</typeparam>
    /// <returns>true if the control supports the specified pattern; otherwise false.</returns>
    public virtual bool IsPatternAvailable<T>()
        where T : BasePattern
        => (bool)AutomationElement.GetCurrentPropertyValue(PatternUtility.GetIsPatternAvailableProperty<T>());

    /// <summary>
    /// Retrieves the specified pattern from this control.
    /// </summary>
    /// <typeparam name="T">The type of the pattern.</typeparam>
    /// <returns>The retrieved pattern.</returns>
    public virtual T GetPattern<T>()
        where T : BasePattern
        => (T)AutomationElement.GetCurrentPattern(PatternUtility.GetPattern<T>());

    /// <summary>
    /// Retrieved the specified pattern from this control if the pattern is supported.
    /// </summary>
    /// <typeparam name="T">The type of the pattern.</typeparam>
    /// <param name="pattern">The pattern that is retrieved. If the control did not support the pattern, this will be null.</param>
    /// <returns>A value indicating wheather the control does support the specified pattern.</returns>
    public virtual bool TryGetPattern<T>(out T pattern)
        where T : BasePattern
    {
        AutomationElement.TryGetCurrentPattern(PatternUtility.GetPattern<T>(), out object patternObject);
        pattern = patternObject as T;
        if (pattern == null && patternObject != null)
            LogWarning($"Pattern object returned from UIA is not of type {typeof(T).FullName}.");
        return pattern != null;
    }

    /// <summary>
    /// Creates description of the search condition for this control.
    /// </summary>
    /// <returns>A description representing this control.</returns>
    public virtual string GetSearchDescription() => GetSearchDescription(false);

    /// <summary>
    /// Creates description of the search condition for this control.
    /// </summary>
    /// <param name="multiline">Determines whether to use line breaks between parent and child control in the description.</param>
    /// <returns>A description representing this control.</returns>
    public virtual string GetSearchDescription(bool multiline)
    {
        var result = new StringBuilder();
        if (!IsLazy)
        {
            result.Append("Not Lazy: ")
                  .Append(_automationElement.GetSearchDescription());
        }
        else
        {
            if (SearchContext != null)
            {
                var strParent = SearchContext.GetSearchDescription(multiline);
                if (multiline)
                    result.AppendLine(strParent).Append("   ");
                else
                    result.Append(strParent);
                result.Append(" => ");
            }

            result.Append(LocationKey.ToString());
        }

        return result.ToString();
    }

    /// <summary>
    /// Returns a String that represents the current control.
    /// </summary>
    /// <returns>A String that represents the current control.</returns>
    public override string ToString() => GetSearchDescription();

    /// <summary>
    /// Waits until the control exists.
    /// </summary>
    /// <returns>true if the control exists; otherwise false.</returns>
    public virtual bool WaitUntilExists() => WaitUntilExists(Wait.DefaultOptions.TimeoutMs, true);

    /// <summary>
    /// Waits until the control exists.
    /// </summary>
    /// <param name="timeout">The timeout for the wait.</param>
    /// <returns>true if the control exists; otherwise false.</returns>
    public virtual bool WaitUntilExists(int timeout) => WaitUntilExists(timeout, true);

    /// <summary>
    /// Waits until the control exists.
    /// </summary>
    /// <param name="assert">Determines if the test should be marked as failed if the control does not exists after the wait.</param>
    /// <returns>true if the control exists; otherwise false.</returns>
    public virtual bool WaitUntilExists(bool assert) => WaitUntilExists(Wait.DefaultOptions.TimeoutMs, assert);

    /// <summary>
    /// Waits until the control exists.
    /// </summary>
    /// <param name="timeout">The timeout for the wait.</param>
    /// <param name="assert">Determines if the test should be marked as failed if the control does not exists after the wait.</param>
    /// <returns>true if the control exists; otherwise false.</returns>
    public virtual bool WaitUntilExists(int timeout, bool assert)
    {
        return Wait.Until(() => Exists).WithTimeout(timeout).OnFailure(assert, "Element could not be found: " + GetSearchDescription()).Start().Value;
    }

    /// <summary>
    /// Waits until the control is displayed.
    /// </summary>
    /// <returns>true if the control is displayed; otherwise false.</returns>
    public virtual bool WaitUntilDisplayed() => WaitUntilDisplayed(Wait.DefaultOptions.TimeoutMs, true);

    /// <summary>
    /// Waits until the control is displayed.
    /// </summary>
    /// <param name="timeout">The timeout for the wait.</param>
    /// <returns>true if the control is displayed; otherwise false.</returns>
    public virtual bool WaitUntilDisplayed(int timeout) => WaitUntilDisplayed(timeout, true);

    /// <summary>
    /// Waits until the control is displayed.
    /// </summary>
    /// <param name="assert">Determines if the test should be marked as failed if the control is not displayed after the wait.</param>
    /// <returns>true if the control is displayed; otherwise false.</returns>
    public virtual bool WaitUntilDisplayed(bool assert) => WaitUntilDisplayed(Wait.DefaultOptions.TimeoutMs, assert);

    /// <summary>
    /// Waits until the control is displayed.
    /// </summary>
    /// <param name="timeout">The timeout for the wait.</param>
    /// <param name="assert">Determines if the test should be marked as failed if the control is not displayed after the wait.</param>
    /// <returns>true if the control is displayed; otherwise false.</returns>
    public virtual bool WaitUntilDisplayed(int timeout, bool assert)
    {
        return Wait.Until(() => Displayed).WithTimeout(timeout).OnFailure(assert, "Element does not exist or is not displayed: " + GetSearchDescription()).Start().Value;
    }

    /// <summary>
    /// Waits until the control does not exist.
    /// </summary>
    /// <returns>true if the control does not exist; otherwise false.</returns>
    public virtual bool WaitUntilNotExists() => WaitUntilNotExists(Wait.DefaultOptions.TimeoutMs, true);

    /// <summary>
    /// Waits until the control does not exist.
    /// </summary>
    /// <param name="timeout">The timeout for the wait.</param>
    /// <returns>true if the control does not exist; otherwise false.</returns>
    public virtual bool WaitUntilNotExists(int timeout) => WaitUntilNotExists(timeout, true);

    /// <summary>
    /// Waits until the control does not exist.
    /// </summary>
    /// <param name="assert">Determines if the test should be marked as failed if the control still exists after the wait.</param>
    /// <returns>true if the control does not exist; otherwise false.</returns>
    public virtual bool WaitUntilNotExists(bool assert) => WaitUntilNotExists(Wait.DefaultOptions.TimeoutMs, assert);

    /// <summary>
    /// Waits until the control does not exist.
    /// </summary>
    /// <param name="timeout">The timeout for the wait.</param>
    /// <param name="assert">Determines if the test should be marked as failed if the control still exists after the wait.</param>
    /// <returns>true if the control does not exist; otherwise false.</returns>
    public virtual bool WaitUntilNotExists(int timeout, bool assert)
    {
        return Wait.Until(() => !Exists).WithTimeout(timeout).OnFailure(assert, "Element does still exist: " + GetSearchDescription()).Start().Value;
    }

    /// <summary>
    /// Waits until the control is not displayed.
    /// </summary>
    /// <returns>true if the control is not displayed; otherwise false.</returns>
    public virtual bool WaitUntilNotDisplayed() => WaitUntilNotDisplayed(Wait.DefaultOptions.TimeoutMs, true);

    /// <summary>
    /// Waits until the control is not displayed.
    /// </summary>
    /// <param name="timeout">The timeout for the wait.</param>
    /// <returns>true if the control is not displayed; otherwise false.</returns>
    public virtual bool WaitUntilNotDisplayed(int timeout) => WaitUntilNotDisplayed(timeout, true);

    /// <summary>
    /// Waits until the control is not displayed.
    /// </summary>
    /// <param name="assert">Determines if the test should be marked as failed if the control is still displayed after the wait.</param>
    /// <returns>true if the control is not displayed; otherwise false.</returns>
    public virtual bool WaitUntilNotDisplayed(bool assert) => WaitUntilNotDisplayed(Wait.DefaultOptions.TimeoutMs, assert);

    /// <summary>
    /// Waits until the control is not displayed.
    /// </summary>
    /// <param name="timeout">The timeout for the wait.</param>
    /// <param name="assert">Determines if the test should be marked as failed if the control is still displayed after the wait.</param>
    /// <returns>true if the control is not displayed; otherwise false.</returns>
    public virtual bool WaitUntilNotDisplayed(int timeout, bool assert)
    {
        return Wait.Until(() => !Displayed).WithTimeout(timeout).OnFailure(assert, "Element is still displayed: " + GetSearchDescription()).Start().Value;
    }

    /// <summary>
    /// Blocks the current thread until this control is ready to receive mouse or keyboard input, or until the default time-out expires.
    /// </summary>
    /// <returns>true if this control is ready to receive mouse or keyboard input before the time-out; otherwise, false.</returns>
    public virtual bool WaitForControlReady() => WaitForControlReady(Wait.DefaultOptions.TimeoutMs, true);

    /// <summary>
    /// Blocks the current thread until this control is ready to receive mouse or keyboard input, or until the default time-out expires.
    /// </summary>
    /// <param name="timeout">The timeout for the wait.</param>
    /// <returns>true if this control is ready to receive mouse or keyboard input before the time-out; otherwise, false.</returns>
    public virtual bool WaitForControlReady(int timeout) => WaitForControlReady(timeout, true);

    /// <summary>
    /// Blocks the current thread until this control is ready to receive mouse or keyboard input, or until the default time-out expires.
    /// </summary>
    /// <param name="assert">Determines if the test should be marked as failed if the control is not ready after the wait.</param>
    /// <returns>true if this control is ready to receive mouse or keyboard input before the time-out; otherwise, false.</returns>
    public virtual bool WaitForControlReady(bool assert) => WaitForControlReady(Wait.DefaultOptions.TimeoutMs, assert);

    /// <summary>
    /// Blocks the current thread until this control is ready to receive mouse or keyboard input, or until the default time-out expires.
    /// </summary>
    /// <param name="timeout">The timeout for the wait.</param>
    /// <param name="assert">Determines if the test should be marked as failed if the control is not ready after the wait.</param>
    /// <returns>true if this control is ready to receive mouse or keyboard input before the time-out; otherwise, false.</returns>
    public virtual bool WaitForControlReady(int timeout, bool assert)
    {
        var pid = TestHelper.Try(() => ProcessId);
        if (pid <= 0)
            return true;
        var process = Process.GetProcessById(pid);
        if (process == null)
            return true;
        return Wait.Until(() =>
        {
            try
            {
                return process.WaitForInputIdle(5000);
            }
            catch (Win32Exception ex)
            {
                Logger.LogWarning("Error while waiting for input idle: {0}", ex);
                return true;
            }
        }).OnFailure(assert, $"The control not not ready after {timeout / 1000:0.00} seconds of waiting.").Start().Value;
    }

    /// <summary>
    /// Searches through the search contexts until a context of the specified type is found.
    /// </summary>
    /// <typeparam name="T">The desired search context type.</typeparam>
    /// <param name="matchExactType">Determines wether the type has to match exactly.</param>
    /// <returns>Returns the wanted search context if one was found; otherwise null.</returns>
    public T FindAncestorSearchContext<T>(bool matchExactType)
        where T : UITestControl
    {
        var currentContext = SearchContext;
        while (currentContext != null && ((matchExactType && currentContext.GetType() != typeof(T)) || (!matchExactType && !typeof(T).IsInstanceOfType(currentContext))))
        {
            currentContext = currentContext.SearchContext;
        }

        return currentContext as T;
    }

    /// <summary>
    /// Reads the color at the specific relative point of the control from screen.
    /// </summary>
    /// <param name="point">The relative point to read the pixel from.</param>
    /// <returns>Returns the color that is displayed relative to this control.</returns>
    public virtual System.Drawing.Color GetColorFromPoint(Point point) => GetColorFromPoint((int)point.X, (int)point.Y);

    /// <summary>
    /// Reads the color at the specific relative point of the control from screen.
    /// </summary>
    /// <param name="x">The relative x-coordinate to read the pixel from.</param>
    /// <param name="y">The relative y-coordinate to read the pixel from.</param>
    /// <returns>Returns the color that is displayed relative to this control.</returns>
    public virtual System.Drawing.Color GetColorFromPoint(int x, int y)
    {
        var loc = BoundingRectangle.Location;
        return WindowsApiHelper.GetScreenPixel((int)loc.X + x, (int)loc.Y + y);
    }

    internal static By GetBaseLocationKey(Type controlType)
    {
        if (!typeof(UITestControl).IsAssignableFrom(controlType))
            throw new InvalidOperationException($"Type \"{controlType.FullName}\" needs to be derived from \"{typeof(UITestControl).FullName}\".");
        var constructor = controlType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Array.Empty<Type>(), null);
        if (constructor == null)
            throw new InvalidOperationException($"Type \"{controlType.FullName}\" needs an empty constructor.");
        var instance = (UITestControl)constructor.Invoke(Array.Empty<object>());
        return instance.BaseLocationKey;
    }

    internal AutomationElement GetCachedAutomationElement()
    {
        return _automationElement;
    }

    /// <summary>
    /// Logs the specified debug message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="params">The parameters.</param>
    protected internal virtual void LogDebug(string message, params object[] @params) => Logger.LogDebug(message + $" ({GetSearchDescription()})", @params);

    /// <summary>
    /// Logs the specified informational message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="params">The parameters.</param>
    protected internal virtual void LogInfo(string message, params object[] @params) => Logger.LogInfo(message + $" ({GetSearchDescription()})", @params);

    /// <summary>
    /// Logs the specified warning message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="params">The parameters.</param>
    protected internal virtual void LogWarning(string message, params object[] @params) => Logger.LogWarning(message + $" ({GetSearchDescription()})", @params);

    /// <summary>
    /// Logs the specified error message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="params">The parameters.</param>
    protected internal virtual void LogError(string message, params object[] @params) => Logger.LogError(message + $" ({GetSearchDescription()})", @params);

    /// <summary>
    /// Logs the specified critical message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="params">The parameters.</param>
    protected internal virtual void LogCritical(string message, params object[] @params) => Logger.LogCritical(message + $" ({GetSearchDescription()})", @params);

    /// <summary>
    /// Initializes the control and potentially child control for this control. This method is executed in the constructors.
    /// </summary>
    protected virtual void Initialize()
    {
    }

    private IEnumerable<AutomationElement> GetAllParents()
    {
        var result = new LinkedList<AutomationElement>();
        var walker = TreeWalker.RawViewWalker;
        var current = AutomationElement;
        while (current != null && current != AutomationElement.RootElement)
        {
            if (current != AutomationElement)
                result.AddFirst(current);
            current = walker.GetParent(current);
        }

        return result;
    }
}

/// <summary>
/// Provides extension methods for the <see cref="IUITestControl"/> interface.
/// </summary>
public static class UITestControlExtensions
{
    /// <summary>
    /// Returns an enumerable of all UITestControls that match the specified control definition.
    /// </summary>
    /// <typeparam name="T">The type of control to find.</typeparam>
    /// <param name="control">The control under which to find controls.</param>
    /// <returns>A list of found controls.</returns>
    public static IEnumerable<T> FindMatchingControls<T>(this T control)
        where T : IUITestControl
    {
        if (!control.IsLazy)
            throw new NotSupportedException("This method is not supported by Non-Lazy controls.");
        return SearchEngine.FindAll(control.LocationKey, control.SearchContext?.AutomationElement ?? AutomationElement.RootElement)
            .Select(x => (T)Activator.CreateInstance(typeof(T), x));
    }
}
