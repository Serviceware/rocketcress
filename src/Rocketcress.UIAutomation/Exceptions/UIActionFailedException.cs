using Rocketcress.UIAutomation.Controls;

namespace Rocketcress.UIAutomation.Exceptions;

/// <summary>
/// Represents errors that occur when actions on UIAutomation elements fail.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Exceptions.UIAutomationControlException" />
public class UIActionFailedException : UIAutomationControlException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UIActionFailedException"/> class.
    /// </summary>
    public UIActionFailedException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIActionFailedException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public UIActionFailedException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIActionFailedException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="element">The element.</param>
    public UIActionFailedException(string message, IUITestControl element)
        : base(message, element)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIActionFailedException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="locationKey">The location key.</param>
    public UIActionFailedException(string message, IUITestControl parent, By locationKey)
        : base(message, parent, locationKey)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIActionFailedException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="element">The element.</param>
    /// <param name="innerException">The inner exception.</param>
    public UIActionFailedException(string message, IUITestControl element, Exception innerException)
        : base(message, element, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIActionFailedException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="locationKey">The location key.</param>
    /// <param name="innerException">The inner exception.</param>
    public UIActionFailedException(string message, IUITestControl parent, By locationKey, Exception innerException)
        : base(message, parent, locationKey, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIActionFailedException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="app">The application.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="locationKey">The location key.</param>
    public UIActionFailedException(string message, Application app, AutomationElement parent, By locationKey)
        : base(message, app, parent, locationKey)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIActionFailedException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="app">The application.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="locationKey">The location key.</param>
    /// <param name="innerException">The inner exception.</param>
    public UIActionFailedException(string message, Application app, AutomationElement parent, By locationKey, Exception innerException)
        : base(message, app, parent, locationKey, innerException)
    {
    }
}
