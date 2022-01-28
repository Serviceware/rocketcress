using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Rocketcress.Core.Base;

/// <summary>
/// Base class of a test context for windows only tests.
/// </summary>
public abstract class WindowsTestContextBase : TestContextBase
{
#if !SLIM
    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsTestContextBase"/> class.
    /// </summary>
    /// <param name="testContext">The current MSTest test context.</param>
    /// <param name="settings">The test settings.</param>
    protected WindowsTestContextBase(TestContext testContext, SettingsBase settings)
        : base(testContext, settings)
#else
    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsTestContextBase"/> class.
    /// </summary>
    /// <param name="settings">The test settings.</param>
    protected WindowsTestContextBase(SettingsBase settings)
        : base(settings)
#endif
    {
    }

    /// <inheritdoc/>
    public override bool CanTakeScreenshot { get; } = true;

    /// <inheritdoc />
    protected override void SaveScreenshot(string path)
    {
        int screenLeft = SystemInformation.VirtualScreen.Left;
        int screenTop = SystemInformation.VirtualScreen.Top;
        int screenWidth = SystemInformation.VirtualScreen.Width;
        int screenHeight = SystemInformation.VirtualScreen.Height;

        using var bmp = new Bitmap(screenWidth, screenHeight);
        using (var g = Graphics.FromImage(bmp))
            g.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);
        bmp.Save(path, ImageFormat.Jpeg);
    }
}
