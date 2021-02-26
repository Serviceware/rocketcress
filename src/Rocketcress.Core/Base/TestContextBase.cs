using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
#if !SLIM
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Rocketcress.Core.Base
{
    /// <summary>
    /// Base class of a test context.
    /// </summary>
    public abstract class TestContextBase : IDisposable
    {
        /// <summary>
        /// Gets the current instance of the <see cref="TestContextBase"/>.
        /// </summary>
        public static TestContextBase CurrentContext { get; private set; }

        /// <summary>
        /// Event that is triggered when the context changed.
        /// </summary>
        public static event EventHandler<TestContextChangedEventArgs<TestContextBase>> ContextChanged;

#if !SLIM
        /// <summary>
        /// Gets or sets the current MSTest test context of the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }
#endif

        /// <summary>
        /// Gets or sets the current test settings.
        /// </summary>
        public virtual SettingsBase Settings { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestContextBase"/> class.
        /// </summary>
        protected TestContextBase()
        {
        }

        /// <summary>
        /// This method is called when the context has been created.
        /// </summary>
        /// <param name="lastContext">The last context.</param>
        protected virtual void OnContextCreated(TestContextBase lastContext)
        {
        }

        /// <summary>
        /// Takes a screenshots and appends it to the test result.
        /// </summary>
        /// <returns>Returns the path to the screenshot file.</returns>
        public virtual string TakeAndAppendScreenshot()
            => TakeAndAppendScreenshot(null);

        /// <summary>
        /// Takes a screenshots and appends it to the test result.
        /// </summary>
        /// <param name="name">The name of the screenshot.</param>
        /// <returns>Returns the path to the screenshot file.</returns>
        public virtual string TakeAndAppendScreenshot(string name)
        {
            try
            {
#if !SLIM
                string fileDir = TestContext.TestLogsDir;
                string fileName = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", TestContext.TestName, name).TrimEnd('_');
#else
                string fileDir = Environment.CurrentDirectory;
                string fileName = string.IsNullOrEmpty(name) ? "Unknwon" : name;
#endif
                string fileExt = ".jpg";
                string trimmedFileName = TrimString(fileName, 240 - fileDir.Length - fileExt.Length - 1);

                string path = Path.Combine(fileDir, trimmedFileName + fileExt);
                for (int i = 2; File.Exists(path) && i < 100; i++)
                {
                    trimmedFileName = TrimString(fileName, 240 - fileDir.Length - fileExt.Length - 3 - i.ToString(CultureInfo.InvariantCulture).Length);
                    path = Path.Combine(fileDir, string.Format(CultureInfo.InvariantCulture, "{0} ({1}){2}", trimmedFileName, i, fileExt));
                }

                SaveScreenshot(path);
#if !SLIM
                if (File.Exists(path))
                    TestContext.AddResultFile(path);
#endif
                return path;
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Could not take or save screenshot: {0}", ex);
                return null;
            }
        }

        /// <summary>
        /// Saves a screenshot to a specified location.
        /// </summary>
        /// <param name="path">The file to which the screenshot data should be written to.</param>
        protected abstract void SaveScreenshot(string path);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Determines wether the dispose is called from a <see cref="IDisposable.Dispose"/> method or a finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            CurrentContext = null;
            RaiseContextChangedEvent(this, null);
        }

        /// <summary>
        /// Converts a <see cref="JObject"/> packaged as an <see cref="object"/> to a specified type.
        /// </summary>
        /// <typeparam name="T">The type to which to convert the <see cref="JObject"/>.</typeparam>
        /// <param name="jObject">The <see cref="JObject"/> to convert.</param>
        /// <returns>Returns the <paramref name="jObject"/> converted to <typeparamref name="T"/>.</returns>
        public static T JObjectToObject<T>(object jObject)
            where T : class
        {
            var jo = (JObject)jObject;
            return jo.ToObject<T>();
        }

        /// <summary>
        /// Raises the <see cref="ContextChanged"/> event.
        /// </summary>
        /// <param name="oldContext">The old context.</param>
        /// <param name="newContext">The new context.</param>
        protected static void RaiseContextChangedEvent(object oldContext, object newContext)
        {
            ContextChanged?.Invoke(null, new TestContextChangedEventArgs<TestContextBase>(oldContext as TestContextBase, newContext as TestContextBase));
        }

#pragma warning disable CS1572 // XML comment has a param tag, but there is no parameter by that name
        /// <summary>
        /// Initializes an instance of the <see cref="TestContextBase"/> class.
        /// </summary>
        /// <typeparam name="T">The type of the context.</typeparam>
        /// <param name="activationFunc">A function that creates an instance of the wanted test context class.</param>
        /// <param name="settings">The settings to use during the test.</param>
        /// <param name="testContext">The MSTest Test Context.</param>
        /// <param name="initAction">An action that is executed before the new context is set as current context. Add additional information to the object here if needed.</param>
        /// <returns>The created context.</returns>
        protected static T CreateContext<T>(
            Func<T> activationFunc,
            SettingsBase settings,
#if !SLIM
            TestContext testContext,
#endif
            Action<T> initAction)
            where T : TestContextBase
        {
            var ctx = activationFunc();
#if !SLIM
            ctx.TestContext = testContext;
#endif
            ctx.Settings = settings;
            initAction?.Invoke(ctx);

            var oldContext = CurrentContext;
            CurrentContext = ctx;

            ctx.OnContextCreated(oldContext);
            RaiseContextChangedEvent(oldContext, ctx);

            return ctx;
        }
#pragma warning restore CS1572 // XML comment has a param tag, but there is no parameter by that name

        private static string TrimString(string str, int maxLength)
        {
            if (str == null || maxLength <= 0)
                return string.Empty;
            if (str.Length <= maxLength)
                return str;
            if (maxLength <= 3)
                return new string('.', maxLength);
            return str.Substring(0, (maxLength - 3) / 2) + "..." + str.Substring(str.Length - ((maxLength - 3) / 2));
        }
    }

    /// <summary>
    /// Represents event data for the <see cref="TestContextBase.ContextChanged"/> event.
    /// </summary>
    /// <typeparam name="T">The derived type of the <see cref="TestContextBase"/> class.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Event arguments")]
    public class TestContextChangedEventArgs<T> : EventArgs
        where T : TestContextBase
    {
        /// <summary>
        /// Gets the old instance of the <see cref="TestContextBase"/>.
        /// </summary>
        public T OldTestContext { get; }

        /// <summary>
        /// Gets the new instance of the <see cref="TestContextBase"/>.
        /// </summary>
        public T NewTestContext { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestContextChangedEventArgs{T}"/> class.
        /// </summary>
        /// <param name="oldContext">The old instance of the <see cref="TestContextBase"/>.</param>
        /// <param name="newContext">The new instance of the <see cref="TestContextBase"/>.</param>
        public TestContextChangedEventArgs(T oldContext, T newContext)
        {
            OldTestContext = oldContext;
            NewTestContext = newContext;
        }
    }
}
