using System.Collections.Generic;

namespace Miracle.Settings
{
    public partial class SettingsLoader
    {
        /// <summary>
        /// List of value providers that are searched until a value is found.
        /// </summary>
        public List<IValueProvider> ValueProviders { get; }

        private static List<IValueProvider> GetDefaultValueProviders()
        {
            return new List<IValueProvider>
            {
                new AppSettingsValueProvider()
            };
        }

        public SettingsLoader AddProvider(IValueProvider provider)
        {
            ValueProviders.Add(provider);
            return this;
        }

        public SettingsLoader ClearProviders()
        {
            ValueProviders.Clear();
            return this;
        }

        private bool TryGetValue(string key, out string value)
        {
            value = null;
            foreach (var provider in ValueProviders)
            {
                if (provider.TryGetValue(key, out value))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Get all keys starting with prefix.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        private bool GetKeys(string prefix, out IEnumerable<string> keys)
        {
            foreach (var provider in ValueProviders)
            {
                if (provider.TryGetKeys(prefix, out keys))
                {
                    return true;
                }
            }
            keys = null;
            return false;
        }
    }
}