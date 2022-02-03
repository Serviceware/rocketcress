using System.Collections.ObjectModel;

#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()

namespace Rocketcress.Selenium;

/// <summary>
/// Wrapper class for <see cref="OpenQA.Selenium.By"/>.
/// </summary>
/// <seealso cref="OpenQA.Selenium.By" />
public class By : OpenQA.Selenium.By
{
    /// <summary>
    /// Initializes a new instance of the <see cref="By"/> class.
    /// </summary>
    protected By()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="By"/> class.
    /// </summary>
    /// <param name="mechanism">The mechanism.</param>
    /// <param name="criteria">The criteria.</param>
    protected By(string mechanism, string criteria)
        : base(mechanism, criteria)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="By"/> class.
    /// </summary>
    /// <param name="findElementMethod">The find element method.</param>
    /// <param name="findElementsMethod">The find elements method.</param>
    protected By(Func<ISearchContext, IWebElement> findElementMethod, Func<ISearchContext, ReadOnlyCollection<IWebElement>> findElementsMethod)
        : base(findElementMethod, findElementsMethod)
    {
    }

    public static bool operator ==(By one, By two) => (object)one == two || (one != null && two != null && one.Equals(two));
    public static bool operator !=(By one, By two) => !(one == two);
    public static bool operator ==(By one, OpenQA.Selenium.By two) => (object)one == two || (one != null && two != null && one.Equals(two));
    public static bool operator !=(By one, OpenQA.Selenium.By two) => !(one == two);
    public static bool operator ==(OpenQA.Selenium.By one, By two) => (object)one == two || (one != null && two != null && one.Equals(two));
    public static bool operator !=(OpenQA.Selenium.By one, By two) => !(one == two);

    /// <summary>
    /// Creates a by object finding elements with a specified id.
    /// </summary>
    /// <param name="idToFind">The identifier to find.</param>
    /// <returns>The created by object.</returns>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "id is not hungarian notation.")]
    public static new By Id(string idToFind) => FromBy(OpenQA.Selenium.By.Id(idToFind));

    /// <summary>
    /// Creates a by object finding elements with a specified link text.
    /// </summary>
    /// <param name="linkTextToFind">The link text to find.</param>
    /// <returns>The created by object.</returns>
    public static new By LinkText(string linkTextToFind) => FromBy(OpenQA.Selenium.By.LinkText(linkTextToFind));

    /// <summary>
    /// Creates a by object finding elements with a specified name.
    /// </summary>
    /// <param name="nameToFind">The name to find.</param>
    /// <returns>The created by object.</returns>
    public static new By Name(string nameToFind) => FromBy(OpenQA.Selenium.By.Name(nameToFind));

    /// <summary>
    /// Creates a by object finding elements using a specified XPath.
    /// </summary>
    /// <param name="xpathToFind">The xpath to find.</param>
    /// <returns>The created by object.</returns>
    public static new By XPath(string xpathToFind) => FromBy(OpenQA.Selenium.By.XPath(xpathToFind));

    /// <summary>
    /// Creates a by object finding elements with a specified class name.
    /// </summary>
    /// <param name="classNameToFind">The class name to find.</param>
    /// <returns>The created by object.</returns>
    public static new By ClassName(string classNameToFind) => FromBy(OpenQA.Selenium.By.ClassName(classNameToFind));

    /// <summary>
    /// Creates a by object finding elements with a specified partial link text.
    /// </summary>
    /// <param name="partialLinkTextToFind">The partial link text to find.</param>
    /// <returns>The created by object.</returns>
    public static new By PartialLinkText(string partialLinkTextToFind) => FromBy(OpenQA.Selenium.By.PartialLinkText(partialLinkTextToFind));

    /// <summary>
    /// Creates a by object finding elements with a specified tag name.
    /// </summary>
    /// <param name="tagNameToFind">The tag name to find.</param>
    /// <returns>The created by object.</returns>
    public static new By TagName(string tagNameToFind) => FromBy(OpenQA.Selenium.By.TagName(tagNameToFind));

    /// <summary>
    /// Creates a by object finding elements using a specified CSS selector.
    /// </summary>
    /// <param name="cssSelectorToFind">The CSS selector to find.</param>
    /// <returns>The created by object.</returns>
    public static new By CssSelector(string cssSelectorToFind) => FromBy(OpenQA.Selenium.By.CssSelector(cssSelectorToFind));

    /// <summary>
    /// Creates a <see cref="By"/> using a <see cref="OpenQA.Selenium.By"/> object.
    /// </summary>
    /// <param name="by">The by object to wrap.</param>
    /// <returns>The created <see cref="By"/> object.</returns>
    public static By FromBy(OpenQA.Selenium.By by)
    {
        Guard.NotNull(by);
        By result;
        if (by.Mechanism != null && by.Criteria != null)
            result = new By(by.Mechanism, by.Criteria);
        else
            result = new By(by.FindElement, by.FindElements);
        result.Description = by.ToString();
        return result;
    }
}
