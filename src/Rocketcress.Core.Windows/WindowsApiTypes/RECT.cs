using System.Runtime.InteropServices;

namespace Rocketcress.Core.WindowsApiTypes;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter
[StructLayout(LayoutKind.Sequential)]
public struct RECT
{
    public int left;
    public int top;
    public int right;
    public int bottom;

    public RECT(int left, int top, int right, int bottom)
    {
        this.left = left;
        this.top = top;
        this.right = right;
        this.bottom = bottom;
    }

    public bool IsEmpty
    {
        get
        {
            return left >= right || top >= bottom;
        }
    }
}
