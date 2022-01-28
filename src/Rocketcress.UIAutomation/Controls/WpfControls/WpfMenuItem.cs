using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation menu item control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestMenuItemControl" />
[AutoDetectControl]
[GenerateUIMapParts(IdStyle = IdStyle.Disabled)]
public partial class WpfMenuItem : WpfControl, IUITestMenuItemControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.MenuItem);

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Base Location Key should be on top.")]
    private ToggleControlSupport _toggleControlSupport;
    private MenuControlSupport _menuControlSupport;
    private ExpandCollapseControlSupport _expandCollapseControlSupport;

    /// <summary>
    /// Gets the toggle pattern.
    /// </summary>
    public TogglePattern TogglePattern => GetPattern<TogglePattern>();

    /// <summary>
    /// Gets the expand collapse pattern.
    /// </summary>
    public ExpandCollapsePattern ExpandCollapsePattern => GetPattern<ExpandCollapsePattern>();

    /// <inheritdoc/>
    public virtual string Header => HeaderControl.DisplayText;

    /// <inheritdoc/>
    public virtual bool Checked
    {
        get => _toggleControlSupport.GetChecked();
        set => _toggleControlSupport.SetChecked(value);
    }

    /// <inheritdoc/>
    public virtual bool Expanded
    {
        get => _expandCollapseControlSupport.GetExpanded();
        set => _expandCollapseControlSupport.SetExpanded(value);
    }

    /// <inheritdoc/>
    public virtual IEnumerable<IUITestControl> Items => _menuControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

    /// <inheritdoc/>
    public bool Indeterminate => TogglePattern.Current.ToggleState == ToggleState.Indeterminate;

    /// <summary>
    /// Gets or sets the header control.
    /// </summary>
    [UIMapControl]
    protected virtual WpfText HeaderControl { get; set; }

    partial void OnInitialized()
    {
        _toggleControlSupport = new ToggleControlSupport(this);
        _menuControlSupport = new MenuControlSupport(this, By.Framework(FrameworkIds.Wpf).AndControlType(ControlType.MenuItem), ControlType.Menu, ControlType.MenuItem);
        _expandCollapseControlSupport = new ExpandCollapseControlSupport(this);
    }
}
