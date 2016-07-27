using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;

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
            if (TryGetPropertyValue(propertyInfo, key, out propertyValue))
            {
                var list = GetReferencesList(propertyInfo, prefix);
                list.Add(propertyValue);

                if (TryConstructPropertyValue(propertyInfo, list.ToArray(), out value))
                    return true;
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
                value =
                    GetType()
                        .GetMethod(nameof(CreateArray))
                        .MakeGenericMethod(elementType)
                        .Invoke(this, new object[] { key + PropertySeparator });
                return true;
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
                return true;
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
                return true;
            }
            value = null;
            return false;
        }

        private bool NestedClassHandler(PropertyInfo propertyInfo, string prefix, string key, out object value)
        {
            var propertyType = propertyInfo.PropertyType;
            if (propertyType.IsClass && propertyType != typeof(string))
            {
                value =
                    GetType()
                        .GetMethod(nameof(Create))
                        .MakeGenericMethod(propertyType)
                        .Invoke(this, new object[] { key });
                return true;
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
                        throw new ConfigurationErrorsException("A value has to be provided for referenced Setting: " + referenceKey);
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
                ITypeConverter typeConverter = Activator.CreateInstance(attribute.TypeConverter) as ITypeConverter;
                if (typeConverter == null)
                    throw new ConfigurationErrorsException("Setting TypeConverters must implement " + typeof (ITypeConverter));

                if (typeConverter.CanConvert(values, propertyInfo.PropertyType))
                {
                    value = typeConverter.ChangeType(values, propertyInfo.PropertyType);
                    return true;
                }
                throw new ConfigurationErrorsException($"Unable to convert values: {string.Join(",", values.Select(x => x.ToString()))} into type {propertyInfo.PropertyType}");
            }

            value = ChangeType(values, propertyInfo.PropertyType);
            return true;
        }
    }
}