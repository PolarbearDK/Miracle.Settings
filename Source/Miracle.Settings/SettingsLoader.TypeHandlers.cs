using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Miracle.Settings.Properties;

namespace Miracle.Settings
{
    public partial class SettingsLoader
    {
        private delegate bool TypeHandlerDelegate(PropertyInfo propertyInfo, string prefix, string key, out object value);

        private readonly List<TypeHandlerDelegate> _typeHandlers;

        private List<TypeHandlerDelegate> GetTypeHandlers()
        {
            return new List<TypeHandlerDelegate>
            {
                DirectGet,
                ArrayHandler,
                ListHandler,
                DictionaryHandler,
                NestedClassHandler,
            };
        }

        private bool DirectGet(PropertyInfo propertyInfo, string prefix, string key, out object value)
        {
            object propertyValue;
            var list = GetReferencesList(propertyInfo, prefix);
            if (TryGetPropertyValue(propertyInfo, key, out propertyValue))
            {
                list.Add(propertyValue);

                if (TryConstructPropertyValue(propertyInfo, list.ToArray(), out value))
                    return true;
            }
            else
            {
                // No value provided. Throw an error if any of the type converters can handle this property.
                list.Add(string.Empty);
                var values = list.ToArray();
                if (TypeConverters.Any(x => x.CanConvert(values, propertyInfo.PropertyType)))
                    throw new ConfigurationErrorsException(string.Format(Resources.MissingValueFormat, propertyInfo.PropertyType, key));
            }
            value = null;
            return false;
        }

        private bool ArrayHandler(PropertyInfo propertyInfo, string prefix, string key, out object value)
        {
            var propertyType = propertyInfo.PropertyType;
            if (propertyType.IsArray)
            {
                Type elementType = propertyType.GetElementType();
                value = GetType()
                    .GetMethod(nameof(CreateArray))
                    .MakeGenericMethod(elementType)
                    .Invoke(this, new object[] { key + PropertySeparator });

                if (value != null)
                    return true;

                if (TryGetPropertyValue(propertyInfo, key, out value))
                    return true;

                throw new ConfigurationErrorsException(string.Format(Resources.MissingValueFormat, propertyType, key));
            }
            value = null;
            return false;
        }

        private bool ListHandler(PropertyInfo propertyInfo, string prefix, string key, out object value)
        {
            var propertyType = propertyInfo.PropertyType;
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().IsGenericTypeDefinitionAssignableFrom(typeof(List<>)))
            {
                value =
                    GetType()
                        .GetMethod(nameof(CreateList))
                        .MakeGenericMethod(propertyType.GetGenericArguments())
                        .Invoke(this, new object[] { key + PropertySeparator });

                if (value != null)
                    return true;

                if (TryGetPropertyValue(propertyInfo, key, out value))
                    return true;

                throw new ConfigurationErrorsException(string.Format(Resources.MissingValueFormat, propertyType, key));
            }
            value = null;
            return false;
        }

        private bool DictionaryHandler(PropertyInfo propertyInfo, string prefix, string key, out object value)
        {
            var propertyType = propertyInfo.PropertyType;
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().IsGenericTypeDefinitionAssignableFrom(typeof(Dictionary<,>)))
            {
                value =
                    GetType()
                        .GetMethod(nameof(CreateDictionary))
                        .MakeGenericMethod(propertyType.GetGenericArguments())
                        .Invoke(this, new object[] { key + PropertySeparator, null });

                if (value != null)
                    return true;

                if (TryGetPropertyValue(propertyInfo, key, out value))
                    return true;

                throw new ConfigurationErrorsException(string.Format(Resources.MissingValueFormat, propertyType, key));
            }
            value = null;
            return false;
        }

        private bool NestedClassHandler(PropertyInfo propertyInfo, string prefix, string key, out object value)
        {
            var propertyType = propertyInfo.PropertyType;
            if (propertyType.IsClass && propertyType != typeof(string))
            {
                try
                {
                    value =
                        GetType()
                            .GetMethod(nameof(Create))
                            .MakeGenericMethod(propertyType)
                            .Invoke(this, new object[] {key});
                    return true;
                }
                // Remove the awfull TargetInvocationException
                catch (System.Reflection.TargetInvocationException ex)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                }
            }
            value = null;
            return false;
        }

        private List<object> GetReferencesList(PropertyInfo propertyInfo, string prefix)
        {
            var list = new List<object>();

            SettingAttribute attribute = propertyInfo.GetCustomAttributes(typeof (SettingAttribute), false).FirstOrDefault() as SettingAttribute;
            if (attribute != null && attribute.References != null)
            {
                foreach (var reference in attribute.References)
                {
                    var referenceKey = prefix + reference;
                    string referenceValue;
                    if (TryGetValue(referenceKey, out referenceValue))
                    {
                        list.Add(referenceValue);
                    }
                    else
                    {
                        throw new ConfigurationErrorsException(string.Format(Resources.MissingReferenceValueFormat, referenceKey));
                    }
                }
            }
            return list;
        }

        private bool TryGetPropertyValue(PropertyInfo propertyInfo, string key, out object value)
        {
            string stringValue;
            if (TryGetValue(key, out stringValue))
            {
                value = stringValue;
                return true;
            }

            // Get default value if DefaultValueAttribute is present
            DefaultValueAttribute attr = propertyInfo.GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault() as DefaultValueAttribute;
            if (attr != null)
            {
                value = attr.Value;
                return true;
            }

            value = null;
            return false;
        }

        private bool TryConstructPropertyValue(PropertyInfo propertyInfo, object[] values, out object value)
        {
            SettingAttribute attribute = propertyInfo.GetCustomAttributes(typeof(SettingAttribute), false).FirstOrDefault() as SettingAttribute;
            if (attribute != null && attribute.TypeConverter != null)
            {
                ITypeConverter typeConverter;
                try
                {
                    typeConverter = Activator.CreateInstance(attribute.TypeConverter) as ITypeConverter;
                }
                catch (Exception ex)
                {
                    throw new ConfigurationErrorsException(string.Format(Resources.CreateTypeConverterErrorFormat, attribute.TypeConverter), ex);
                }

                if (typeConverter == null)
                    throw new ConfigurationErrorsException(string.Format(Resources.BadExplicitTypeConverterTypeFormat,typeof (ITypeConverter)));

                if (typeConverter.CanConvert(values, propertyInfo.PropertyType))
                {
                    value = ChangeType(values, propertyInfo.PropertyType, typeConverter);
                    return true;
                }
                throw new ConfigurationErrorsException(
                    string.Format(
                        Resources.ExplicitTypeConverterErrorFormat,
                        string.Join(",", values.Select(x => x.ToString())),
                        propertyInfo.PropertyType));
            }

            value = ChangeType(values, propertyInfo.PropertyType);
            return true;
        }
    }
}
