using OpenQA.Selenium;
using Rocketcress.Core;
using System;

namespace Rocketcress.Selenium.Controls
{
    /// <summary>
    /// Represents a text-box element on a web page controlled by selenium.
    /// </summary>
    public class WebTextInput : WebElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebTextInput"/> class as lazy element.
        /// </summary>
        /// <param name="locationKey">The location key.</param>
        public WebTextInput(By locationKey)
            : base(locationKey)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebTextInput"/> class as lazy element.
        /// </summary>
        /// <param name="locationKey">The location key.</param>
        /// <param name="searchContext">The search context.</param>
        public WebTextInput(By locationKey, ISearchContext searchContext)
            : base(locationKey, searchContext)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebTextInput"/> class as non-lazy element.
        /// </summary>
        /// <param name="element">The wrapped element.</param>
        public WebTextInput(IWebElement element)
            : base(element)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebTextInput"/> class.
        /// </summary>
        protected WebTextInput()
            : base()
        {
        }

        /// <summary>
        /// Gets or sets the text of the TextBox.
        /// </summary>
        public new string Text
        {
            get => GetAttribute("value");
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Retry.Until(() =>
                    {
                        Clear();
                        SendKeys(value);
                        return string.Equals(Text?.Replace("\r", string.Empty), value.Replace("\r", string.Empty), StringComparison.Ordinal);
                    }).WithMaxRetryCount(3)
                      .WithTimeGap(1000)
                      .OnError().Abort()
                      .ThrowOnFailure()
                      .Start();
                }
                else
                {
                    Clear();
                }
            }
        }

        /// <summary>
        /// Gets the maximum length for text in the TextBox.
        /// </summary>
        public int MaxLength
        {
            get
            {
                if (int.TryParse(GetAttribute("maxlength"), out int result))
                    return result;
                return int.MaxValue;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control is read only.
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return string.Equals(GetAttribute("readonly"), "readonly", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(GetAttribute("readonly"), "true", StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
