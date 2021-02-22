using Rocketcress.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Rocketcress.Selenium
{
    /// <summary>
    /// Tells MSTest which browser(s) to use for the test this attribute is applied to.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class BrowserDataSourceAttribute : Attribute, ITestDataSource
    {
        private readonly (Browser browser, CultureInfo language)[] _browsers;

        /// <summary>
        /// Initializes a new Instance of the <see cref="BrowserDataSourceAttribute"/> class.
        /// </summary>
        /// <param name="browsers">Determine the browsers on which to execute the test. Seperate multiple browsers by a ';'. You can also specify the language by adding @[lang] (e.g. Chrome@en-US) - default is en-US.</param>
        public BrowserDataSourceAttribute(string browsers)
        {
            if (string.IsNullOrWhiteSpace(browsers))
                throw new ArgumentException("The browsers parameter can not be null or white space.", nameof(browsers));
            _browsers = browsers.Split(';')
                .Select(x =>
                {
                    var split = x.Split('@');
                    if (!Enum.TryParse<Browser>(split[0], out var browser))
                        throw new InvalidOperationException($"The browser \"{split[0]}\" is unknown.");
                    CultureInfo language;
                    if (split.Length >= 2)
                    {
                        try { language = CultureInfo.GetCultureInfo(split[1]); }
                        catch (CultureNotFoundException ex) { throw new InvalidOperationException($"The culture {split[1]} is unknown.", ex); }
                    }
                    else
                        language = CultureInfo.GetCultureInfo("en-US");


                    return (browser, language);
                })
                .ToArray();
        }

        /// <inheritdoc />
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            foreach (var (browser, language) in _browsers)
            {
                SeleniumTestContext.CurrentBrowser = browser;
                SeleniumTestContext.CurrentBrowserLanguage = language;
                yield return Array.Empty<object>();
            }
        }

        /// <inheritdoc />
        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            if (data != null)
                return $"{SeleniumTestContext.CurrentBrowser} - {SeleniumTestContext.CurrentBrowserLanguage.Name}";
            return null;
        }
    }
}
