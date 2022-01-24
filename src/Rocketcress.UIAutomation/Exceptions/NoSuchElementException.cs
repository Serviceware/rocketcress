using Rocketcress.UIAutomation.Controls;

namespace Rocketcress.UIAutomation.Exceptions;

/// <summary>
/// Represents errors that occur when a UIAutomation element could not be found.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Exceptions.UIAutomationControlException" />
public class NoSuchElementException : UIAutomationControlException
{
    private const string DefaultMessage = "No Element was found with the given search properties";

    /// <summary>
    /// Initializes a new instance of the <see cref="NoSuchElementException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public NoSuchElementException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NoSuchElementException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="element">The element.</param>
    public NoSuchElementException(string message, UITestControl element)
        : base(message, element)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NoSuchElementException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="locationKey">The location key.</param>
    public NoSuchElementException(string message, UITestControl parent, By locationKey)
        : base(message, parent, locationKey)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NoSuchElementException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="app">The application.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="locationKey">The location key.</param>
    public NoSuchElementException(string message, Application app, AutomationElement parent, By locationKey)
        : base(message, app, parent, locationKey)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NoSuchElementException"/> class.
    /// </summary>
    /// <param name="element">The element.</param>
    public NoSuchElementException(UITestControl element)
        : base(DefaultMessage, element)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NoSuchElementException"/> class.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="locationKey">The location key.</param>
    public NoSuchElementException(UITestControl parent, By locationKey)
        : base(DefaultMessage, parent, locationKey)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NoSuchElementException"/> class.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="locationKey">The location key.</param>
    public NoSuchElementException(Application app, AutomationElement parent, By locationKey)
        : base(DefaultMessage, app, parent, locationKey)
    {
    }
}
