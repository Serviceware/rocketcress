using Rocketcress.UIAutomation.Controls;
using Rocketcress.UIAutomation.Models;

namespace Rocketcress.UIAutomation.Exceptions;

/// <summary>
/// Represents errors that occur when something related to UIAutomation element fails.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Exceptions.UIAutomationException" />
public class UIAutomationControlException : UIAutomationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UIAutomationControlException"/> class.
    /// </summary>
    public UIAutomationControlException()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIAutomationControlException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public UIAutomationControlException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIAutomationControlException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="locationKey">The location key.</param>
    public UIAutomationControlException(string message, IUITestControl parent, By locationKey)
        : this(message, parent, locationKey, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIAutomationControlException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="locationKey">The location key.</param>
    /// <param name="innerException">The inner exception.</param>
    public UIAutomationControlException(string message, IUITestControl parent, By locationKey, Exception innerException)
        : this(message, new UITestControl(parent?.Application, locationKey, parent), innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIAutomationControlException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="app">The application.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="locationKey">The location key.</param>
    public UIAutomationControlException(string message, Application app, AutomationElement parent, By locationKey)
        : this(message, app, parent, locationKey, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIAutomationControlException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="app">The application.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="locationKey">The location key.</param>
    /// <param name="innerException">The inner exception.</param>
    public UIAutomationControlException(string message, Application app, AutomationElement parent, By locationKey, Exception innerException)
        : this(message, new UITestControl(app, locationKey, parent), innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIAutomationControlException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="element">The element.</param>
    public UIAutomationControlException(string message, IUITestControl element)
        : this(message, element, (Exception)null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UIAutomationControlException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="element">The element.</param>
    /// <param name="innerException">The inner exception.</param>
    public UIAutomationControlException(string message, IUITestControl element, Exception innerException)
        : base(message?.TrimEnd('.') + $": {element?.GetSearchDescription(true)}", innerException)
    {
        ControlDescriptor = new UITestControlDescriptor(element);
    }

    /// <summary>
    /// Gets or sets the control descriptor.
    /// </summary>
    public UITestControlDescriptor ControlDescriptor { get; set; }
}
