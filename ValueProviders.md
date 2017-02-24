# ValueProviders

Value providers provides the string values that SettingLoader use to initialize properties on setting objects.

## Built in ValueProviders
- __AppSettingsValueProvider__ (Default) - Load string from AppSettings
- __EnvironmentValueProvider__ - Load strings from Environment.

## Register value provider
Alter the list of value providers by using the following SettingLoader methods:
- __AddProvider(...provider...)__ - Register a provider to end of value provider list.
- __ClearProviders()__ - Remove all providers from the value provider list.

The SettingLoader property: __ValueProviders__ contains a list of all currently registered value providers.

## IValueProvider interface
Create your own custom value provider by implementing the IValueProvider interface:

Value providers must implement interface:
```CSharp
public interface IValueProvider
{
    bool TryGetValue(string key, out string value);
    bool TryGetKeys(string prefix, out IEnumerable<string> keys);
}
```
How about loading appSettings from a central database?

