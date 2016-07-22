using System;
using System.Collections.Generic;

namespace Miracle.Settings
{
    public partial class SettingsLoader
    {
        private delegate bool TypeHandlerDelegate(Type propertyType, string key, out object value);
        private readonly List<TypeHandlerDelegate> _typeHandlers;

        private List<TypeHandlerDelegate> GetTypeHandlers()
        {
            return new List<TypeHandlerDelegate>
            {
                DirectGet,
                ShortCircuitHandledTypes,
                ArrayHandler,
                ListHandler,
                DictionaryHandler,
                NestedClassHandler,
            };
        }

        private bool DirectGet(Type propertyType, string key, out object value)
        {
            value = null;
            string stringValue;
            if (TryGetValue(key, out stringValue))
            {
                value = ChangeType(stringValue, propertyType);
                return true;
            }
            return false;
        }

        private bool ShortCircuitHandledTypes(Type propertyType, string key, out object value)
        {
            value = null;
            return TypeConverters.ContainsKey(propertyType);
        }

        private bool ArrayHandler(Type propertyType, string key, out object value)
        {
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

        private bool ListHandler(Type propertyType, string key, out object value)
        {
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

        private bool DictionaryHandler(Type propertyType, string key, out object value)
        {
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

        private bool NestedClassHandler(Type propertyType, string key, out object value)
        {
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
    }
}