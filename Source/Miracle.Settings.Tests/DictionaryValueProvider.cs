using System.Collections.Generic;
using System.Linq;

namespace Miracle.Settings.Tests
{
	public class DictionaryValueProvider: IValueProvider
	{
		private readonly IDictionary<string, string> _values;

		public DictionaryValueProvider(IDictionary<string,string> values)
		{
			_values = values;
		}

		public bool TryGetValue(string key, out string value)
		{
			return _values.TryGetValue(key, out value);
		}

		public bool TryGetKeys(string prefix, out IEnumerable<string> keys)
		{
			keys = _values.Keys.Where(x => x.StartsWith(prefix));
			return keys.Any();
		}

	    public static SettingsLoader CreateSettingsLoader(IDictionary<string, string> values)
	    {
	        var valueProvider = new DictionaryValueProvider(values);
	        var settingsLoader = new SettingsLoader();
	        settingsLoader.ValueProviders.Clear();
	        settingsLoader.ValueProviders.Add(valueProvider);
	        return settingsLoader;
	    }

	}
}
