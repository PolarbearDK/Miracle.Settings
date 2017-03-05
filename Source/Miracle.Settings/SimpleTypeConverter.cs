using System;
using System.Diagnostics;

namespace Miracle.Settings
{
    /// <summary>
    /// Simple type converter that handles conversion from a single string value using a converter function
    /// </summary>
    /// <typeparam name="T">Type to convert to</typeparam>
    public class SimpleTypeConverter<T> : ITypeConverter
    {
        private readonly Func<string, T> _convert;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="convert"></param>
        public SimpleTypeConverter(Func<string, T> convert)
        {
            _convert = convert;
        }

        /// <summary>
        /// The type that this converter handles.
        /// </summary>
        public Type Type => typeof(T);

        /// <summary>
        /// Check if <param name="values"/> can be converted to type <param name="conversionType"/>
        /// </summary>
        /// <param name="values">Values to convert</param>
        /// <param name="conversionType">Destination type to convert to</param>
        /// <returns>True if type converter is able to convert values to desired type, otherwise false</returns>
        public bool CanConvert(object[] values, Type conversionType)
        {
            return conversionType == typeof(T) && values.Length == 1 && (values[0] is string || values[0].GetType() == conversionType);
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
            Debug.Assert(values.Length == 1);

            var s = values[0] as string;
            if (s != null)
                return _convert(s);

            if (values[0].GetType() == conversionType)
                return values[0];

            return null;
        }
    }
}