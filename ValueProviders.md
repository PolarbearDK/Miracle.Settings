# ValueProviders

Value providers provides the string values that SettingLoader use to initialize properties on setting objects.

The key of the setting being loaded is calcualted as: (Prefix + PropertySeparator) + Name. No PropertySeparator is applied if Prefix is null (root). Name is the name os the property-name, but this can be overwritten by applying a SettingAttribute with "Name" property.

## Built in ValueProviders

- **AppSettingsValueProvider** (Default in Full .NET) - Load string from AppSettings
- **EnvironmentValueProvider** - Load strings from Environment variables.
- **DictionaryValueProvider** - Load strings from Dictionary<string,string> (great for unit tests).

## Register value provider

Alter the list of value providers by using the following SettingLoader methods:

- **AddProvider(...provider...)** - Register a provider to end of value provider list.
- **ClearProviders()** - Remove all providers from the value provider list.

The SettingLoader property: **ValueProviders** contains a list of all currently registered value providers.

## IValueProvider interface

Create custom value provider by implementing the IValueProvider interface:

```CSharp
public interface IValueProvider
{
    bool TryGetValue(string key, out string value);
    bool TryGetKeys(string prefix, out IEnumerable<string> keys);
}
```

Values are attempted loaded from value providers in the order they are specified. DefaultValueAttribute are always considered last.

How about loading appSettings from a central database? [See SQLServer sample](Source/Miracle.Settings.Tests/DatabaseTest/SampleSqlValueProvider.cs)
