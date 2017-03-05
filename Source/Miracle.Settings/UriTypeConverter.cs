using System;
using System.Configuration;

namespace Miracle.Settings
{
    /// <summary>
    /// Convert to Uri using 1 or 2 string values.
    /// The reference part is the base-uri, and the value part is the relative-uri.
    /// </summary>
    public class UriTypeConverter : ITypeConverter
    {
        /// <summary>
        /// Check if <param name="values"/> can be converted to type <param name="conversionType"/>
        /// </summary>
        /// <param name="values">Values to convert</param>
        /// <param name="conversionType">Destination type to convert to</param>
        /// <returns>True if type converter is able to convert values to desired type, otherwise false</returns>
        public bool CanConvert(object[] values, Type conversionType)
        {
            return conversionType == typeof(Uri) && values.Length > 0 && values.Length <= 2 && values[0] is string;
        }

        /// <summary>
        /// Convert <param name="values"/> into instance of type <param name="conversionType"/>
        /// </summary>
        /// <param name="values">Values to convert</param>
        /// <param name="conversionType">The type of object to return.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <returns>Instance of type <param name="conversionType"/> or null if unable to convert</returns>
        public object ChangeType(object[] values, Type conversionType, IFormatProvider formatProvider)
        {
            switch (values.Length)
            {
                case 1:
                    return new Uri((string) values[0]);
                case 2:
                    return new Uri(new Uri((string) values[0]), (string) values[1]);
                default:
                    throw new ConfigurationErrorsException("Wrong number of values provided for type converter.");
            }
        }
    }
}
