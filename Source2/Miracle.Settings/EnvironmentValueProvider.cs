using System;
using System.Collections.Generic;
using System.Linq;

namespace Miracle.Settings
{
    /// <summary>
    /// Value provider using environment variables as values
    /// </summary>
    public class EnvironmentValueProvider : IValueProvider
    {
		/// <summary>
		/// Get value identified by <paramref name="key" />
		/// </summary>
		/// <param name="key">Key to find</param>
		/// <param name="value">Output parameter: value found</param>
		/// <returns>True if value was found, otherwise false</returns>
		public bool TryGetValue(string key, out string value)
        {
            value = Environment.GetEnvironmentVariable(key);
            return value != null;
        }

		/// <summary>
		/// Get all keys prefixed by <paramref name="prefix" />
		/// </summary>
		/// <param name="prefix">Prefix to find</param>
		/// <param name="keys">Output parameter: keys found</param>
		/// <returns>True if values was found, otherwise false</returns>
		public bool TryGetKeys(string prefix, out IEnumerable<string> keys)
        {
            keys = Environment.GetEnvironmentVariables().Keys.Cast<string>();
            return false;
        }
    }
}
