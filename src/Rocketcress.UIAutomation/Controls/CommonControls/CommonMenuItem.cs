using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.CommonControls;

/// <summary>
/// Represents a common menu item control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.UITestControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestMenuItemControl" />
[AutoDetectControl(Priority = -50)]
[GenerateUIMapParts]
public partial class CommonMenuItem : UITestControl, IUITestMenuItemControl
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
    /// Gets the expand/collapse pattern.
    /// </summary>
    public ExpandCollapsePattern ExpandCollapsePattern => GetPattern<ExpandCollapsePattern>();

    /// <inheritdoc/>
    public virtual string Header => HeaderControl.DisplayText;

    /// <inheritdoc/>
    public virtual bool Checked
    {
        get => _toggleControlSupport.GetChecked();
        set => _toggleControlSupport.SetChecked(value, true);
    }

    /// <inheritdoc/>
    public virtual bool Expanded
    {
        get => _expandCollapseControlSupport.GetExpanded();
        set => _expandCollapseControlSupport.SetExpanded(value);
    }

    /// <inheritdoc/>
    public virtual IEnumerable<IUITestControl> Items => _menuControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

    /// <summary>
    /// Gets or sets the header control.
    /// </summary>
    [UIMapControl(IdStyle = IdStyle.Disabled)]
    protected virtual CommonText HeaderControl { get; set; }

    partial void OnInitialized()
    {
        _toggleControlSupport = new ToggleControlSupport(this);
        _menuControlSupport = new MenuControlSupport(this, By.ControlType(ControlType.MenuItem), ControlType.Menu, ControlType.MenuItem);
        _expandCollapseControlSupport = new ExpandCollapseControlSupport(this);
    }
}
