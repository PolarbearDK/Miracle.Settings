using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Miracle.Settings
{
    /// <summary>
    /// Value provider using ConfigurationManager.AppSettings as values
    /// </summary>
    public class AppSettingsValueProvider : IValueProvider
    {
        public bool TryGetValue(string key, out string value)
        {
            value = ConfigurationManager.AppSettings[key];
            return value != null;
        }

        public bool TryGetKeys(string prefix, out IEnumerable<string> keys)
        {
            keys = ConfigurationManager.AppSettings.AllKeys.Where(x => x.StartsWith(prefix)).ToArray();
            return keys.Any();
        }
    }
}