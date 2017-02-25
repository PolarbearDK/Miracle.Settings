using System;
using System.Configuration;

namespace Miracle.Settings
{
    public class UriTypeConverter : ITypeConverter
    {
        public bool CanConvert(object[] values, Type conversionType)
        {
            return conversionType == typeof(Uri) && values.Length > 0 && values.Length <= 2 && values[0] is string;
        }

        public object ChangeType(object[] values, Type conversionType, IFormatProvider formatProvider)
        {
            switch (values.Length)
            {
                case 1:
                    return new Uri((string)values[0]);
                case 2:
                    return new Uri(new Uri((string)values[0]),(string)values[1]);
                default:
                    throw new ConfigurationErrorsException("Wrong number of values provided for type converter.");
            }
        }
    }
}
