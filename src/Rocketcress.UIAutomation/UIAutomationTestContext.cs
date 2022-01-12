using Rocketcress.Core;
using Rocketcress.Core.Base;
using Rocketcress.UIAutomation.Common;
using Rocketcress.UIAutomation.Interaction;

namespace Rocketcress.UIAutomation;

/// <summary>
/// Represents a test context for a UIAutomation Test.
/// </summary>
public class UIAutomationTestContext : WindowsTestContextBase
{
    /// <summary>
    /// Gets the current test settings.
    /// </summary>
    public new Settings Settings => (Settings)base.Settings;

    /// <summary>
    /// Gets or sets the <see cref="Application"/> that is currently under test and is actively tested currently.
    /// </summary>
    public Application ActiveApplication { get; set; }

    /// <summary>
    /// Gets all <see cref="Application"/>s that are currently under test.
    /// </summary>
    public List<Application> Applications { get; }

#pragma warning disable CS1572 // XML comment has a param tag, but there is no parameter by that name
#pragma warning disable SA1612 // Element parameter documentation should match element parameters
    /// <summary>
    /// Initializes a new instance of the <see cref="UIAutomationTestContext"/> class.
    /// </summary>
    /// <param name="testContext">The current MSTest test context.</param>
    /// <param name="settings">The test settings.</param>
#if !SLIM
    public UIAutomationTestContext(TestContext testContext, Settings settings)
        : base(testContext, settings)
#else
    public UIAutomationTestContext(Settings settings)
        : base(settings)
#endif
    {
        Applications = new List<Application>();
    }
#pragma warning restore CS1572 // XML comment has a param tag, but there is no parameter by that name
#pragma warning restore SA1612 // Element parameter documentation should match element parameters

    protected override void OnInitialize()
    {
        base.OnInitialize();

        Mouse.IsWaitForControlReadyEnabled = true;
        ControlUtility.EnsureControlRegistryIsFilled();

        // Set process DPI Aware, so Clicks are working on High DPI screens
        if (Environment.OSVersion.Version.Major >= 6)
            WindowsApiHelper.SetProcessDPIAware();
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        foreach (var app in Applications)
        {
            if (app.StartType == ApplicationStartType.Launched && !app.Process.HasExited)
                app.Process.Kill();
        }

        if (disposing)
        {
            Applications.Clear();
        }

        base.Dispose(disposing);
    }
}
