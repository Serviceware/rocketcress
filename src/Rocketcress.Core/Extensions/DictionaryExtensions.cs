#nullable disable

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
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="dict">The dictionary.</param>
        /// <param name="key">The key to try get the value from.</param>
        /// <returns>Returns the value of the key out of the dictioary. If the key does not exists, the default value is returned.</returns>
        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            if (dict.ContainsKey(key))
                return dict[key];
            return default;
        }
    }
}
