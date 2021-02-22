using System.Collections.Generic;

namespace Rocketcress.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the IDictionary interface.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Tries to get a value out of the dictionary.
        /// </summary>
        /// <typeparam name="U">The key type.</typeparam>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key to try get the value from.</param>
        /// <returns>Returns the value of the key out of the dictioary. If the key does not exists, the default value is returned.</returns>
        public static T TryGetValue<U,T>(this IDictionary<U,T> dict, U key)
        {
            if (dict.ContainsKey(key))
                return dict[key];
            return default(T);
        }
    }
}
