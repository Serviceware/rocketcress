using System.Collections.Generic;

namespace Rocketcress.UIAutomation.Controls
{
    public interface IUITestListControl : IUITestControl
    {
        string[] SelectedItems { get; set; }
        int[] SelectedIndices { get; set; }
        IEnumerable<IUITestControl> Items { get; }
        bool IsMultiSelection { get; }
    }

    public interface IUITestListControl<TListItem> : IUITestListControl where TListItem : IUITestListItemControl
    {
        new IEnumerable<TListItem> Items { get; }
    }
}
