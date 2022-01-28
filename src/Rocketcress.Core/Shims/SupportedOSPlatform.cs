#if NETFRAMEWORK
namespace System.Runtime.Versioning;

internal class SupportedOSPlatformAttribute : Attribute
{
    public SupportedOSPlatformAttribute(string platform)
    {
    }
}
#endif