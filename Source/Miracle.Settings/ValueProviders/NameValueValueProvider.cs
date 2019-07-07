using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Miracle.Settings
{
	/// <summary>
	/// Value provider using NameValueCollection as values
	/// </summary>
	public class NameValueValueProvider : IValueProvider
	{
		private readonly NameValueCollection _collection;

		/// <summary>
		/// Provide values from 
		/// </summary>
		/// <param name="collection"></param>
		public NameValueValueProvider(NameValueCollection collection)
		{
			_collection = collection;
		}

		/// <inheritdoc />
		public bool TryGetValue(string key, out string value)
		{
			value = _collection[key];
			return value != null;
		}

		/// <inheritdoc />
		public bool TryGetKeys(string prefix, out IEnumerable<string> keys)
		{
			if (prefix != null)
			{
				var childPrefix = prefix + SettingsLoader.PropertySeparator;
				keys = _collection
					.AllKeys
					.Where(x => x.Equals(prefix) || x.StartsWith(childPrefix))
					.ToArray();
			}
			else
			{
				keys = _collection.AllKeys;
			}

			return keys.Any();
		}
	}
}