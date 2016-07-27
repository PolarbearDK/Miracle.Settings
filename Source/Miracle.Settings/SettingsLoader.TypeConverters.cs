using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Miracle.Settings
{
    public partial class SettingsLoader
    {
        /// <summary>
        /// Dictionary containing explicit type conversions (in addition to what <see cref="Convert"/> can handle)
        /// </summary>
        public List<ITypeConverter> TypeConverters { get; }

        /// <summary>
        /// Get default type converters
        /// </summary>
        /// <returns></returns>
        private List<ITypeConverter> GetDefaultTypeConverters()
        {
            return new List<ITypeConverter>()
            {
                new SimpleTypeConverter<Guid>(Guid.Parse),
                new SimpleTypeConverter<TimeSpan>(TimeSpan.Parse),
                new UriTypeConverter(),
                new EnumTypeConverter(),
                new DefaultChangeTypeConverter(),
            };
        }

        /// <summary>
        /// Convert value into instance of type conversionType
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <param name="conversionType">The type to convert to</param>
        /// <returns></returns>
        private object ChangeType(object value, Type conversionType)
        {
            return ChangeType(new[] {value}, conversionType);
        }

        /// <summary>
        /// Convert values into instance of type conversionType
        /// </summary>
        /// <param name="values">the values to convert</param>
        /// <param name="conversionType">The type to convert to</param>
        /// <returns></returns>
        private object ChangeType(object[] values, Type conversionType)
        {
            foreach (var typeConverter in TypeConverters)
            {
                if (typeConverter.CanConvert(values, conversionType))
                    return typeConverter.ChangeType(values, conversionType);
            }

            throw new ConfigurationErrorsException(
                values.Length == 1 
                ? $"Unable to convert value: {values.FirstOrDefault()} into type {conversionType}"
                : $"Unable to convert values: {string.Join(", ",values.Select(x=>x.ToString()))} into type {conversionType}");
        }

        public SettingsLoader RemoveTypeConverter(ITypeConverter typeConverter)
        {
            TypeConverters.Remove(typeConverter);
            return this;
        }


        public SettingsLoader AddTypeConverter(ITypeConverter typeConverter)
        {
            TypeConverters.Insert(0, typeConverter);
            return this;
        }

        /// <summary>
        /// Add simple type converter for type <typeparamref name="T" /> using converter function <paramref name="convert" />
        /// </summary>
        /// <remarks>
        /// Implemented using <see cref="SimpleTypeConverter{T}"/>
        /// </remarks>
        /// <typeparam name="T">Type to add converter for</typeparam>
        /// <param name="convert">converter function to convert from string to <typeparamref name="T" /></param>
        /// <returns>Reference to current SettingsLoader for fluid configuration</returns>
        public SettingsLoader AddTypeConverter<T>(Func<string, T> convert)
        {
            AddTypeConverter(new SimpleTypeConverter<T>(convert));
            return this;
        }
    }
}