using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using Rocketcress.UIAutomation.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfComboBox : WpfControl, IUITestComboBoxControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ComboBox);

        #region Private Fields
        private static readonly By ByItem = By.Scope(TreeScope.Children).AndFramework(FrameworkIds.Wpf).AndControlType(ControlType.ListItem);
        private ValueControlSupport _valueControlSupport;
        private ListControlSupport _listControlSupport;
        #endregion

        #region Patterns
        public ExpandCollapsePattern ExpandCollapsePattern => GetPattern<ExpandCollapsePattern>();
        public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();
        public ScrollPattern ScrollPattern => GetPattern<ScrollPattern>();
        public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();
        public ValuePattern ValuePattern => GetPattern<ValuePattern>();
        #endregion

        #region Constructors
        public WpfComboBox(By locationKey)
            : base(locationKey)
        {
        }

        public WpfComboBox(IUITestControl parent)
            : base(parent)
        {
        }

        public WpfComboBox(AutomationElement element)
            : base(element)
        {
        }

        public WpfComboBox(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WpfComboBox(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WpfComboBox()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            _valueControlSupport = new ValueControlSupport(this);
            _listControlSupport = new ListControlSupport(this, ByItem);
        }
        #endregion

        #region Public Properties
        public virtual string SelectedItem
        {
            get => _listControlSupport.GetSelectedItems().FirstOrDefault();
            set => _listControlSupport.SetSelectedItem(value);
        }

        public virtual int SelectedIndex
        {
            get
            {
                var result = _listControlSupport.GetSelectedIndices().FirstOrDefault();
                Expanded = false;
                return result;
            }
            set => _listControlSupport.SetSelectedIndex(value);
        }

        public virtual string Text
        {
            get => TryGetPattern<ValuePattern>(out var valuePattern) ? valuePattern.Current.Value : SelectedItem;
            set
            {
                if (!IsPatternAvailable<ValuePattern>())
                    throw new UIActionNotSupportedException("This combo box does not support direct text input. Use the SelectedItem property instead.", this);
                _valueControlSupport.SetValue(value);
            }
        }

        public virtual bool Expanded
        {
            get => ExpandCollapsePattern.Current.ExpandCollapseState == ExpandCollapseState.Expanded;
            set => (value ? (Action)ExpandCollapsePattern.Expand : ExpandCollapsePattern.Collapse)();
        }

        public virtual IEnumerable<IUITestControl> Items => _listControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(x));
        #endregion
    }
}
