using OpenQA.Selenium;
using WebElement = Rocketcress.Selenium.Controls.WebElement;

namespace Rocketcress.Selenium.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="IWebElement"/> interface.
    /// </summary>
    public static class WebElementExtensions
    {
        /// <summary>
        /// Creates a <see cref="WebElement"/>-Object as wrapper for the current <see cref="IWebElement"/>.
        /// If the current <see cref="IWebElement"/> is a <see cref="WebElement"/> this will be returned.
        /// </summary>
        /// <param name="element">The element to extend.</param>
        /// <returns>A <see cref="WebElement"/> that is a wrapper for the <see cref="IWebElement"/>.</returns>
        public static WebElement Extend(this IWebElement element)
        {
            if (element is not WebElement result)
                result = new WebElement(element);
            return result;
        }

        /// <summary>
        /// Gets the parent element.
        /// </summary>
        /// <param name="element">The element to get the parent from.</param>
        /// <returns>Returns the parent from the element.</returns>
        public static IWebElement GetParent(this IWebElement element)
        {
            return element.FindElement(By.XPath(".."));
        }
    }
}
