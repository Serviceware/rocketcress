namespace Rocketcress.UIAutomation.Controls;

/// <summary>
/// Represents a tab list UIAutomation control.
/// </summary>
/// <seealso cref="IUITestControl" />
/// <seealso cref="IUITestContainerControl" />
public interface IUITestTabListControl : IUITestControl, IUITestContainerControl
{
    /// <summary>
    /// Gets or sets the index of the selected tab.
    /// </summary>
    int SelectedIndex { get; set; }

    /// <summary>
    /// Gets the tabs.
    /// </summary>
    IEnumerable<IUITestControl> Tabs { get; }
}

/// <summary>
/// Represents a tab list UIAutomation control.
/// </summary>
/// <typeparam name="TPage">The type of the page.</typeparam>
/// <seealso cref="IUITestTabListControl" />
/// <seealso cref="IUITestContainerControl{TPage}" />
public interface IUITestTabListControl<TPage> : IUITestTabListControl, IUITestContainerControl<TPage>
    where TPage : IUITestTabPageControl
{
    /// <summary>
    /// Gets the tabs.
    /// </summary>
    new IEnumerable<TPage> Tabs { get; }
}
