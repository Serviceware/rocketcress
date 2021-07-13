using Rocketcress.Core.Attributes;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
#if !SLIM
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

#nullable disable

namespace Rocketcress.Core.Base
{
    /// <summary>
    /// Base class for test classes.
    /// </summary>
    /// <typeparam name="TSettings">The type of the settings to use.</typeparam>
    /// <typeparam name="TContext">The type of the test context to use.</typeparam>
#if !SLIM
    [DeploymentItem("TestSettings/", "TestSettings/")]
    [DeploymentItem("TestSettings\\", "TestSettings\\")]
#endif
    public abstract class TestBase<TSettings, TContext> : TestObjectBase
        where TSettings : SettingsBase
        where TContext : TestContextBase
    {
        #region Properties

#if !SLIM
        /// <summary>
        /// Gets or sets the current MSTest <see cref="TestContext"/>.
        /// </summary>
        public TestContext TestContext { get; set; }
#endif

        /// <summary>
        /// Gets the current Rocketcress test context.
        /// </summary>
        public TContext CurrentContext { get; private set; }

        /// <summary>
        /// Gets or sets the current Test Settings.
        /// </summary>
        public virtual TSettings Settings
        {
            get => GetProperty(() => LoadSettings());
            set => SetProperty(value);
        }

        /// <summary>
        /// Gets the last exception that occurred in the current AppDomain.
        /// </summary>
        protected Exception LastException { get; private set; }

        #endregion

        #region Members for settings

        /// <summary>
        /// Determines the settings file to use.
        /// </summary>
        /// <param name="default">Determines wether the default settings.json should be loaded.</param>
        /// <returns>Returns the path to the correct settings file.</returns>
        protected virtual string GetSettingFile(bool @default) => SettingsBase.GetSettingFile(GetType().Assembly, null, @default);

        /// <summary>
        /// Loads the settings from the given settings files.
        /// </summary>
        /// <param name="settingsFile">The specialized settings file.</param>
        /// <param name="defaultSettingsFile">The default settings file.</param>
        /// <returns>Returns the settings from <paramref name="defaultSettingsFile"/> which values are overwritten by <paramref name="settingsFile"/>.</returns>
        protected virtual TSettings LoadSettings(string settingsFile, string defaultSettingsFile) => SettingsBase.GetFromFiles<TSettings>(settingsFile, defaultSettingsFile);

        #endregion

        #region Test Initialize/Cleanup

        /// <summary>
        /// Initializes a Test.
        /// </summary>
#if !SLIM
        [TestInitialize]
#endif
        public virtual void InitializeTest()
        {
#if !SLIM
            Logger.LogInfo($"Test '{TestContext.TestName}' initializing...");
            Logger.LogDebug($"Deployment directory: {TestContext.DeploymentDirectory}");
#endif

            TestHelper.IsDebugConfiguration = GetIsDebugConfiguration();
            Logger.LogDebug($"IsDebugConfiguration (could be overridden by derives classes): " + TestHelper.IsDebugConfiguration);

            AppDomain.CurrentDomain.FirstChanceException += AppDomain_FirstChanceException;

            if (!HasProperty(nameof(Settings)))
                SetProperty(LoadSettings(), nameof(Settings));
            ServiceContext.ResetInstance();
            CurrentContext = OnCreateContext();
        }

        /// <summary>
        /// Loads settings from the correct settings files.
        /// </summary>
        /// <returns>Returns the loaded settings.</returns>
        public TSettings LoadSettings()
        {
            var settingsFile = GetSettingFile(false);
            var defaultSettingsFile = GetSettingFile(true);
            var result = LoadSettings(settingsFile, defaultSettingsFile);

            CheckRequiredKeys(result, GetType());

            return result;
        }

        /// <summary>
        /// Cleans up the current Test.
        /// </summary>
#if !SLIM
        [TestCleanup]
#endif
        public virtual void CleanupTest()
        {
#if !SLIM
            if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed && LastException != null)
                Logger.LogError("Exception while test run: {0}", LastException);

            Logger.LogInfo($"Test '{TestContext.TestName}' cleaning up...");
#endif

            AppDomain.CurrentDomain.FirstChanceException -= AppDomain_FirstChanceException;
        }

        #endregion

        /// <summary>
        /// Is used to create the test context.
        /// </summary>
        /// <returns>The created context.</returns>
        protected abstract TContext OnCreateContext();

        #region Public Functions

        /// <summary>
        /// Checks if the settings contain all the settings keys defined by the given type.
        /// </summary>
        /// <param name="settings">The settings to check.</param>
        /// <param name="type">The type of the main SettingsKeys class.</param>
        public static void CheckRequiredKeys(SettingsBase settings, Type type)
        {
            var requiredKeys = AddKeysClassAttribute.GetKeys(type);
            var missingSettings = (from x in requiredKeys
                                   where !settings.OtherSettings.ContainsKey(SettingsBase.GetKey(x))
                                   select x).ToArray();
            if (missingSettings.Length > 0)
            {
                Logger.LogWarning("The following required setting keys were not provided in the settings file:" +
                    Environment.NewLine + "\t- " + string.Join(Environment.NewLine + "\t- ", missingSettings));
            }
        }

        #endregion

        #region Private Methods

        private void AppDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            LastException = e.Exception;
        }

        private bool GetIsDebugConfiguration()
        {
            var debuggableAttribute = GetType().Assembly.GetCustomAttribute<DebuggableAttribute>();
            return debuggableAttribute?.IsJITOptimizerDisabled == true;
        }

        #endregion
    }
}
