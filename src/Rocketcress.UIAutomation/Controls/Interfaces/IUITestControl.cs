using Rocketcress.UIAutomation.Exceptions;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls
{
    public interface IUITestControl
    {
        event EventHandler AutomationElementChanged;

        #region Properties

        /// <summary>
        /// Gets the location key that is used to find the underlying <see cref="System.Windows.Automation.AutomationElement"/>.
        /// </summary>
        By LocationKey { get; }

        /// <summary>
        /// Gets the parent control.
        /// </summary>
        IUITestControl SearchContext { get; }

        /// <summary>
        /// Gets the application to which the current control is associated with.
        /// </summary>
        Application Application { get; }

        /// <summary>
        /// Gets a value indicating whether the underlying <see cref="System.Windows.Automation.AutomationElement"/> is loaded dynamically.
        /// </summary>
        bool IsLazy { get; }

        /// <summary>
        /// Gets the underlying <see cref="System.Windows.Automation.AutomationElement"/>.
        /// </summary>
        AutomationElement AutomationElement { get; }

        /// <summary>
        /// Gets a value indicating whether the control exists.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Gets a value indicating whether the control exists and is displayed (not offscreen).
        /// </summary>
        bool Displayed { get; }
        #endregion

        #region AutomationElement Properties

        /// <summary>
        /// Gets a lazy <see cref="IUITestControl"/> object representing the parent of this control.
        /// </summary>
        IUITestControl Parent { get; }

        /// <summary>
        /// Gets a lazy <see cref="IUITestControl"/> object representing the window, in which the current control is contained in.
        /// </summary>
        IUITestControl Window { get; }

        /// <summary>
        /// Gets the automation id of this control.
        /// </summary>
        string AutomationId { get; }

        /// <summary>
        /// Gets a value indicating whether this control is enabled.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Gets the Bounding rectangle for this control.
        /// </summary>
        Rect BoundingRectangle { get; }

        /// <summary>
        /// Gets a value indicating whether the control has a bounding rectangle.
        /// </summary>
        bool HasBoundingRectangle { get; }

        /// <summary>
        /// Gets the screen location for this control.
        /// </summary>
        Point Location { get; }

        /// <summary>
        /// Gets the size of this control.
        /// </summary>
        Size Size { get; }

        /// <summary>
        /// Gets a value indicating whether this control can have keyboard focus.
        /// </summary>
        bool IsFocusable { get; }

        /// <summary>
        /// Gets a value indicating whether this control has keyboard focus.
        /// </summary>
        bool HasFocus { get; }

        /// <summary>
        /// Gets the control type of this control.
        /// </summary>
        ControlType ControlType { get; }

        /// <summary>
        /// Gets the name of this control.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the id of the process, from which this control was created from.
        /// </summary>
        int ProcessId { get; }

        /// <summary>
        /// Gets a value indicating whether this control is offscreen.
        /// </summary>
        bool IsOffscreen { get; }

        /// <summary>
        /// Gets the native window handle from this control.
        /// </summary>
        IntPtr WindowHandle { get; }

        /// <summary>
        /// Gets a point on screen that can be used to click on this control.
        /// </summary>
        Point ClickablePoint { get; }

        /// <summary>
        /// Gets the class name of this control.
        /// </summary>
        string ClassName { get; }

        /// <summary>
        /// Gets the help text of this control.
        /// </summary>
        string HelpText { get; }
        #endregion

        #region Methods

        /// <summary>
        /// Clears the underlying AutomationElement, so that on the next acces to the <see cref="AutomationElement"/> property, this control is searched again.
        /// </summary>
        void ClearAutomationElementCache();

        /// <summary>
        /// Tries to find the control defined by this <see cref="IUITestControl"/>.
        /// </summary>
        /// <returns>Return true, if the control was found; otherwise false.</returns>
        bool TryFind();

        /// <summary>
        /// Searches for the control defined by this <see cref="IUITestControl"/> and throws an exception if it is not found.
        /// </summary>
        void Find();

        /// <summary>
        /// Tries to find the control defined by this <see cref="IUITestControl"/> and returns the <see cref="AutomationElement"/> if it was found.
        /// </summary>
        /// <returns>Returns the <see cref="AutomationElement"/> if a control was found; otherwise null.</returns>
        AutomationElement GetAutomationElementFailFast();

        /// <summary>
        /// Tries to find the control defined by this <see cref="IUITestControl"/> and returns the <see cref="AutomationElement"/> if it was found.
        /// </summary>
        /// <param name="element">The element to return to.</param>
        /// <returns>Return true, if the control was found; otherwise false.</returns>
        bool TryGetAutomationElementFailFast(out AutomationElement element);

        /// <summary>
        /// Clicks the control on a clickable point.
        /// </summary>
        void Click();

        /// <summary>
        /// Clicks on the point relative to the top left corner of the control.
        /// </summary>
        /// <param name="relativePoint">The relative point to click on.</param>
        void Click(Point relativePoint);

        /// <summary>
        /// Right-Clicks the control on a clickable point.
        /// </summary>
        void RightClick();

        /// <summary>
        /// Right-Clicks on the point relative to the top left corner of the control.
        /// </summary>
        /// <param name="relativePoint">The relative point to click on.</param>
        void RightClick(Point relativePoint);

        /// <summary>
        /// Double-Clicks the control on a clickable point.
        /// </summary>
        void DoubleClick();

        /// <summary>
        /// Double-Clicks on the point relative to the top left corner of the control.
        /// </summary>
        /// <param name="relativePoint">The relative point to click on.</param>
        void DoubleClick(Point relativePoint);

        /// <summary>
        /// Drags from a clickable point on this control and drops on a clickable point on the specified target control.
        /// </summary>
        /// <param name="toControl">The drag target control.</param>
        void DragDrop(IUITestControl toControl);

        /// <summary>
        /// Drags from a clickable point on this control and drops on the speficied relative point to this control.
        /// </summary>
        /// <param name="relativePoint">The relative point to drop.</param>
        void DragDropRelative(Point relativePoint);

        /// <summary>
        /// Drags from a clickable point on this control and drops on the specified absolute point.
        /// </summary>
        /// <param name="absolutePoint">The absolute point.</param>
        void DragDropAbsolute(Point absolutePoint);

        /// <summary>
        /// Moves the mouse to a clickable point on this control.
        /// </summary>
        /// <returns><c>true</c> when the mouse has been moved; otherwise <c>false</c>.</returns>
        bool MoveMouseToClickablePoint();

        /// <summary>
        /// Moves the mouse to a clickable point on this control.
        /// </summary>
        /// <param name="assert">Determined wether to throw an exception if the control is not displayed in time.</param>
        /// <returns><c>true</c> when the mouse has been moved; otherwise <c>false</c>.</returns>
        bool MoveMouseToClickablePoint(bool assert);

        /// <summary>
        /// Moves the mouse to a clickable point on this control.
        /// </summary>
        /// <param name="timeout">The time to wait for the control to be displayed before moving.</param>
        /// <param name="assert">Determined wether to throw an exception if the control is not displayed in time.</param>
        /// <returns><c>true</c> when the mouse has been moved; otherwise <c>false</c>.</returns>
        bool MoveMouseToClickablePoint(int timeout, bool assert);

        /// <summary>
        /// Moves the mouse to a clickable point on this control.
        /// </summary>
        /// <param name="duration">The duration of the mouse travel.</param>
        /// <returns><c>true</c> when the mouse has been moved; otherwise <c>false</c>.</returns>
        bool MoveMouseSlowlyToClickablePoint(int duration);

        /// <summary>
        /// Moves the mouse to a clickable point on this control.
        /// </summary>
        /// <param name="duration">The duration of the mouse travel.</param>
        /// <param name="assert">Determined wether to throw an exception if the control is not displayed in time.</param>
        /// <returns><c>true</c> when the mouse has been moved; otherwise <c>false</c>.</returns>
        bool MoveMouseSlowlyToClickablePoint(int duration, bool assert);

        /// <summary>
        /// Moves the mouse to a clickable point on this control.
        /// </summary>
        /// <param name="duration">The duration of the mouse travel.</param>
        /// <param name="timeout">The time to wait for the control to be displayed before moving.</param>
        /// <param name="assert">Determined wether to throw an exception if the control is not displayed in time.</param>
        /// <returns><c>true</c> when the mouse has been moved; otherwise <c>false</c>.</returns>
        bool MoveMouseSlowlyToClickablePoint(int duration, int timeout, bool assert);

        /// <summary>
        /// Scrolls the user interface to make sure that the control is clickable.
        /// </summary>
        void EnsureClickable();

        /// <summary>
        /// Send the specified keys to the control.
        /// </summary>
        /// <param name="keys">The keys to press.</param>
        void SendKeys(string keys);

        /// <summary>
        /// Sets the keyboard focus to the control.
        /// </summary>
        void SetFocus();

        /// <summary>
        /// Searches for a descendant control that matches the given location key.
        /// </summary>
        /// <param name="locationKey">The location key that is used for the search.</param>
        /// <exception cref="NoSuchElementException">A control with the given location key was not found.</exception>
        /// <returns>A <see cref="UITestControl"/> that represents the first found control.</returns>
        IUITestControl FindElement(By locationKey);

        /// <summary>
        /// Searches for all descemdamt controls that matches the given location key.
        /// </summary>
        /// <param name="locationKey">The location key that is used for the search.</param>
        /// <returns>An enumerable of UITestControls, which contains all found controls.</returns>
        IEnumerable<IUITestControl> FindElements(By locationKey);

        /// <summary>
        /// Returns the parent of the current control.
        /// </summary>
        /// <returns>The parent of the current control.</returns>
        IUITestControl FindWindow();

        /// <summary>
        /// Returns the window in which the current control is located in.
        /// </summary>
        /// <returns>The window in which the current control is located in.</returns>
        IUITestControl FindParent();

        /// <summary>
        /// Returns an enumerable of all first-level children of the current control.
        /// </summary>
        /// <returns>An enumerable of all first-level children of the current control.</returns>
        IEnumerable<IUITestControl> GetChildren();

        /// <summary>
        /// Gets the value of a specified PropertyValue of the underlying <see cref="System.Windows.Automation.AutomationElement"/>.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="property">The property to retrieve the value from.</param>
        /// <returns>The value for the specified property on the underlying <see cref="System.Windows.Automation.AutomationElement"/>.</returns>
        T GetPropertyValue<T>(AutomationProperty property);

        /// <summary>
        /// Checks wether the specified pattern is available on this control.
        /// </summary>
        /// <typeparam name="T">The type of the pattern to check.</typeparam>
        /// <returns>true if the control supports the specified pattern; otherwise false.</returns>
        bool IsPatternAvailable<T>()
            where T : BasePattern;

        /// <summary>
        /// Retrieves the specified pattern from this control.
        /// </summary>
        /// <typeparam name="T">The type of the pattern.</typeparam>
        /// <returns>The retrieved pattern.</returns>
        T GetPattern<T>()
            where T : BasePattern;

        /// <summary>
        /// Retrieved the specified pattern from this control if the pattern is supported.
        /// </summary>
        /// <typeparam name="T">The type of the pattern.</typeparam>
        /// <param name="pattern">The pattern that is retrieved. If the control did not support the pattern, this will be null.</param>
        /// <returns>A value indicating wheather the control does support the specified pattern.</returns>
        bool TryGetPattern<T>(out T pattern)
            where T : BasePattern;

        /// <summary>
        /// Creates description of the search condition for this control.
        /// </summary>
        /// <returns>A description representing this control.</returns>
        string GetSearchDescription();

        /// <summary>
        /// Creates description of the search condition for this control.
        /// </summary>
        /// <param name="multiline">Determines whether to use line breaks between parent and child control in the description.</param>
        /// <returns>A description representing this control.</returns>
        string GetSearchDescription(bool multiline);

        /// <summary>
        /// Waits until the control exists.
        /// </summary>
        /// <returns>true if the control exists; otherwise false.</returns>
        bool WaitUntilExists();

        /// <summary>
        /// Waits until the control exists.
        /// </summary>
        /// <param name="timeout">The timeout for the wait.</param>
        /// <returns>true if the control exists; otherwise false.</returns>
        bool WaitUntilExists(int timeout);

        /// <summary>
        /// Waits until the control exists.
        /// </summary>
        /// <param name="assert">Determines if the test should be marked as failed if the control does not exists after the wait.</param>
        /// <returns>true if the control exists; otherwise false.</returns>
        bool WaitUntilExists(bool assert);

        /// <summary>
        /// Waits until the control exists.
        /// </summary>
        /// <param name="timeout">The timeout for the wait.</param>
        /// <param name="assert">Determines if the test should be marked as failed if the control does not exists after the wait.</param>
        /// <returns>true if the control exists; otherwise false.</returns>
        bool WaitUntilExists(int timeout, bool assert);

        /// <summary>
        /// Waits until the control is displayed.
        /// </summary>
        /// <returns>true if the control is displayed; otherwise false.</returns>
        bool WaitUntilDisplayed();

        /// <summary>
        /// Waits until the control is displayed.
        /// </summary>
        /// <param name="timeout">The timeout for the wait.</param>
        /// <returns>true if the control is displayed; otherwise false.</returns>
        bool WaitUntilDisplayed(int timeout);

        /// <summary>
        /// Waits until the control is displayed.
        /// </summary>
        /// <param name="assert">Determines if the test should be marked as failed if the control is not displayed after the wait.</param>
        /// <returns>true if the control is displayed; otherwise false.</returns>
        bool WaitUntilDisplayed(bool assert);

        /// <summary>
        /// Waits until the control is displayed.
        /// </summary>
        /// <param name="timeout">The timeout for the wait.</param>
        /// <param name="assert">Determines if the test should be marked as failed if the control is not displayed after the wait.</param>
        /// <returns>true if the control is displayed; otherwise false.</returns>
        bool WaitUntilDisplayed(int timeout, bool assert);

        /// <summary>
        /// Waits until the control does not exist.
        /// </summary>
        /// <returns>true if the control does not exist; otherwise false.</returns>
        bool WaitUntilNotExists();

        /// <summary>
        /// Waits until the control does not exist.
        /// </summary>
        /// <param name="timeout">The timeout for the wait.</param>
        /// <returns>true if the control does not exist; otherwise false.</returns>
        bool WaitUntilNotExists(int timeout);

        /// <summary>
        /// Waits until the control does not exist.
        /// </summary>
        /// <param name="assert">Determines if the test should be marked as failed if the control still exists after the wait.</param>
        /// <returns>true if the control does not exist; otherwise false.</returns>
        bool WaitUntilNotExists(bool assert);

        /// <summary>
        /// Waits until the control does not exist.
        /// </summary>
        /// <param name="timeout">The timeout for the wait.</param>
        /// <param name="assert">Determines if the test should be marked as failed if the control still exists after the wait.</param>
        /// <returns>true if the control does not exist; otherwise false.</returns>
        bool WaitUntilNotExists(int timeout, bool assert);

        /// <summary>
        /// Waits until the control is not displayed.
        /// </summary>
        /// <returns>true if the control is not displayed; otherwise false.</returns>
        bool WaitUntilNotDisplayed();

        /// <summary>
        /// Waits until the control is not displayed.
        /// </summary>
        /// <param name="timeout">The timeout for the wait.</param>
        /// <returns>true if the control is not displayed; otherwise false.</returns>
        bool WaitUntilNotDisplayed(int timeout);

        /// <summary>
        /// Waits until the control is not displayed.
        /// </summary>
        /// <param name="assert">Determines if the test should be marked as failed if the control is still displayed after the wait.</param>
        /// <returns>true if the control is not displayed; otherwise false.</returns>
        bool WaitUntilNotDisplayed(bool assert);

        /// <summary>
        /// Waits until the control is not displayed.
        /// </summary>
        /// <param name="timeout">The timeout for the wait.</param>
        /// <param name="assert">Determines if the test should be marked as failed if the control is still displayed after the wait.</param>
        /// <returns>true if the control is not displayed; otherwise false.</returns>
        bool WaitUntilNotDisplayed(int timeout, bool assert);

        /// <summary>
        /// Blocks the current thread until this control is ready to receive mouse or keyboard input, or until the default time-out expires.
        /// </summary>
        /// <returns>true if this control is ready to receive mouse or keyboard input before the time-out; otherwise, false.</returns>
        bool WaitForControlReady();

        /// <summary>
        /// Blocks the current thread until this control is ready to receive mouse or keyboard input, or until the default time-out expires.
        /// </summary>
        /// <param name="timeout">The timeout for the wait.</param>
        /// <returns>true if this control is ready to receive mouse or keyboard input before the time-out; otherwise, false.</returns>
        bool WaitForControlReady(int timeout);

        /// <summary>
        /// Blocks the current thread until this control is ready to receive mouse or keyboard input, or until the default time-out expires.
        /// </summary>
        /// <param name="assert">Determines if the test should be marked as failed if the control is not ready after the wait.</param>
        /// <returns>true if this control is ready to receive mouse or keyboard input before the time-out; otherwise, false.</returns>
        bool WaitForControlReady(bool assert);

        /// <summary>
        /// Blocks the current thread until this control is ready to receive mouse or keyboard input, or until the default time-out expires.
        /// </summary>
        /// <param name="timeout">The timeout for the wait.</param>
        /// <param name="assert">Determines if the test should be marked as failed if the control is not ready after the wait.</param>
        /// <returns>true if this control is ready to receive mouse or keyboard input before the time-out; otherwise, false.</returns>
        bool WaitForControlReady(int timeout, bool assert);

        /// <summary>
        /// Searches through the search contexts until a context of the specified type is found.
        /// </summary>
        /// <typeparam name="T">The desired search context type.</typeparam>
        /// <param name="matchExactType">Determines wether the type has to match exactly.</param>
        /// <returns>Returns the wanted search context if one was found; otherwise null.</returns>
        T FindAncestorSearchContext<T>(bool matchExactType)
            where T : UITestControl;

        /// <summary>
        /// Reads the color at the specific relative point of the control from screen.
        /// </summary>
        /// <param name="point">The relative point to read the pixel from.</param>
        /// <returns>Returns the color that is displayed relative to this control.</returns>
        System.Drawing.Color GetColorFromPoint(Point point);

        /// <summary>
        /// Reads the color at the specific relative point of the control from screen.
        /// </summary>
        /// <param name="x">The relative x-coordinate to read the pixel from.</param>
        /// <param name="y">The relative y-coordinate to read the pixel from.</param>
        /// <returns>Returns the color that is displayed relative to this control.</returns>
        System.Drawing.Color GetColorFromPoint(int x, int y);
        #endregion
    }
}
