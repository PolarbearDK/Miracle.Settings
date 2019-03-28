# Type Converters

Values are converted to target type using a Type Converters. Type converters controls how a value is constructed from one or more settings. 


## Build in type support
Miracle.Settings has build-in support for POCO (Plain old CLR objects) objects, and all types that implement IConvertible interface.

Miracle.Settings also has custom type converters for these types:

| Type          | Comment                                                                                                               | Multi value |
| ------------- | --------------------------------------------------------------------------------------------------------------------- | ----------- |
| DateTime      | ISO8601 converted to local date/time                                                                                  |             |
| DirectoryInfo | check that directory exist                                                                                            | *           |
| Enum          | incl. flags enum                                                                                                      |             |
| FileInfo      | check that file exist                                                                                                 | *           |
| Guid          | Any format that [Guid.Parse](https://msdn.microsoft.com/en-us/library/system.guid.parse.aspx) supports.               |             |
| IPAddress     | Any format that [IPAddress.Parse](https://msdn.microsoft.com/en-us/library/system.net.ipaddress.parse.aspx) supports. |             |
| TimeSpan      | Any format that [TimeSpan.Parse](https://msdn.microsoft.com/en-us/library/system.timespan.parse.aspx) supports.       |             |
| Type          | checks that type exist                                                                                                |             |
| Uri           | check that url is valid                                                                                               | 2           |

Multi value is covered in [Section about Reference(s) attribute](Annotations.md) 

## Add support for additional types
Support for additional types can be added by providing a type converter for the specific type.

## Custom Type Converter
The default DateTime converter expects ISO 8601 format converted to local. This example creates a type converter for DateTime that accept dates in danish locale format:

Sample:
```XML
<configuration>
  <appSettings>
    <add key="Christmas" value="24/12-2016" />
  </appSettings>
</configuration>
```

Type converters can be configured per setting loader instance.
```CSharp
ISettingsLoader settingsLoader = new SettingsLoader()
    // Add converter from Xml date/time format to DateTime
    .AddTypeConverter<DateTime>(x => DateTime.Parse(x, System.Globalization.CultureInfo.GetCultureInfo("da-DK")));
DateTime dateTime = settingsLoader.Create<DateTime>("DateTime");
```

Type converters can also be specified in-line to only affect one property.
```CSharp
public class FooSettings
{
    [Setting(TypeConverter = typeof(MyLocalDateTimeConverter))]
    public DateTime MyDateTimeProperty { get; set; }
}
```

## Implementing a custom type converter
Type converters must implement interface: Miracle.Settings.ITypeConverter
```CSharp
public interface ITypeConverter
{
    /// <summary>
    /// Check if <param name="values"/> can be converted to type <param name="conversionType"/>
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <param name="conversionType">Destination type to convert to</param>
    /// <returns>True if type converter is able to convert values to desired type, otherwise false</returns>
    bool CanConvert(object[] values, Type conversionType);

    /// <summary>
    /// Convert <param name="values"/> into instance of type <param name="conversionType"/>
    /// </summary>
    /// <param name="values">Values to convert</param>
    /// <param name="conversionType">Destination type to convert to</param>
    /// <returns>Instance of type <param name="conversionType"/></returns>
    object ChangeType(object[] values, Type conversionType);
}
```

Sample type converter:
```CSharp
/// <summary>
/// Type converter that combines several strings into a valid path.
/// </summary>
public class PathTypeConverter : ITypeConverter
{
    public bool CanConvert(object[] values, Type conversionType)
    {
        return conversionType == typeof(string) && values.Length > 0 && values.All(x=>x is string);
    }

    public object ChangeType(object[] values, Type conversionType)
    {
        return Path.Combine(values.Cast<string>().ToArray());
    }
}
```
## Add type converter
Type converters can be added to SettingLoader using fluid convenience methods:
- __AddTypeConverter<T>(Func<string, T> convert)__ Add simple type converter for type T using converter function.
- __AddTypeConverter(ITypeConverter typeConverter)__ Add type converter instance to front of list of type converters

The full list of current type converters are available in SettingLoader property: __TypeConverters__. This list can be manipulated to get full control over all type converting.
