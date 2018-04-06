using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Miracle.Settings
{
#if NETFULL
	/// <summary>
	/// Value provider using ConfigurationManager.AppSettings as values
	/// </summary>
	public class AppSettingsValueProvider : IValueProvider
    {
        /// <summary>
        /// Get value identified by <paramref name="key" />
        /// </summary>
        /// <param name="key">Key to find</param>
        /// <param name="value">Output parameter: value found</param>
        /// <returns>True if value was found, otherwise false</returns>
        public bool TryGetValue(string key, out string value)
        {
            value = ConfigurationManager.AppSettings[key];
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
	        if (prefix != null)
	        {
		        var childPrefix = prefix + SettingsLoader.PropertySeparator;
		        keys = ConfigurationManager.AppSettings
			        .AllKeys
			        .Where(x => x.Equals(prefix) || x.StartsWith(childPrefix))
			        .ToArray();
	        }
	        else
	        {
		        keys = ConfigurationManager.AppSettings.AllKeys;
	        }

            return keys.Any();
        }
    }
#endif

}
