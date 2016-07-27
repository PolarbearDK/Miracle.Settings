using System.Collections.Generic;

namespace Miracle.Settings
{
    public interface IValueProvider
    {
        /// <summary>
        /// Get value identified by <param name="key" />
        /// </summary>
        /// <param name="key">Key to find</param>
        /// <param name="value">Output parameter: value found</param>
        /// <returns>True if value was found, otherwise false</returns>
        bool TryGetValue(string key, out string value);
        /// <summary>
        /// Get all keys prefixed by <param name="prefix" />
        /// </summary>
        /// <param name="prefix">Prefix to find</param>
        /// <param name="keys">Output parameter: keys found</param>
        /// <returns>True if values was found, otherwise false</returns>
        bool TryGetKeys(string prefix, out IEnumerable<string> keys);
    }
}