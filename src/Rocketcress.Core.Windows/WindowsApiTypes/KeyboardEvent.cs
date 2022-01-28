namespace Rocketcress.Core.WindowsApiTypes;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1602 // Enumeration items should be documented

public enum KeyboardEvent
{
    KeyDown = 0x0000,
    ExtendedKey = 0x0001,
    KeyUp = 0x0002,
    Press = KeyDown | KeyUp,
}
