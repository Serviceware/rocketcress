using System.Xml.Serialization;

namespace Rocketcress.Core.Utilities;

/// <summary>
/// Provides methods that help with XML files.
/// </summary>
public static class XmlUtility
{
    /// <summary>
    /// Loads an XML file.
    /// </summary>
    /// <typeparam name="T">The class to wich the XML file should be deserialized to.</typeparam>
    /// <param name="filePath">The file path.</param>
    /// <returns>Returns the deserialized XML file.</returns>
    public static T? LoadXmlFromFile<T>(string filePath)
        where T : class
    {
        using var fs = new FileStream(filePath, FileMode.Open);
        var xmlSer = new XmlSerializer(typeof(T));
        return xmlSer.Deserialize(fs) as T;
    }
}
