using System;
using System.Collections.Generic;

namespace Miracle.Settings
{
    public partial class SettingsLoader
    {
        /// <summary>
        /// Dictionary containing explicit type conversions (in addition to what <see cref="Convert"/> can handle)
        /// </summary>
        public Dictionary<Type, Func<string, object>> TypeConverters { get; }

        /// <summary>
        /// Get default type converters
        /// </summary>
        /// <returns></returns>
        protected virtual Dictionary<Type, Func<string, object>> GetDefaultTypeConverters()
        {
            return new Dictionary<Type, Func<string, object>>
            {
                {typeof(Guid), s => Guid.Parse(s)},
                {typeof(TimeSpan), s => TimeSpan.Parse(s)},
                {typeof(Uri), s => new Uri(s)}
            };
        }
        /// <summary>
        /// Add extra explicit conversions to what Convert.ChangeType can handle
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <param name="conversionType">The type to convert to</param>
        /// <returns></returns>
        private object ChangeType(object value, Type conversionType)
        {
            var s = value as string;
            if (s != null)
            {
                Func<string, object> func;
                if (TypeConverters.TryGetValue(conversionType, out func))
                {
                    return func(s);
                }

                if (conversionType.IsEnum)
                    return Enum.Parse(conversionType, s, true);
            }
            return Convert.ChangeType(value, conversionType);
        }

        public SettingsLoader RemoveTypeConverter(Type type)
        {
            TypeConverters.Remove(type);
            return this;
        }

        public SettingsLoader AddTypeConverter(Type type, Func<string, object> converter)
        {
            TypeConverters.Add(type, converter);
            return this;
        }
    }
}