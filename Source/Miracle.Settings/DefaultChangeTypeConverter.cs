using System;
using System.Linq;

namespace Miracle.Settings
{
    public class DefaultChangeTypeConverter : ITypeConverter
    {
        private readonly Type[] _handledTypes =
        {
            typeof (bool),
            typeof (char),
            typeof (sbyte),
            typeof (byte),
            typeof (short),
            typeof (ushort),
            typeof (int),
            typeof (uint),
            typeof (long),
            typeof (ulong),
            typeof (float),
            typeof (double),
            typeof (decimal),
            typeof (string),
            typeof (object),
        };

        public object ChangeType(object[] values, Type conversionType, IFormatProvider formatProvider)
        {
            return Convert.ChangeType(values[0], conversionType, formatProvider);
        }

        public bool CanConvert(object[] values, Type conversionType)
        {
            return values.Length == 1 && _handledTypes.Contains(conversionType);
        }
    }
}
