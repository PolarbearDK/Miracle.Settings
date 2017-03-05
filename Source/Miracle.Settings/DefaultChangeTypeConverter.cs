using System;
using System.Linq;

namespace Miracle.Settings
{
    /// <summary>
    /// This is the fallback change type converter that SetttingsLoader uses when all else fails.
    /// </summary>
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

        /// <summary>
        /// Convert <param name="values"/> into instance of type <param name="conversionType"/>
        /// </summary>
        /// <param name="values">Values to convert</param>
        /// <param name="conversionType">The type of object to return.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <returns>Instance of type <param name="conversionType"/> or null if unable to convert</returns>
        public object ChangeType(object[] values, Type conversionType, IFormatProvider formatProvider)
        {
            return Convert.ChangeType(values[0], conversionType, formatProvider);
        }

        /// <summary>
        /// Check if <param name="values"/> can be converted to type <param name="conversionType"/>
        /// </summary>
        /// <param name="values">Values to convert</param>
        /// <param name="conversionType">Destination type to convert to</param>
        /// <returns>True if type converter is able to convert values to desired type, otherwise false</returns>
        public bool CanConvert(object[] values, Type conversionType)
        {
            return values.Length == 1 && _handledTypes.Contains(conversionType);
        }
    }
}
