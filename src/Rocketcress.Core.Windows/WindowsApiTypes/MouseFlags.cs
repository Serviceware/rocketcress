using System;

namespace Rocketcress.Core.WindowsApiTypes
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [Flags]
    public enum MouseEventFlags : uint
    {
        LeftDown = 0x00000002,
        LeftUp = 0x00000004,
        MiddleDown = 0x00000020,
        MiddleUp = 0x00000040,
        Move = 0x00000001,
        Absolute = 0x00008000,
        RightDown = 0x00000008,
        RightUp = 0x00000010,
        XDown = 0x00000080,
        XUp = 0x00000100,
        Wheel = 0x00000800,
        HWheel = 0x00001000
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
