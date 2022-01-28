using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using System.Windows;

namespace Rocketcress.UIAutomation.Controls.WpfControls;

/// <summary>
/// Represents a Windows Presentation Foundation tree item control.
/// </summary>
/// <seealso cref="Rocketcress.UIAutomation.Controls.WpfControls.WpfControl" />
/// <seealso cref="Rocketcress.UIAutomation.Controls.IUITestTreeItemControl" />
[AutoDetectControl]
[GenerateUIMapParts]
public partial class WpfTreeItem : WpfControl, IUITestTreeItemControl
{
    /// <inheritdoc/>
    protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.TreeItem);

    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Base Location Key should be on top.")]
    private static readonly By ByItem = By.Framework(FrameworkIds.Wpf).AndControlType(ControlType.TreeItem);
    private MenuControlSupport _menuControlSupport;
    private SelectionItemControlSupport _selectionItemControlSupport;

    /// <summary>
    /// Gets the expand collapse pattern.
    /// </summary>
    public ExpandCollapsePattern ExpandCollapsePattern => GetPattern<ExpandCollapsePattern>();

    /// <summary>
    /// Gets the item container pattern.
    /// </summary>
    public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();

    /// <summary>
    /// Gets the scroll item pattern.
    /// </summary>
    public ScrollItemPattern ScrollItemPattern => GetPattern<ScrollItemPattern>();

    /// <summary>
    /// Gets the selection item pattern.
    /// </summary>
    public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();

    /// <inheritdoc/>
    public override Point ClickablePoint => HeaderControl.Exists ? HeaderControl.ClickablePoint : base.ClickablePoint;

    /// <inheritdoc/>
    public virtual bool Expanded
    {
        get => ExpandCollapsePattern.Current.ExpandCollapseState == ExpandCollapseState.Expanded;
        set => (value ? (Action)ExpandCollapsePattern.Expand : ExpandCollapsePattern.Collapse)();
    }

    /// <inheritdoc/>
    public virtual bool HasChildNodes => ExpandCollapsePattern.Current.ExpandCollapseState != ExpandCollapseState.LeafNode;

    /// <inheritdoc/>
    public virtual string Header => HeaderControl.DisplayText;

    /// <inheritdoc/>
    public virtual IEnumerable<IUITestControl> Nodes => _menuControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(Application, x));

    /// <inheritdoc/>
    public virtual IUITestControl ParentNode => FindElements(By.Scope(TreeScope.Ancestors).Append(ByItem, false, false)).FirstOrDefault();

    /// <inheritdoc/>
    public virtual bool Selected
    {
        get => _selectionItemControlSupport.GetSelected();
        set => _selectionItemControlSupport.SetSelected(value);
    }

    /// <inheritdoc/>
    IEnumerable<IUITestControl> IUITestContainerControl.Items => Nodes;

    /// <summary>
    /// Gets or sets the header control.
    /// </summary>
    [UIMapControl(IdStyle = IdStyle.Disabled)]
    protected WpfText HeaderControl { get; set; }

    partial void OnInitialized()
    {
        _menuControlSupport = new MenuControlSupport(this, ByItem, ControlType.Tree, ControlType.TreeItem);
        _selectionItemControlSupport = new SelectionItemControlSupport(this);
    }
}
