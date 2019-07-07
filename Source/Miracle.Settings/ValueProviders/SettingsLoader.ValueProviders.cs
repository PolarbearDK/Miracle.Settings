using System.Collections.Generic;

namespace Miracle.Settings
{
    public partial class SettingsLoader
    {
        /// <summary>
        /// List of value providers that are searched until a value is found.
        /// </summary>
        public List<IValueProvider> ValueProviders { get; }

        /// <summary>
        /// Add value provider to list of value providers
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public SettingsLoader AddProvider(IValueProvider provider)
        {
            ValueProviders.Add(provider);
            return this;
        }

        /// <summary>
        /// Remove all value providers.
        /// </summary>
        /// <returns></returns>
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

        private bool HasKeys(string prefix)
        {
            IEnumerable<string> keys;
            return GetKeys(prefix, out keys);
        }
    }
}