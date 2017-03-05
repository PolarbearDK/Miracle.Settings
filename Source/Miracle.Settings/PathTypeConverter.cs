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
        /// <summary>
        /// Check if <param name="values"/> can be converted to type <param name="conversionType"/>
        /// </summary>
        /// <param name="values">Values to convert</param>
        /// <param name="conversionType">Destination type to convert to</param>
        /// <returns>True if type converter is able to convert values to desired type, otherwise false</returns>
        public bool CanConvert(object[] values, Type conversionType)
        {
            return conversionType == typeof(string) && values.Length > 0 && values.All(x => x is string);
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
            return Path.Combine(values.Cast<string>().ToArray());
        }
    }
}
