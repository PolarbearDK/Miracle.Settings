using System.Collections.Generic;
using System.Linq;

namespace Miracle.Settings.Tests
{
	public class MockValueProvider: IValueProvider
	{
		private readonly IDictionary<string, string> _values;

		public MockValueProvider(IDictionary<string,string> values)
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
	}
}
