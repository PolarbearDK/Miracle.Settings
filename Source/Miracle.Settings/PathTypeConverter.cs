using System;
using System.IO;
using System.Linq;

namespace Miracle.Settings
{
    /// <summary>
    /// Type converter that combines several strings into a valid path
    /// </summary>
    public class PathTypeConverter : ITypeConverter
    {
        public bool CanConvert(object[] values, Type conversionType)
        {
            return conversionType == typeof(string) && values.Length > 0 && values.All(x => x is string);
        }

        public object ChangeType(object[] values, Type conversionType, IFormatProvider formatProvider)
        {
            return Path.Combine(values.Cast<string>().ToArray());
        }
    }
}
