# TypeConverters

Type converters controls how a value is constructed from one or more settings.

## Add type converter
Type converters can be added using fluid convenience methods:
- __AddTypeConverter<T>(Func<string, T> convert)__ Add simple type converter for type T using converter function.
- __AddTypeConverter(ITypeConverter typeConverter)__ Add type converter instance to front of list of type converters

The full list of current type converters are available in property: __TypeConverters__.

## Custom Type Converter
You can add custom type converters for types not supported out of the box.
A good examle is DateTime where string representation varies with localization. 
The default converter expects ISO 8601 format converted to local. If you want to specifiy dates in a different format, this can be done like this:

Sample:
```XML
<configuration>
  <appSettings>
    <add key="Christmas" value="24/12-2016" />
  </appSettings>
</configuration>
```

Type converters can be configured per setting instance.
```CSharp
ISettingsLoader settingsLoader = new SettingsLoader()
    // Add converter from Xml date/time format to DateTime
    .AddTypeConverter<DateTime>(x => DateTime.Parse(x, System.Globalization.CultureInfo.GetCultureInfo("da-DK")));
DateTime dateTime = settingsLoader.Create<DateTime>("DateTime");
```

Type converters can be specified in-line to only affect one property.
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
