# Miracle.Settings

Deserialize/Load settings into strong typed object (POCO).

Features:
* Load settings from config file, environment variables or custom source (Database is an obvious scenario)
* Load settings prefixed by a specific prefix, or from root (prefix=null)
* Throws exception in case of missing setting.
* Supports nested objects, arrays, lists and dictionaries.
* Construct property from more than one value.
* Extendable type convertion system
* Built in converters for Enum, Guid, Timespan and Uri.
* Built in converters for DirectoryInfo and FileInfo that check the existance of the file system object.

## Table of content
* [Usage](#usage)
* [Simple Example](#simple-example)
* [Using default value](#using-default-value)
* [Nested object](#Nested-object)
* [Arrays, Lists & Dictionaries](arrays-lists--dictionaries)

Advanced topics
* [Setting attribute](SettingAttribute.md)
* [Type converters](TypeConverters.md)
* [Value providers](ValueProviders.md)

## Usage
Available as a NuGet package: [Miracle.Settings](https://www.nuget.org/packages/Miracle.Settings/)

To install Miracle.Settings, run the following command in the Package Manager Console
```Powershell
PM> Install-Package Miracle.Settings
```


## Simple example
A basic example on how to load a POCO object with settings.
```XML
<configuration>
  <appSettings>
    <add key="Foo" value="Foo string" />
    <add key="Bar" value="42" />
  </appSettings>
</configuration>
```
```CSharp
public class FooBar
{
    public string Foo { get; set; }
    public int Bar { get; set; }
}
```
```CSharp
ISettingsLoader settingsLoader = new SettingsLoader();
// Get settings at "root" level (without a prefix) 
var settings = settingsLoader.Create<FooBar>();
```

## Using default value
If no value is provided for a property, then value is taken from DefaultValueAttribute.

Sample:
```XML
<configuration>
  <appSettings>
    <add key="Foo" value="Foo string" />
  </appSettings>
</configuration>
```
```CSharp
public class FooBar
{
    public string Foo { get; set; }
    [DefaultValue(42)]
    public int Bar { get; set; }
}
```
```CSharp
ISettingsLoader settingsLoader = new SettingsLoader();
// Foo is loaded from setting. 
// No value is provided in settings for Bar, but as it has a DefaultValueAttribute it gets a value of 42.
var settings = settingsLoader.Create<FooBar>();
```

## Nested object
Nested objects are supported using "dot" notation.

```XML
<configuration>
  <appSettings>
    <add key="MyPrefix.Foo" value="Foo string" />
    <add key="MyPrefix.Nested.Foo" value="Foo" />
    <add key="MyPrefix.Nested.Bar" value="42" />
  </appSettings>
</configuration>
```

```CSharp
public class FooBar
{
    public string Foo { get; set; }
    public Nested Nested { get; set; }
}

public class Nested
{
    public string Foo { get; set; }
    public int Bar { get; set; }
}
```

```CSharp
ISettingsLoader settingsLoader = new SettingsLoader();
// Get settings prefixed by "MyPrefix"
var settings = settingsLoader.Create<FooBar>("MyPrefix");
```

## Arrays, Lists & Dictionaries
Collections (Arrays, Lists & Dictionaries) can be loaded directly or as nested properties. 
Keys must be unique, so collection keys must be suffixed by something to make them unique.

```XML
<configuration>
  <appSettings>
    <add key="MyPrefix.1" value="Foo string" />
    <add key="MyPrefix.2" value="Foo" />
    <add key="MyPrefix.x" value="42" />
  </appSettings>
</configuration>
```

```CSharp
ISettingsLoader settingsLoader = new SettingsLoader();
// Get the same settings as array, list & dictionary.
// With array and list, the "key" path is lost. 
string[] settings1 = settingsLoader.CreateArray<string>("MyPrefix");
List<string> settings2 = settingsLoader.CreateList<string>("MyPrefix");
// With dictionary, the part of the key after prefix is used as dictionary key. 
// In this case this would produce keys: "1","2","x"
Dictionary<string,string> settings3 = settingsLoader.CreateDictionary<string>("MyPrefix");
```

## Custom Type Converter
Add custom type converters for types not supported out of the box. 

A good examle is DateTime which doesn't have a default converter due to DateTime objects varying formats due to localization and timezones.

Sample:
```XML
<configuration>
  <appSettings>
    <add key="DateTime" value="2004-07-17T08:00:00.000000+01:00" />
  </appSettings>
</configuration>
```
```CSharp
ISettingsLoader settingsLoader = new SettingsLoader()
    // Add converter from Xml date/time format to DateTime
    .AddTypeConverter(typeof(DateTime), s => XmlConvert.ToDateTime(s, XmlDateTimeSerializationMode.Local));
DateTime dateTime = settingsLoader.Create<DateTime>("DateTime");
```

## Rules
When loading a strong typed object, the following rules apply:

1. A value MUST be provided for each public property with a setter, or an exception is thrown.
2. Values are attempted loaded from value providers in the order they are specified. DefaultValueAttribute is always considered last.
3. Values are converted to target type using custom "TypeConverters" with fallback to Convert.ChangeType. Add custom type converter using: AddTypeConverter.
4. The key of the setting being loaded is calcualted as: (Prefix + PropertySeparator) + Name. No PropertySeparator is applied if Prefix is null (root). Name is the name of the property, but this can be overwritten by applying a SettingAttribute with "Name" property.
5. Elements in Arrays and Lists are returned in the same order as they are returned by the value provider. The AppSettings value proveider returns values in the same order as they are specified. 


