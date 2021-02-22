using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Rocketcress.Core.Base
{
    /// <summary>
    /// Base class of a test context for windows only tests.
    /// </summary>
    public abstract class WindowsTestContextBase : TestContextBase
    {
        /// <summary>
        /// Gets the current instance of the <see cref="WindowsTestContextBase"/>.
        /// </summary>
        public static new WindowsTestContextBase CurrentContext { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsTestContextBase"/> class.
        /// </summary>
        protected WindowsTestContextBase() { }

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

        /// <inheritdoc />
        protected override void OnContextCreated(TestContextBase lastContext)
        {
            base.OnContextCreated(lastContext);
            CurrentContext = this;
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            CurrentContext = null;
            base.Dispose(disposing);
        }
    }
}
