using Rocketcress.Core.Base;

namespace Rocketcress.Selenium.Base
{
    /// <summary>
    /// Base class for classes that contain <see cref="IWebElement"/>s.
    /// </summary>
    public abstract class WebElementContainer : TestObjectBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebElementContainer"/> class.
        /// </summary>
        /// <param name="initialize">Determines whether the <see cref="Initialize"/> method should be called in this constructor.</param>
        protected WebElementContainer(bool initialize)
        {
            if (initialize)
                Initialize();
        }

        /// <summary>
        /// Initializes this <see cref="WebElementContainer"/>.
        /// </summary>
        protected virtual void Initialize()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            InitializeControls();
#pragma warning restore CS0618 // Type or member is obsolete
        }

        /// <summary>
        /// Initializes all child controls of this element.
        /// </summary>
        [Obsolete("Use the Initialize() method instead.")]
        protected virtual void InitializeControls()
        {
        }
    }
}
