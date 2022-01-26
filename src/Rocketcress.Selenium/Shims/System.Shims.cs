#if NETFRAMEWORK
#pragma warning disable SA1649 // File name should match first type name
namespace System.Runtime.Versioning
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event, AllowMultiple = true, Inherited = false)]
    internal sealed class SupportedOSPlatformAttribute : Attribute
    {
        public SupportedOSPlatformAttribute(string platformName)
        {
            PlatformName = platformName;
        }

        public string PlatformName { get; }
    }
}
#endif