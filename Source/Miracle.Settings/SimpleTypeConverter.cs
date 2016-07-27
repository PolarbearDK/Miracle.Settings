using System;
using System.Diagnostics;

namespace Miracle.Settings
{
    /// <summary>
    /// Simple type converter that handles conversion from a single string value
    /// </summary>
    /// <typeparam name="T">Type to convert to</typeparam>
    public class SimpleTypeConverter<T> : ITypeConverter
    {
        private readonly Func<string, T> _convert;

        public SimpleTypeConverter(Func<string, T> convert)
        {
            _convert = convert;
        }

        public Type Type { get { return typeof (T); } }

        public bool CanConvert(object[] values, Type conversionType)
        {
            return conversionType == typeof(T) && values.Length == 1 && values[0] is string;
        }

        public object ChangeType(object[] values, Type conversionType)
        {
            Debug.Assert(values.Length == 1);
            return _convert((string)values[0]);
        }
    }
}