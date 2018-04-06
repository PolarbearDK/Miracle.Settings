#if NETCORE

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Miracle.Settings
{
	/// <summary>
	/// Value provider using IConfigurationRoot as values
	/// </summary>
	public class ConfigurationValueProvider : IValueProvider
	{
		private readonly IConfigurationRoot _configurationRoot;

		/// <summary>
		/// Construct a value provider based ov values from <paramref name="configurationRoot"/>
		/// </summary>
		/// <param name="configurationRoot"></param>
		public ConfigurationValueProvider(IConfigurationRoot configurationRoot)
		{
			_configurationRoot = configurationRoot;
		}
		/// <summary>
		/// Get value identified by <paramref name="key" />
		/// </summary>
		/// <param name="key">Key to find</param>
		/// <param name="value">Output parameter: value found</param>
		/// <returns>True if value was found, otherwise false</returns>
		public bool TryGetValue(string key, out string value)
		{
			value = _configurationRoot[key];
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
				var section = _configurationRoot.GetSection(prefix);

				if (section != null)
				{
					keys = section.AsEnumerable().Select(x => section.Key + "." + x.Key);
				}
				else
				{
					keys = new string[] { };
				}
			}
			else
			{
				keys = _configurationRoot.AsEnumerable().Select(x => x.Key);
			}

			return keys.Any();
		}
	}
}

#endif
