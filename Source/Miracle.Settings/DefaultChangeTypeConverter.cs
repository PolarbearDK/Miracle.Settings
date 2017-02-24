using System;
using System.Linq;

namespace Miracle.Settings
{
    public class DefaultChangeTypeConverter : ITypeConverter
    {
        private readonly Type[] _handledTypes =
        {
            typeof (Boolean),
            typeof (Char),
            typeof (SByte),
            typeof (Byte),
            typeof (Int16),
            typeof (UInt16),
            typeof (Int32),
            typeof (UInt32),
            typeof (Int64),
            typeof (UInt64),
            typeof (Single),
            typeof (Double),
            typeof (Decimal),
            typeof (String),
            typeof (Object),
        };

        public object ChangeType(object[] values, Type conversionType)
        {
            return Convert.ChangeType(values[0], conversionType);
        }

        public bool CanConvert(object[] values, Type conversionType)
        {
            return values.Length == 1 && _handledTypes.Contains(conversionType);
        }
    }
}
