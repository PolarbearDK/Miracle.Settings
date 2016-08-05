# ValueProviders

Value providers provides the string values that SettingLoader use to initialize properties on setting objects..



## Built in ValueProviders
- __AppSettingsValueProvider__ (Default) - Load from AppSettings
- __EnvironmentValueProvider__ - Load from Environment.

## Change value provider
Value providers can be changed using the following SettingLoader methods:
- __AddProvider(...provider...)__ - Add new provider to end of value provider list.
- __ClearProviders()__ - Remove all providers from the value provider list.

The value proveders list is available through the SettingLoader property: __ValueProviders__

## IValueProvider interface
Value providers must implement interface:
```CSharp
public interface IValueProvider
{
    bool TryGetValue(string key, out string value);
    bool TryGetKeys(string prefix, out IEnumerable<string> keys);
}
```

