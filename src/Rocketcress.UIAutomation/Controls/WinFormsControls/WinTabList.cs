using Rocketcress.Core.Extensions;
using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls;

/// <summary>
/// Represents a Windows Forms tab list control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WinFormsControls.WinControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestTabListControl{TItem}" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WinTabList : WinControl, IUITestTabListControl<WinTabPage>
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Tab);

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Base Location Key should be on top.")]
    private ListControlSupport _listControlSupport;

    /// <summary>
    /// Gets the selection pattern.
    /// </summary>
    public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();

    /// <inheritdoc/>
    public virtual int SelectedIndex
    {
        get => _listControlSupport.GetSelectedIndices().TryFirst(out var index) ? index : -1;
        set => _listControlSupport.SetSelectedIndex(value);
    }

    /// <inheritdoc/>
    public IEnumerable<WinTabPage> Tabs => ((IUITestTabListControl)this).Tabs.OfType<WinTabPage>();

    /// <inheritdoc/>
    IEnumerable<IUITestControl> IUITestTabListControl.Tabs => _listControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

    /// <inheritdoc/>
    IEnumerable<WinTabPage> IUITestContainerControl<WinTabPage>.Items => Tabs;

    /// <inheritdoc/>
    IEnumerable<IUITestControl> IUITestContainerControl.Items => ((IUITestTabListControl)this).Tabs;

    partial void OnInitialized()
    {
        _listControlSupport = new ListControlSupport(this, By.Framework(FrameworkIds.WindowsForms).AndControlType(ControlType.TabItem));
    }
}
