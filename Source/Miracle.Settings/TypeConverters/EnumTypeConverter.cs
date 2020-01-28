using System;

namespace Miracle.Settings
{

    /// <summary>
    /// Type converter for Enums
    /// </summary>
    public class EnumTypeConverter : ITypeConverter
    {
		/// <summary>
		/// Check if <paramref name="values"/> can be converted to type <paramref name="conversionType"/>
		/// </summary>
		/// <param name="values">Values to convert</param>
		/// <param name="conversionType">Destination type to convert to</param>
		/// <returns>True if type converter is able to convert values to desired type, otherwise false</returns>
		public bool CanConvert(object[] values, Type conversionType)
        {
            return (conversionType.IsEnum || Nullable.GetUnderlyingType(conversionType)?.IsEnum == true)
                   && values.Length == 1 && (values[0] is string || values[0].GetType() == conversionType);
        }

		/// <summary>
		/// Convert <paramref name="values"/> into instance of type <paramref name="conversionType"/>
		/// </summary>
		/// <param name="values">Values to convert</param>
		/// <param name="conversionType">The type of object to return.</param>
		/// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
		/// <returns>Instance of type <paramref name="conversionType"/> or null if unable to convert</returns>
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
