using System;

namespace Miracle.Settings
{

    public class EnumTypeConverter : ITypeConverter
    {
        public bool CanConvert(object[] values, Type conversionType)
        {
            return conversionType.IsEnum && values.Length == 1 && (values[0] is string || values[0].GetType() == conversionType);
        }

        public object ChangeType(object[] values, Type conversionType, IFormatProvider formatProvider)
        {
            var s = values[0] as string;
            if(s != null)
                return Enum.Parse(conversionType, s, true);

            if (values[0].GetType() == conversionType)
                return values[0];

            return null;
        }
    }
}