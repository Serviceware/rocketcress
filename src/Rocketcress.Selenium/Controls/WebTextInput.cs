using Rocketcress.Core;

namespace Rocketcress.Selenium.Controls;

/// <summary>
/// Represents a text-box element on a web page controlled by selenium.
/// </summary>
[GenerateUIMapParts]
public partial class WebTextInput : WebElement
{
#pragma warning disable SA1600 // Elements should be documented
    public WebTextInput(WebDriver driver, OpenQA.Selenium.By locationKey)
        : base(driver, locationKey)
    {
    }

    public WebTextInput(WebDriver driver, IWebElement element)
        : base(driver, element)
    {
    }

    public WebTextInput(WebDriver driver, OpenQA.Selenium.By locationKey, ISearchContext searchContext)
        : base(driver, locationKey, searchContext)
    {
    }

    protected WebTextInput(WebDriver driver)
        : base(driver)
    {
    }
#pragma warning restore SA1600 // Elements should be documented

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
                Retry.Until(
                    () =>
                    {
                        Clear();
                        SendKeys(value);
                        return string.Equals(Text?.Replace("\r", string.Empty), value.Replace("\r", string.Empty), StringComparison.Ordinal);
                    })
                    .WithMaxRetryCount(3)
                    .WithTimeGap(1000)
                    .OnError().Abort()
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

    /// <summary>
    /// Gets the wait entry point for this <see cref="WebTextInput" />.
    /// </summary>
    public new virtual WaitEntry Wait => (WaitEntry)base.Wait;

    /// <inheritdoc/>
    protected override WebElement.WaitEntry CreateWaitEntry()
    {
        return new WaitEntry(this);
    }

    /// <summary>
    /// Wait entry for the <see cref="WebTextInput"/> class.
    /// </summary>
    /// <seealso cref="Rocketcress.Selenium.Controls.WebElement.WaitEntry" />
    public new class WaitEntry : WebElement.WaitEntry
    {
        private readonly WebTextInput _element;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitEntry"/> class.
        /// </summary>
        /// <param name="element">The web element.</param>
        public WaitEntry(WebTextInput element)
            : base(element)
        {
            _element = element;
        }

        /// <summary>
        /// Gets a wait operation that waits until this element is read only.
        /// </summary>
        public virtual IWait<bool> UntilReadOnly
            => Until(OnCheckReadOnly).WithDefaultErrorMessage($"Element is not read only: {_element.GetSearchDescription()}");

        /// <summary>
        /// Gets a wait operation that waits until this element is not read only.
        /// </summary>
        public virtual IWait<bool> UntilNotReadOnly
            => Until(OnCheckNotReadOnly).WithDefaultErrorMessage($"Element is still read only: {_element.GetSearchDescription()}");

        /// <summary>
        /// Called when the <see cref="UntilReadOnly"/> wait operations checks whether to continue waiting or not.
        /// </summary>
        /// <returns>When <c>true</c> is returned, the wait operation is completed; otherwisem, the waiting continues.</returns>
        protected virtual bool OnCheckReadOnly() => _element.ReadOnly;

        /// <summary>
        /// Called when the <see cref="UntilNotReadOnly"/> wait operations checks whether to continue waiting or not.
        /// </summary>
        /// <returns>When <c>true</c> is returned, the wait operation is completed; otherwisem, the waiting continues.</returns>
        protected virtual bool OnCheckNotReadOnly() => !_element.ReadOnly;
    }
}
