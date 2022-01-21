using System.Globalization;
using Rocketcress.Core.Models;

namespace Rocketcress.Core.Base;

/// <summary>
/// Base class of a test context.
/// </summary>
public class TestContextBase : TestObjectBase, IDisposable
{
#if !SLIM
    /// <summary>
    /// Gets the current MSTest test context of the current test run.
    /// </summary>
    public TestContext TestContext { get; }
#endif

    /// <summary>
    /// Gets the current test settings.
    /// </summary>
    public virtual SettingsBase Settings { get; }

#pragma warning disable CS1572 // XML comment has a param tag, but there is no parameter by that name
#pragma warning disable SA1612 // Element parameter documentation should match element parameters
    /// <summary>
    /// Initializes a new instance of the <see cref="TestContextBase"/> class.
    /// </summary>
    /// <param name="testContext">The current MSTest test context.</param>
    /// <param name="settings">The test settings.</param>
#if !SLIM
    public TestContextBase(TestContext testContext, SettingsBase settings)
    {
        TestContext = testContext;
#else
    public TestContextBase(SettingsBase settings)
    {
#endif
        Settings = settings;
        OnContextCreated();
    }
#pragma warning restore SA1612 // Element parameter documentation should match element parameters
#pragma warning restore CS1572 // XML comment has a param tag, but there is no parameter by that name

    /// <summary>
    /// Initializes this test context.
    /// </summary>
    public virtual void Initialize()
    {
        OnInitialize();
    }

    /// <summary>
    /// Takes a screenshots and appends it to the test result.
    /// </summary>
    /// <returns>Returns the path to the screenshot file.</returns>
    public string? TakeAndAppendScreenshot()
        => TakeAndAppendScreenshot(null);

    /// <summary>
    /// Takes a screenshots and appends it to the test result.
    /// </summary>
    /// <param name="name">The name of the screenshot.</param>
    /// <returns>Returns the path to the screenshot file.</returns>
    public virtual string? TakeAndAppendScreenshot(string? name)
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
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Saves a screenshot to a specified location.
    /// </summary>
    /// <param name="path">The file to which the screenshot data should be written to.</param>
    protected virtual void SaveScreenshot(string path)
    {
        // Do not take screenshot in base class. To enable this functionality override this method in a derived class.
    }

    /// <summary>
    /// This method is called when the context has been created.
    /// </summary>
    protected virtual void OnContextCreated()
    {
    }

    /// <summary>
    /// This method is called when the context is initializing.
    /// </summary>
    protected virtual void OnInitialize()
    {
        Wait.DefaultOptions.Timeout = Settings.Timeout;
        LanguageDependent.DefaultLanguage = Settings.Language;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <param name="disposing">Determines wether the dispose is called from a <see cref="IDisposable.Dispose"/> method or a finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            ClearProperties();
        }
    }

    private static string TrimString(string? str, int maxLength)
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
