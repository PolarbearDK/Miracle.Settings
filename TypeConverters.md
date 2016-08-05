# TypeConverters

Type converters controls how a value is constructed from one or more settings.

## Custom Type Converter
You can add custom type converters for types not supported out of the box.
A good examle is DateTime where string representation varies with localization. 
If you want a more "stable" representation you could use ISO 8601 format.


Sample:
```XML
<configuration>
  <appSettings>
    <add key="DateTime" value="2004-07-17T08:00:00.000000+01:00" />
  </appSettings>
</configuration>
```

Type converters can be configured per setting instance.
```CSharp
ISettingsLoader settingsLoader = new SettingsLoader()
    // Add converter from Xml date/time format to DateTime
    .AddTypeConverter<DateTime>(x => XmlConvert.ToDateTime(x, XmlDateTimeSerializationMode.Local));
DateTime dateTime = settingsLoader.Create<DateTime>("DateTime");
```

Type converters can be specified in-line to only affect one property.
```CSharp
public class FooSettings
{
    [Setting(TypeConverter = typeof(MyCustomDateTimeConverter))]
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
