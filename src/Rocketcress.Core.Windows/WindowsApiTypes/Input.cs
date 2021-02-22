using System.Runtime.InteropServices;

namespace Rocketcress.Core.WindowsApiTypes
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646270(v=vs.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Input
    {
        public InputType Type;
        public InputData Data;
    }

    public enum InputType : uint
    {
        Mouse = 0,
        Keyboard = 1,
        Hardware = 2
    }

    /// <summary>
    /// http://social.msdn.microsoft.com/Forums/en/csharplanguage/thread/f0e82d6e-4999-4d22-b3d3-32b25f61fb2a
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct InputData
    {
        [FieldOffset(0)]
        public HardwareInput Hardware;
        [FieldOffset(0)]
        public KeyboardInput Keyboard;
        [FieldOffset(0)]
        public MouseInput Mouse;
    }

    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646310(v=vs.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct HardwareInput
    {
        public uint Msg;
        public ushort ParamL;
        public ushort ParamH;
    }

    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646310(v=vs.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardInput
    {
        public ushort Vk;
        public ushort Scan;
        public uint Flags;
        public uint Time;        
    }

    /// <summary>
    /// http://social.msdn.microsoft.com/forums/en-US/netfxbcl/thread/2abc6be8-c593-4686-93d2-89785232dacd
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseInput
    {
        public int X;
        public int Y;
        public uint MouseData;
        public MouseEventFlags Flags;
        public uint Time;
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
