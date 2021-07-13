using Rocketcress.Core;
using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Controls.CommonControls;
using Rocketcress.UIAutomation.Controls.ControlSupport;
using Rocketcress.UIAutomation.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WinFormsControls
{
    [AutoDetectControl]
    public class WinComboBox : WinControl, IUITestComboBoxControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.ComboBox);

        #region Private Fields
        private static readonly By ByItem = By.Scope(TreeScope.Children).AndControlType(ControlType.ListItem);
        private ValueControlSupport _valueControlSupport;
        private ListControlSupport _listControlSupport;
        private UITestControl _listControl;
        private CommonButton _expandButton;
        #endregion

        #region Patterns
        public ExpandCollapsePattern ExpandCollapsePattern => GetPattern<ExpandCollapsePattern>();
        public ItemContainerPattern ItemContainerPattern => GetPattern<ItemContainerPattern>();
        public ScrollPattern ScrollPattern => GetPattern<ScrollPattern>();
        public SelectionPattern SelectionPattern => GetPattern<SelectionPattern>();
        public ValuePattern ValuePattern => GetPattern<ValuePattern>();
        #endregion

        #region Constructors
        public WinComboBox(By locationKey)
            : base(locationKey)
        {
        }

        public WinComboBox(IUITestControl parent)
            : base(parent)
        {
        }

        public WinComboBox(AutomationElement element)
            : base(element)
        {
        }

        public WinComboBox(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public WinComboBox(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected WinComboBox()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            _listControl = new UITestControl(By.ControlType(ControlType.List), this);
            _expandButton = new CommonButton(By.Empty, this);

            _valueControlSupport = new ValueControlSupport(this);
            _listControlSupport = new ListControlSupport(_listControl, ByItem);
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
                Expanded = true;
                var result = _listControlSupport.GetSelectedIndices().FirstOrDefault();
                Expanded = false;
                return result;
            }
            set
            {
                Expanded = true;
                var element = new UITestControl(_listControlSupport.EnumerateItems().ElementAt(value));
                element.Click();
            }
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
            get => _listControl.Displayed;
            set
            {
                if (_listControl.Displayed != value)
                {
                    _expandButton.Click();
                    Wait.Until(() => _listControl.Displayed == value).WithTimeout(2000).Start();
                }
            }
        }

        public virtual IEnumerable<IUITestControl> Items => _listControlSupport.EnumerateItems().Select(x => ControlUtility.GetControl(x));
        #endregion
    }
}
