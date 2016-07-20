using System;
using System.Collections.Generic;
using System.Linq;

namespace Miracle.Settings
{
    public class EnvironmentProvider : IValueProvider
    {
        public bool TryGetValue(string key, out string value)
        {
            value = Environment.GetEnvironmentVariable(key);
            return value != null;
        }

        public bool TryGetKeys(string prefix, out IEnumerable<string> keys)
        {
            keys = Environment.GetEnvironmentVariables().Keys.Cast<string>();
            return false;
        }
    }
}