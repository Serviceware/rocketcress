using Rocketcress.Core.Base;

namespace Rocketcress.UIAutomation;

/// <summary>
/// Settings for UIAutomation tests.
/// </summary>
/// <seealso cref="Rocketcress.Core.Base.SettingsBase" />
public class Settings : SettingsBase
{
    /// <summary>
    /// Gets or sets the name of the server.
    /// </summary>
    public virtual string ServerName { get; set; }
}
