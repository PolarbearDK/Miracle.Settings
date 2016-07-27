using System;

namespace Miracle.Settings
{
    public class DefaultChangeTypeConverter : ITypeConverter
    {
        public object ChangeType(object[] values, Type conversionType)
        {
            return Convert.ChangeType(values[0], conversionType);
        }

        public bool CanConvert(object[] values, Type conversionType)
        {
            return values.Length == 1;
        }
    }
}