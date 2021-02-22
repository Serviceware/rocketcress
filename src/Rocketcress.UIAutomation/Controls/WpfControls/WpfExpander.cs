﻿using Rocketcress.UIAutomation.Exceptions;
using Rocketcress.Core;
using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.WpfControls
{
    [AutoDetectControl]
    public class WpfExpander : UITestControl, IUITestExpanderControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey
            .AndControlType(ControlType.Group)
            .AndPatternAvailable<ExpandCollapsePattern>();

        #region Patterns
        public ExpandCollapsePattern ExpandCollapsePattern => GetPattern<ExpandCollapsePattern>();
        public SynchronizedInputPattern SynchronizedInputPattern => GetPattern<SynchronizedInputPattern>();
        #endregion

        #region Construcotrs
        public WpfExpander(By locationKey) : base(locationKey) { }
        public WpfExpander(IUITestControl parent) : base(parent) { }
        public WpfExpander(AutomationElement element) : base(element) { }
        public WpfExpander(By locationKey, AutomationElement parent) : base(locationKey, parent) { }
        public WpfExpander(By locationKey, IUITestControl parent) : base(locationKey, parent) { }
        protected WpfExpander() { }
        #endregion

        #region Public Properties
        public virtual bool Expanded
        {
            get => ExpandCollapsePattern.Current.ExpandCollapseState == ExpandCollapseState.Expanded;
            set
            {
                if(Expanded != value)
                {
                    Click();
                    if(!Waiter.WaitUntil(() => value == Expanded, 5000, 0))
                    {
                        LogWarning("Expand state was not set correctly by clicking the control. The state is now set via the pattern.");
                        if (value)
                            ExpandCollapsePattern.Expand();
                        else
                            ExpandCollapsePattern.Collapse();
                        if (value != Expanded)
                            throw new UIAutomationControlException("The expand state has not been set correctly.", this);
                    }
                }
            }
        }
        #endregion
    }
}
