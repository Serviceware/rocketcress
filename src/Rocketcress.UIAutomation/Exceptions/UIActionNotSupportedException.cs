using Rocketcress.UIAutomation.Controls;

namespace Rocketcress.UIAutomation.Exceptions;

/// <summary>
/// Represents errors that occur when actions on UIAutomation elements have been executed that the element does not support.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Exceptions.UIAutomationControlException" />
public class UIActionNotSupportedException : UIAutomationControlException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UIActionNotSupportedException"/> class.
    /// </summary>
    public UIActionNotSupportedException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIActionNotSupportedException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public UIActionNotSupportedException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIActionNotSupportedException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="element">The element.</param>
    public UIActionNotSupportedException(string message, UITestControl element)
        : base(message, element)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIActionNotSupportedException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="locationKey">The location key.</param>
    public UIActionNotSupportedException(string message, UITestControl parent, By locationKey)
        : base(message, parent, locationKey)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIActionNotSupportedException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="app">The application.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="locationKey">The location key.</param>
    public UIActionNotSupportedException(string message, Application app, AutomationElement parent, By locationKey)
        : base(message, app, parent, locationKey)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIActionNotSupportedException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="element">The element.</param>
    /// <param name="innerException">The inner exception.</param>
    public UIActionNotSupportedException(string message, UITestControl element, Exception innerException)
        : base(message, element, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIActionNotSupportedException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="locationKey">The location key.</param>
    /// <param name="innerException">The inner exception.</param>
    public UIActionNotSupportedException(string message, UITestControl parent, By locationKey, Exception innerException)
        : base(message, parent, locationKey, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIActionNotSupportedException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="app">The application.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="locationKey">The location key.</param>
    /// <param name="innerException">The inner exception.</param>
    public UIActionNotSupportedException(string message, Application app, AutomationElement parent, By locationKey, Exception innerException)
        : base(message, app, parent, locationKey, innerException)
    {
    }
}
