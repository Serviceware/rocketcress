using Rocketcress.Core;
using Rocketcress.UIAutomation.Controls.WpfControls;
using Rocketcress.UIAutomation.ControlSearch;
using Rocketcress.UIAutomation.Exceptions;

namespace Rocketcress.UIAutomation.Controls.ControlSupport
{
    public class ListControlSupport
    {
        private readonly UITestControl _control;
        private readonly By _listItemDefinition;

        public ListControlSupport(UITestControl control, By listItemDefinition)
        {
            _control = control;
            _listItemDefinition = listItemDefinition;
        }

        public IEnumerable<int> GetSelectedIndices()
        {
            if (!_control.TryGetPattern<SelectionPattern>(out var selectionPattern))
                throw new UIActionNotSupportedException("To get the selected index, the control needs to support the SelectionPattern.", _control);

            var selectedElements = new LinkedList<string>(selectionPattern.Current.GetSelection().Select(x => string.Join(", ", x.GetRuntimeId())));
            int index = 0;
            foreach (var element in EnumerateItems())
            {
                var runtimeId = string.Join(", ", element.GetRuntimeId());
                var node = selectedElements.Find(runtimeId);
                if (node != null)
                {
                    selectedElements.Remove(node);
                    yield return index;
                    if (selectedElements.Count == 0)
                        break;
                }

                index++;
            }
        }

        public void SetSelectedIndex(int value)
        {
            _control.TryGetPattern<ExpandCollapsePattern>(out var expand);

            expand?.Expand();

            var element = EnumerateItems().ElementAt(value);
            new WpfListItem(_control.Application, element).Select();

            expand?.Collapse();
        }

        public void SetSelectedIndices(int[] value)
        {
            if (value.Length == 1)
            {
                SetSelectedIndex(value[0]);
                return;
            }

            if (!_control.TryGetPattern<SelectionPattern>(out var selectionPattern))
                throw new UIActionNotSupportedException("To set the selected indices, the control needs to support the SelectionPattern.", _control);
            if (!selectionPattern.Current.CanSelectMultiple)
                throw new UIActionNotSupportedException("You can not select multiple items in this control.", _control);

            _control.TryGetPattern<ExpandCollapsePattern>(out var expand);

            foreach (var element in selectionPattern.Current.GetSelection())
                ((SelectionItemPattern)element.GetCurrentPattern(SelectionItemPattern.Pattern)).RemoveFromSelection();

            var index = 0;
            var count = 0;
            foreach (var element in EnumerateItems())
            {
                if (value.Contains(index++))
                {
                    ((SelectionItemPattern)element.GetCurrentPattern(SelectionItemPattern.Pattern)).AddToSelection();
                    if (++count >= value.Length)
                        break;
                }
            }

            if (value.Any(x => x >= index || x < 0))
                throw new UIAutomationControlException($"One or more indizes are out of range. Valid range is from 0 to {index - 1}.", _control);

            expand?.Collapse();
        }

        public IEnumerable<string> GetSelectedItems()
        {
            if (!_control.TryGetPattern<SelectionPattern>(out var selectionPattern))
                throw new UIActionNotSupportedException("To get the selected index, the control needs to support the SelectionPattern.", _control);

            return selectionPattern.Current.GetSelection().Select(x => x.Current.Name);
        }

        public void SetSelectedItem(string value)
        {
            if (!_control.TryGetPattern<ItemContainerPattern>(out var itemContainerPattern))
                throw new UIActionNotSupportedException("To set the selected item, the control needs to support the ItemContainerPattern.", _control);
            _control.TryGetPattern<ExpandCollapsePattern>(out var expand);

            expand?.Expand();

            var item = itemContainerPattern.FindItemByProperty(null, AutomationElement.NameProperty, value);
            if (item == null)
                throw new NoSuchElementException($"An element with name \"{value}\" was not found in the combo box items.");
            var listItem = new WpfListItem(_control.Application, item);
            listItem.Select();

            expand?.Collapse();
        }

        public void SetSelectedItems(ICollection<string> value)
        {
            if (value.Count == 1)
            {
                SetSelectedItem(value.First());
                return;
            }

            if (!_control.TryGetPattern<SelectionPattern>(out var selectionPattern))
                throw new UIActionNotSupportedException("To get the selected index, the control needs to support the SelectionPattern.", _control);
            if (!selectionPattern.Current.CanSelectMultiple)
                throw new UIActionNotSupportedException("You can not select multiple items in this control.", _control);
            _control.TryGetPattern<ExpandCollapsePattern>(out var expand);

            expand?.Expand();

            foreach (var element in selectionPattern.Current.GetSelection())
                ((SelectionItemPattern)element.GetCurrentPattern(SelectionItemPattern.Pattern)).RemoveFromSelection();

            var items = new LinkedList<string>(value);
            foreach (var element in EnumerateItems())
            {
                var node = items.Find(element.Current.Name);
                if (node != null)
                {
                    ((SelectionItemPattern)element.GetCurrentPattern(SelectionItemPattern.Pattern)).AddToSelection();
                    items.Remove(node);
                    if (items.Count == 0)
                        break;
                }
            }

            if (items.Count > 0)
                throw new UIAutomationControlException("The following elements were not found in the list: " + string.Join(", ", items), _control);

            expand?.Collapse();
        }

        public IEnumerable<AutomationElement> EnumerateItems()
        {
            _control.TryGetPattern<ScrollPattern>(out var scroll);
            _control.TryGetPattern<ExpandCollapsePattern>(out var expand);
            var canScroll = scroll != null && scroll.Current.VerticallyScrollable;

            var prevExpandState = expand?.Current.ExpandCollapseState;
            expand?.Expand();

            if (Wait.Until(() => SearchEngine.FindFirst(_listItemDefinition, _control.AutomationElement) != null).WithTimeout(UITestControl.LongControlActionTimeout).WithTimeGap(0).Start().Value)
            {
                if (canScroll)
                {
                    foreach (var element in EnumerateItemsWithScroll(scroll))
                        yield return element;
                }
                else
                {
                    foreach (var element in SearchEngine.FindAll(_listItemDefinition, _control.AutomationElement))
                        yield return element;
                }
            }

            if (prevExpandState == ExpandCollapseState.Collapsed)
                expand?.Collapse();
        }

        private IEnumerable<AutomationElement> EnumerateItemsWithScroll(ScrollPattern scroll)
        {
            bool isFirst = true;
            var touchedElements = new HashSet<string>();
            scroll.SetScrollPercent(scroll.Current.HorizontallyScrollable ? 0 : ScrollPattern.NoScroll, 0);
            while (scroll.Current.VerticalScrollPercent < 100)
            {
                if (!isFirst)
                    scroll.ScrollVertical(ScrollAmount.LargeIncrement);
                foreach (var element in SearchEngine.FindAll(_listItemDefinition, _control.AutomationElement))
                {
                    var runtimeId = string.Join(", ", element.GetRuntimeId());
                    if (!touchedElements.Contains(runtimeId))
                    {
                        touchedElements.Add(runtimeId);
                        yield return element;
                    }
                }

                isFirst = false;
            }
        }
    }
}
