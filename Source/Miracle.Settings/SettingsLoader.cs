using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace Miracle.Settings
{
    /// <summary>
    /// Load settings directly into POCO objects.
    /// </summary>
    public partial class SettingsLoader : ISettingsLoader
    {
        public string PropertySeparator = ".";

        public SettingsLoader()
        {
            ValueProviders = GetDefaultValueProviders();
            _typeHandlers = GetTypeHandlers();
            TypeConverters = GetDefaultTypeConverters();
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
            foreach (var prop in GetLoadableProperties<T>())
            {
                var key = GetSettingKey(prefix, prop);

                object value = null;
                foreach (var typeHandler in _typeHandlers)
                {
                    // Attempt to get value from type handler. Exit when a handler succedes (returns true)
                    if (typeHandler(prop.PropertyType, key, out value))
                        break;
                }

                if (value == null)
                {
                    // Set default value if DefaultValueAttribute is present
                    DefaultValueAttribute attr = prop.GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault() as DefaultValueAttribute;
                    if (attr != null)
                        value = ChangeType(attr.Value, prop.PropertyType);
                    else
                    {
                        throw new ConfigurationErrorsException("A value has to be provided for Setting: " + key);
                    }
                }

                prop.SetValue(instance, value, null);

            }
            return this;
        }

        /// <summary>
        /// Create an object of type <typeparam name="T" /> initialized with settings prefixed by <param name="prefix"/>
        /// </summary>
        /// <param name="prefix">The prefix of all properties</param>
        public T Create<T>(string prefix = null)
        {
            if (typeof(T).IsClass && typeof(T) != typeof(string))
            {
                var instance = Activator.CreateInstance<T>();
                Load(instance, string.IsNullOrEmpty(prefix) ? prefix : prefix + PropertySeparator);
                return instance;
            }
            else
            {
                string stringValue;
                if (TryGetValue(prefix, out stringValue))
                {
                    return (T)ChangeType(stringValue, typeof(T));
                }
            }
            return default(T);
        }

        /// <summary>
        /// Create array with elements of type <typeparam name="T" /> from settings prefixed by <param name="prefix"/>
        /// </summary>
        /// <param name="prefix">The prefix of all settings in the array</param>
        public T[] CreateArray<T>(string prefix)
        {
            return CreateList<T>(prefix)?
                .ToArray();
        }

        /// <summary>
        /// Create a list with elements of type <typeparam name="T" /> from settings prefixed by <param name="prefix"/>
        /// </summary>
        /// <param name="prefix">The prefix of all settings in the list</param>
        public List<T> CreateList<T>(string prefix)
        {
            return GetCollectionPrefixes(ToCollectionPrefix(prefix))?
                .Select(Create<T>)
                .ToList();
        }

        /// <summary>
        /// Create dictionary from settings prefixed by <param name="prefix"/>
        /// </summary>
        /// <param name="prefix">The prefix of all settings in the dictionary</param>
        public Dictionary<TKey, TValue> CreateDictionary<TKey, TValue>(string prefix)
        {
            // Ensure valid collection prefix (including .)
            prefix = ToCollectionPrefix(prefix);

            return GetCollectionPrefixes(prefix)?
                .ToDictionary(
                    x => (TKey)ChangeType(x.Substring(prefix.Length), typeof(TKey)), 
                    Create<TValue>);
        }

        private string ToCollectionPrefix(string prefix)
        {
            return string.IsNullOrWhiteSpace(prefix) || prefix.EndsWith(PropertySeparator) ? prefix : prefix + PropertySeparator;
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
            var pos = key.IndexOf(PropertySeparator, prefix.Length + PropertySeparator.Length, StringComparison.InvariantCulture);
            return pos == -1 ? key : key.Substring(0, pos);
        }

        /// <summary>
        /// Return list of properties that are to be loaded on class <typeparam name="T" />
        /// </summary>
        /// <typeparam name="T">The class type that are being loaded with settings</typeparam>
        /// <returns>properties that are to be loaded on type <typeparam name="T" /></returns>
        protected virtual IEnumerable<PropertyInfo> GetLoadableProperties<T>()
        {
            return typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }

        /// <summary>
        /// Get the setting key by combining prefix and property name or setting name (annotation)
        /// </summary>
        /// <param name="prefix">The prefix of the current setting</param>
        /// <param name="prop">The property that is being loaded</param>
        /// <returns></returns>
        protected virtual string GetSettingKey(string prefix, PropertyInfo prop)
        {
            SettingAttribute attr = prop.GetCustomAttributes(typeof(SettingAttribute), false).FirstOrDefault() as SettingAttribute;
            if (attr != null)
                return prefix + attr.Name;
            return prefix + prop.Name;
        }
    }
}
