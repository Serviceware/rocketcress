using Rocketcress.Core.Utilities;
using System;

namespace Rocketcress.Core.IO
{
    /// <summary>
    /// Provides methods that help with XML files.
    /// </summary>
    [Obsolete("Use Rocketcress.Core.Utilities.XmlUtility instead.")]
    public static class XmlHelper
    {
        /// <summary>
        /// Loads an XML file.
        /// </summary>
        /// <typeparam name="T">The class to wich the XML file should be deserialized to.</typeparam>
        /// <param name="filePath">The file path.</param>
        /// <returns>Returns the deserialized XML file.</returns>
        public static T? LoadXmlFromFile<T>(string filePath)
            where T : class
            => XmlUtility.LoadXmlFromFile<T>(filePath);
    }
}
