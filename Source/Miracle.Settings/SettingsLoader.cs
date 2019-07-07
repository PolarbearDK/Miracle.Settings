using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Miracle.Settings.Properties;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Miracle.Settings.Tests")]

namespace Miracle.Settings
{
	/// <summary>
    /// Load settings directly into POCO objects.
    /// </summary>
    public partial class SettingsLoader : ISettingsLoader
    {
        /// <summary>
        /// The separator between appSetting key fragments
        /// </summary>
        internal const string PropertySeparator = ":";

#if NETFULL
		/// <summary>
		/// Construct SettingsLoader using AppSettings as value provider
		/// </summary>
		public SettingsLoader()
            : this(new AppSettingsValueProvider())
        {
        }
#endif
		/// <summary>
		/// Construct SettingsLoader using specific Value provider
		/// </summary>
		/// <param name="valueProvider">value providers to use for this SettingLoader instance</param>
		public SettingsLoader(IValueProvider valueProvider)
			: this(new [] { valueProvider })
	    {
        }

		/// <summary>
		/// Construct SettingsLoader using specific Value providers
		/// </summary>
		/// <param name="valueProviders">Enumeration of value providers to use for this SettingLoader instance</param>
		public SettingsLoader(IEnumerable<IValueProvider> valueProviders)
        {
            ValueProviders = new List<IValueProvider>(valueProviders);
            _typeHandlers = GetTypeHandlers();
	        PathProviders = GetDefaultPathConverters();
			TypeConverters = GetDefaultTypeConverters();
        }

        /// <summary>
        /// Check if settings matches any properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public bool HasSettings<T>(string prefix = null)
        {
            // Initialize all properties of T with values provided by typehandlers using reflection
            return GetLoadableProperties<T>()
                .Select(propertyInfo => GetSettingKey(prefix, propertyInfo))
                .Any(HasKeys);
        }

        /// <summary>
        /// Load an existing instance of type T with settings prefixed by prefix.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public ISettingsLoader Load<T>(T instance, string prefix = null)
        {
            // Initialize all properties of T with values provided by typehandlers using reflection
            foreach (var propertyInfo in GetLoadableProperties<T>())
            {
                var key = GetSettingKey(prefix, propertyInfo);

                object value = null;
                if (_typeHandlers.Any(typeHandler => typeHandler(propertyInfo, prefix, key, out value)))
                {
                    if (value != null)
                    {
                        propertyInfo.SetValue(instance, value, null);
                        continue;
                    }
                }

                if (!IsPropertyOptional(propertyInfo))
                {
                    throw new SettingsException(string.Format(Resources.MissingValueFormat, propertyInfo.PropertyType, key));
                }
            }

            // Validate model using System.ComponentModel.DataAnnotations;
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(instance, null, null);
            if (!Validator.TryValidateObject(instance, context, validationResults, true))
            {
                var validationResult = validationResults.First();
                throw new SettingsException(string.Format(Resources.ValidationError, GetSettingKey(prefix, validationResult.MemberNames.First()), validationResult.ErrorMessage));
            }

            return this;
        }

        /// <summary>
        /// Create an object of type <typeparam name="T" /> initialized with settings prefixed by <paramref name="prefix"/>
        /// </summary>
        /// <param name="prefix">The prefix of all properties</param>
        public T Create<T>(string prefix = null)
        {
            string stringValue = null;
            if (!string.IsNullOrEmpty(prefix))
            {
                TryGetValue(prefix, out stringValue);

                if (stringValue != null)
                {
                    // Attempt to convert directly
                    if (CanChangeType(new object[] {stringValue}, typeof(T)))
                        return (T) ChangeType(stringValue, typeof(T));
                }
            }

            if (typeof(T).IsClass)
            {
                T instance;
                try
                {
                    instance = Activator.CreateInstance<T>();
                }
                catch (Exception ex)
                {
                    // Error could be caused by missing value and for that reason no matching type converter.
                    if (stringValue == null)
                    {
                        throw new SettingsException(string.Format(Resources.MissingValueFormat, typeof(T), prefix));
                    }

                    throw new SettingsException(string.Format(Resources.CreateErrorFormat, typeof(T), prefix), ex);
                }
                Load(instance, prefix);
                return instance;
            }
            throw new SettingsException(string.Format(Resources.MissingValueFormat, typeof(T), prefix));
        }

        /// <summary>
        /// Create array with elements of type <typeparam name="T" /> from settings prefixed by <paramref name="prefix"/>
        /// </summary>
        /// <param name="prefix">The prefix of all settings in the array</param>
        public T[] CreateArray<T>(string prefix)
        {
            return CreateList<T>(prefix)?
                .ToArray();
        }

        /// <summary>
        /// Create array of type T from setting with key <paramref name="key"/> and split into string array using <paramref name="separator"/> and <paramref name="options"/>.
        /// </summary>
        /// <param name="key">The key of the setting containing separated values</param>
        /// <param name="separator">the separator(s) used to split string value</param>
        /// <param name="options">the option used to split string value</param>
        public T[] CreateArray<T>(string key, char[] separator, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            return CreateList<T>(key, separator, options)?
                .ToArray();
        }

        /// <summary>
        /// Create a list with elements of type <typeparam name="T" /> from settings prefixed by <paramref name="prefix"/>
        /// </summary>
        /// <param name="prefix">The prefix of all settings in the list</param>
        public List<T> CreateList<T>(string prefix)
        {
            return GetCollectionPrefixes(prefix)?
                .Select(Create<T>)
                .ToList();
        }

        /// <summary>
        /// Create list of type T from setting with key <paramref name="key"/> and split into string array using <paramref name="separator"/> and <paramref name="options"/>.
        /// </summary>
        /// <param name="key">The key of the setting containing separated values</param>
        /// <param name="separator">the separator(s) used to split string value</param>
        /// <param name="options">the option used to split string value</param>
        public List<T> CreateList<T>(string key, char[] separator, StringSplitOptions options)
        {
            if (!string.IsNullOrEmpty(key))
            {
                string stringValue;
                TryGetValue(key, out stringValue);

                if (stringValue != null)
                {
                    return stringValue
                        .Split(separator, options)
                        .Select(value =>
                        {
                            // Attempt to convert directly
                            if (CanChangeType(new object[] {value}, typeof(T)))
                                return (T) ChangeType(value, typeof(T));

                            throw new SettingsException(string.Format(Resources.ConvertValueErrorFormat, value, typeof(T)));
                        }).ToList();
                }
            }
            return null;
        }

        /// <summary>
        /// Create dictionary from settings prefixed by <paramref name="prefix"/>.
        /// </summary>
        /// <param name="prefix">The prefix of all settings in the dictionary</param>
        /// <param name="comparer">Optional dictionary key comparer</param>
        public Dictionary<TKey, TValue> CreateDictionary<TKey, TValue>(string prefix, IEqualityComparer<TKey> comparer = null)
        {
            return GetCollectionPrefixes(prefix)?
                .ToDictionary(
                    x => (TKey) ChangeType(prefix != null ? x.Substring(prefix.Length+ PropertySeparator.Length) : x, typeof(TKey)),
                    Create<TValue>,
                    comparer);
        }

        /// <summary>
        /// Get collection prefixes
        /// </summary>
        /// <example>
        /// With a list of keys:
        ///   A.B.C1.D
        ///   A.B.C1.E
        ///   A.B.C2.F
        ///
        /// and a prefix of:
        ///   A.B.
        ///
        /// will produce
        ///   A.B.C1
        ///   A.B.C2
        ///
        /// </example>
        /// <param name="prefix">The prefix of all settings in the collection</param>
        /// <returns></returns>
        private IEnumerable<string> GetCollectionPrefixes(string prefix)
        {
            IEnumerable<string> keys;
            if (GetKeys(prefix, out keys))
            {
                return keys
                    .GroupBy(x => GetCollectionSelector(prefix, x))
                    .Select(x => x.Key);
            }
            return null;
        }

        /// <summary>
        /// Get key from start up to next PropertySeparator after prefix
        /// </summary>
        /// <param name="prefix">The prefix of the current setting</param>
        /// <param name="key">Key of the current setting</param>
        /// <returns></returns>
        private string GetCollectionSelector(string prefix, string key)
        {
            var toskip = (prefix != null ? prefix.Length : 0) + PropertySeparator.Length;
            var pos = key.IndexOf(PropertySeparator, toskip, StringComparison.InvariantCulture);
            return pos == -1 ? key : key.Substring(0, pos);
        }

		/// <summary>
		/// Check if property is optional, by convention og by attribute.
		/// </summary>
		/// <param name="propertyInfo"></param>
		/// <returns>true if property is optional</returns>
	    private bool IsPropertyOptional(PropertyInfo propertyInfo)
		{
			return
				// Has Optional attribute
				propertyInfo.GetCustomAttributes(typeof(OptionalAttribute), false).Any()
				// or is nullable type
				|| Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null;
		}

		/// <summary>
		/// Return list of properties that are to be loaded on class <typeparamref name="T" />
		/// </summary>
		/// <typeparam name="T">The class type that are being loaded with settings</typeparam>
		/// <returns>properties that are to be loaded on type <typeparamref name="T" /></returns>
		protected virtual IEnumerable<PropertyInfo> GetLoadableProperties<T>()
        {
            return typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.CanWrite)
                .Where(x => !x.GetCustomAttributes(typeof(IgnoreAttribute), false).Any());
        }

        /// <summary>
        /// Get the setting key by combining prefix and property name or setting name (annotation)
        /// </summary>
        /// <param name="prefix">The prefix of the current setting</param>
        /// <param name="propertyInfo">The property that is being loaded</param>
        /// <returns></returns>
        protected virtual string GetSettingKey(string prefix, PropertyInfo propertyInfo)
        {
            SettingAttribute attribute = propertyInfo.GetCustomAttributes(typeof(SettingAttribute), false).FirstOrDefault() as SettingAttribute;
            if (attribute?.Name != null)
                return GetSettingKey(prefix, attribute.Name);
            return GetSettingKey(prefix, propertyInfo.Name);
        }

	    /// <summary>
	    /// Get the setting key by combining prefix and name 
	    /// </summary>
	    /// <param name="prefix">The prefix of the setting</param>
	    /// <param name="name">The name of the setting</param>
	    /// <returns></returns>
	    internal static string GetSettingKey(string prefix, string name)
	    {
		    return prefix != null
			    ? prefix + PropertySeparator + name
			    : name;
	    }

		/// <summary>
	    /// Get the setting key by combining key parts
	    /// </summary>
	    /// <param name="parts">The parts to combine into setting key</param>
	    /// <returns></returns>
	    internal static string GetSettingKey(params string[] parts)
	    {
		    return string.Join(PropertySeparator, parts);
	    }
    }
}
