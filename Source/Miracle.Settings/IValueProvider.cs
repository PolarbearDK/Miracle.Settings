using System.Collections.Generic;

namespace Miracle.Settings
{
    public interface IValueProvider
    {
        bool TryGetValue(string key, out string value);
        bool TryGetKeys(string prefix, out IEnumerable<string> keys);
    }
}