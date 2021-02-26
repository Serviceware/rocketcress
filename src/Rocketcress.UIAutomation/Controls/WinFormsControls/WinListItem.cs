using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    public class WinListItem : WinControl, IUITestListItemControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ListItem);

        #region Patterns
        public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();
        public ScrollItemPattern ScrollItemPattern => GetPattern<ScrollItemPattern>();
        public VirtualizedItemPattern VirtualizedItemPattern => GetPattern<VirtualizedItemPattern>();
        #endregion

        #region Constructors
        public WinListItem(By locationKey)
            : base(locationKey)
        {
        }

        public WinListItem(IUITestControl parent)
            : base(parent)
        {
        }

        public WinListItem(AutomationElement element)
            : base(element)
        {
        }

        public WinListItem(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WinListItem(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WinListItem()
        {
        }
        #endregion

        #region Public Properties
        public virtual bool Selected
        {
            get => SelectionItemPattern.Current.IsSelected;
            set
            {
                if (value)
                    SelectionItemPattern.AddToSelection();
                else
                    SelectionItemPattern.RemoveFromSelection();
            }
        }

        public virtual string DisplayText => Name;
        #endregion

        #region Public Methods
        public virtual void Select()
        {
            if (TryGetPattern<VirtualizedItemPattern>(out var virtualizedItemPattern))
                virtualizedItemPattern.Realize();
            if (TryGetPattern<ScrollItemPattern>(out var scrollItemPattern))
                scrollItemPattern.ScrollIntoView();
            SelectionItemPattern.Select();
        }
        #endregion
    }
}
