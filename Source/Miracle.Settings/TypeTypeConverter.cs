using System;
using System.Configuration;

namespace Miracle.Settings
{
	public class TypeTypeConverter : ITypeConverter
	{
		public bool CanConvert(object[] values, Type conversionType)
		{
			return conversionType == typeof(Type) && values.Length == 1 && values[0] is string;
		}

		public object ChangeType(object[] values, Type conversionType)
		{
			switch (values.Length)
			{
				case 1:
					return Type.GetType((string)values[0], true);
				default:
					throw new ConfigurationErrorsException("Wrong number of values provided for type converter.");
			}
		}
	}
}
