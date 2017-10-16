using System;

namespace Miracle.Settings
{
    /// <summary>
    /// SerringsLoader use this interface to query if a type converter can handle a type conversion, and eventually do the type conversion.
    /// </summary>
    public interface ITypeConverter
    {
        /// <summary>
        /// Check if <paramref name="values"/> can be converted to type <paramref name="conversionType"/>
        /// </summary>
        /// <param name="values">Values to convert</param>
        /// <param name="conversionType">Destination type to convert to</param>
        /// <returns>True if type converter is able to convert values to desired type, otherwise false</returns>
        bool CanConvert(object[] values, Type conversionType);

        /// <summary>
        /// Convert <paramref name="values"/> into instance of type <paramref name="conversionType"/>
        /// </summary>
        /// <param name="values">Values to convert</param>
        /// <param name="conversionType">The type of object to return.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <returns>Instance of type <paramref name="conversionType"/> or null if unable to convert</returns>
        object ChangeType(object[] values, Type conversionType, IFormatProvider formatProvider);
    }
}
