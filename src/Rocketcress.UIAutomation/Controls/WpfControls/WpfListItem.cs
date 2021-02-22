using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfListItem : WpfControl, IUITestListItemControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ListItem);

        #region Patterns
        public SelectionItemPattern SelectionItemPattern => GetPattern<SelectionItemPattern>();
        public ScrollItemPattern ScrollItemPattern => GetPattern<ScrollItemPattern>();
        public VirtualizedItemPattern VirtualizedItemPattern => GetPattern<VirtualizedItemPattern>();
        #endregion

        #region Constructors
        public WpfListItem(By locationKey) : base(locationKey) { }
        public WpfListItem(IUITestControl parent) : base(parent) { }
        public WpfListItem(AutomationElement element) : base(element) { }
        public WpfListItem(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WpfListItem(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WpfListItem() { }
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
        public virtual string ActualDisplayName => InnertText.DisplayText;
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

        private static readonly By ByInnerText = By.ClassName("TextBlock");
        public WpfText InnertText { get { return new WpfText(ByInnerText, this); } }
    }
}
