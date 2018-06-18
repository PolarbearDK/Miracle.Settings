using System.Collections.Generic;
using System.Linq;

namespace Miracle.Settings
{
	/// <summary>
	/// Value provider using Dictionary&lt;string,string&gt; as values
	/// </summary>
	public class DictionaryValueProvider : IValueProvider
	{
		private readonly IDictionary<string, string> _values;

		/// <summary>
		/// Construct using dictionary as values
		/// </summary>
		/// <param name="values">Value source</param>
		public DictionaryValueProvider(IDictionary<string, string> values)
		{
			_values = values;
		}

		/// <inheritdoc />
		public bool TryGetValue(string key, out string value)
		{
			return _values.TryGetValue(key, out value);
		}

		/// <inheritdoc />
		public bool TryGetKeys(string prefix, out IEnumerable<string> keys)
		{
			keys = _values.Keys.Where(x => x.StartsWith(prefix));
			return keys.Any();
		}
	}
}
