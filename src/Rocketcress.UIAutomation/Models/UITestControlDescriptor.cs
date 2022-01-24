using Rocketcress.UIAutomation.Controls;

namespace Rocketcress.UIAutomation.Models;

/// <summary>
/// UIAutomation control descriptor.
/// </summary>
public class UITestControlDescriptor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UITestControlDescriptor"/> class.
    /// </summary>
    /// <param name="control">The control to describe.</param>
    public UITestControlDescriptor(IUITestControl control)
    {
        Parent = control.SearchContext == null ? null : new UITestControlDescriptor(control.SearchContext);
        LocationKey = control.LocationKey;
        AutomationElement = (control as UITestControl)?.GetCachedAutomationElement();
    }

    /// <summary>
    /// Gets or sets the parent control descriptor.
    /// </summary>
    public UITestControlDescriptor Parent { get; set; }

    /// <summary>
    /// Gets or sets the location key.
    /// </summary>
    public By LocationKey { get; set; }

    /// <summary>
    /// Gets or sets the automation element.
    /// </summary>
    public AutomationElement AutomationElement { get; set; }
}
