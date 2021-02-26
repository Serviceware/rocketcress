using System.Windows.Automation;

namespace Rocketcress.UIAutomation.Controls.Win32Controls
{
    [AutoDetectControl]
    public class Win32Text : Win32Control, IUITestTextControl
    {
        protected override By BaseLocationKey => base.BaseLocationKey.AndControlType(ControlType.Text);

        #region Construcotrs
        public Win32Text(By locationKey)
            : base(locationKey)
        {
        }

        public Win32Text(IUITestControl parent)
            : base(parent)
        {
        }

        public Win32Text(AutomationElement element)
            : base(element)
        {
        }

        public Win32Text(By locationKey, AutomationElement parent)
            : base(locationKey, parent)
        {
        }

        public Win32Text(By locationKey, IUITestControl parent)
            : base(locationKey, parent)
        {
        }

        protected Win32Text()
        {
        }
        #endregion

        #region Public Properties
        public virtual string DisplayText => Name;
        public virtual string Text => Name;
        #endregion
    }
}
