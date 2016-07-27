using System;
using System.Configuration;

namespace Miracle.Settings
{
    public class EnumTypeConverter : ITypeConverter
    {
        public object ChangeType(object[] values, Type conversionType)
        {
            if(values[0] is string)
                return Enum.Parse(conversionType, (String)values[0], true);

            if (values[0].GetType() == conversionType)
                return values[0];

            throw new ConfigurationErrorsException("Unable to convert value " + values[0] + " to type " + conversionType);
        }

        public bool CanConvert(object[] values, Type conversionType)
        {
            return conversionType.IsEnum && values.Length == 1 && (values[0] is string || values[0].GetType() == conversionType);
        }
    }
}