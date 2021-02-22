using Rocketcress.Core;
using Rocketcress.Core.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Drawing;
using System.Globalization;

namespace Rocketcress.Selenium
{
    /// <summary>
    /// Represents a class that contains settings that are used for Selenium Tests.
    /// </summary>
    public class Settings : SettingsBase
    {
        private Browser? _currentBrowser;
        private CultureInfo _currentBrowserLanguage;
        private bool? _killAllBrowserProcessesOnCleanup;

        /// <summary>
        /// Gets or sets the default timeout for the wait operations in the Wait-Driver.
        /// </summary>
        public override TimeSpan Timeout
        {
            get => base.Timeout;
            set
            {
                base.Timeout = value;
                if (SeleniumTestContext.CurrentContext != null && SeleniumTestContext.CurrentContext.Driver != null)
                {
                    SeleniumTestContext.CurrentContext.Driver.Manage().Timeouts().PageLoad = value;
                    SeleniumTestContext.CurrentContext.Driver.Manage().Timeouts().AsynchronousJavaScript = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the URL to the login page of the tested application.
        /// </summary>
        public virtual string LoginUrl { get; set; }
        
        /// <summary>
        /// Gets or sets the resolution that the browser should be opened in.
        /// </summary>
        public virtual Size Resolution { get; set; }

        /// <summary>
        /// Gets or sets the default browser to use if no data source is given for a test.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual Browser DefaultBrowser { get; set; }

        /// <summary>
        /// Gets or sets the current browser.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual Browser CurrentBrowser
        {
            get => _currentBrowser ?? DefaultBrowser;
            set => _currentBrowser = value;
        }

        /// <summary>
        /// Gets or sets the default browser language to use if not data source is given for a test.
        /// </summary>
        public virtual CultureInfo DefaultBrowserLanguage { get; set; }

        /// <summary>
        /// Gets or sets the current browser language.
        /// </summary>
        public virtual CultureInfo CurrentBrowserLanguage
        {
            get => _currentBrowserLanguage ?? DefaultBrowserLanguage;
            set => _currentBrowserLanguage = value;
        }

        /// <summary>
        /// Gets or sets the URL of a Selenium Remote Web Driver (leave empty or null to use local driver).
        /// </summary>
        public virtual string RemoteDriverUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating wether all processes of the current Browser should be killed.
        /// </summary>
        public virtual bool KillAllBrowserProcessesOnCleanup
        {
            get => _killAllBrowserProcessesOnCleanup ?? !TestHelper.IsDebugConfiguration;
            set => _killAllBrowserProcessesOnCleanup = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
        {
            Resolution = new Size(1280, 768);
            DefaultBrowser = Browser.Chrome;
            DefaultBrowserLanguage = CultureInfo.GetCultureInfo("en-us");
        }
    }
}
