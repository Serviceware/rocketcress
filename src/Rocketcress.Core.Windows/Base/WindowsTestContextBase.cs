using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Rocketcress.Core.Base;

/// <summary>
/// Base class of a test context for windows only tests.
/// </summary>
public abstract class WindowsTestContextBase : TestContextBase
{
#pragma warning disable CS1572 // XML comment has a param tag, but there is no parameter by that name
#pragma warning disable SA1612 // Element parameter documentation should match element parameters
    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsTestContextBase"/> class.
    /// </summary>
    /// <param name="testContext">The current MSTest test context.</param>
    /// <param name="settings">The test settings.</param>
#if !SLIM
    protected WindowsTestContextBase(TestContext testContext, SettingsBase settings)
        : base(testContext, settings)
#else
    protected WindowsTestContextBase(SettingsBase settings)
        : base(settings)
#endif
    {
    }
#pragma warning restore CS1572 // XML comment has a param tag, but there is no parameter by that name
#pragma warning restore SA1612 // Element parameter documentation should match element parameters

    /// <inheritdoc />
    protected override void SaveScreenshot(string path)
    {
        int screenLeft = SystemInformation.VirtualScreen.Left;
        int screenTop = SystemInformation.VirtualScreen.Top;
        int screenWidth = SystemInformation.VirtualScreen.Width;
        int screenHeight = SystemInformation.VirtualScreen.Height;

        using Bitmap bmp = new Bitmap(screenWidth, screenHeight);
        using (Graphics g = Graphics.FromImage(bmp))
            g.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);
        bmp.Save(path, ImageFormat.Jpeg);
    }
}
