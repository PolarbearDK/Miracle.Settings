using System;
using System.Diagnostics;

namespace Miracle.Settings
{
    /// <summary>
    /// Simple type converter that handles conversion from a single string value using a converter function
    /// </summary>
    /// <typeparam name="T">Type to convert to</typeparam>
    public class SimpleTypeConverter<T> : ITypeConverter
    {
        private readonly Func<string, T> _convert;

        public SimpleTypeConverter(Func<string, T> convert)
        {
            _convert = convert;
        }

        public Type Type => typeof(T);

        public bool CanConvert(object[] values, Type conversionType)
        {
            return conversionType == typeof(T) && values.Length == 1 && (values[0] is string || values[0].GetType() == conversionType);
        }

        public object ChangeType(object[] values, Type conversionType, IFormatProvider formatProvider)
        {
            Debug.Assert(values.Length == 1);

            var s = values[0] as string;
            if (s != null)
                return _convert(s);

            if (values[0].GetType() == conversionType)
                return values[0];

            return null;
        }
    }
}