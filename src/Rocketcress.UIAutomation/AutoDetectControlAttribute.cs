namespace Rocketcress.UIAutomation
{
    /// <summary>
    /// This attribute tells the framework to automatically detect controls of the applied class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AutoDetectControlAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the priority of this control class.
        /// </summary>
        public int Priority { get; set; }
    }
}
