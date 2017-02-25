# Miracle.Settings

Load your application settings into strong typed objects with two lines of code. 

The advantages are many:
* It's super simple.
* No need to write code that load, convert and validate each appSetting.
* No "magic strings" and no defaults scattered around in your source files.
* Your application fails immediately upon bad configuration.
* Get detailed error messages describing exactly which setting has failed. 
* Your settings are always valid.
* You can use DI to inject strong typed settings into your application.
* Supports nested objects, arrays, lists and dictionaries.
* Property validation.

Wait you might say... You can du that with ConfigurationSections! Well you can but there are many drawbacks, and you have to write a LOT of plumbing code.

## Table of content
* [Install](#install)
* [Usage](#usage)
* [Using default value](#using-default-value)
* [Nested object](#nested-object)
* [Arrays, Lists & Dictionaries](#arrays-lists--dictionaries)
* [Type support](#type-support)
* [Rules](#rules)

Advanced topics
* [Controlling deserialization with annotations](Annotatons.md)
* [Type converters](TypeConverters.md)
* [Value providers](ValueProviders.md)
* [Validating settings](Validation.md)

## Install
Available as a NuGet package: [Miracle.Settings](https://www.nuget.org/packages/Miracle.Settings/)

To install Miracle.Settings, run the following command in the Package Manager Console
```Powershell
PM> Install-Package Miracle.Settings
```
## Usage
A basic example on how to load settings into a POCO object.
```XML
<configuration>
  <appSettings>
    <add key="Foo" value="Foo string" />
    <add key="Bar" value="42" />
  </appSettings>
</configuration>
```
The setting that you wish to load from app.config or web.config.
```CSharp
public class FooBar
{
    public string Foo { get; private set; }
    public int Bar { get; private set; }
}
```
The POCO (Plain Old CLR Object) that settings are serialized/loaded into.
```CSharp
ISettingsLoader settingsLoader = new SettingsLoader();
// Get settings at "root" level (without a prefix) 
var settings = settingsLoader.Create<FooBar>();
```
This code loads settings of type of type FooBar into settings variable. Put this code somewhere in your application startup code.

Note! Load settings ONCE, and expose the initialized setting object to the rest of your code.

## Using default value
If no value is provided for a property, then value is taken from DefaultValueAttribute.

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
    public string Foo { get; private set; }
    [DefaultValue(42)]
    public int Bar { get; private set; }
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
    public string Foo { get; private set; }
    public Nested Nested { get; private set; }
}

public class Nested
{
    public string Foo { get; private set; }
    public int Bar { get; private set; }
}

// Get settings prefixed by "MyPrefix"
var settings = settingsLoader.Create<FooBar>("MyPrefix");
```

## Arrays, Lists & Dictionaries
Collections (Arrays, Lists & Dictionaries) can be loaded directly by calling CreateArray/CreateList/CreateDictionary or as properties in setting classes. 

Simple arrays and lists can be loaded from a single string value containing separated values. See [Controlling deserialization with annotations](Annotatons.md)

More advanced scenarios requires a separate value for each collection item. Keys must be unique, so collection keys must be suffixed by something to make them unique. 
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
// Get the same settings as array, list & dictionary.

// With array and list, the "key" path is lost. 
string[] settings1 = settingsLoader.CreateArray<string>("MyPrefix");
List<string> settings2 = settingsLoader.CreateList<string>("MyPrefix");

// With dictionary, the part of the key after prefix is used as dictionary key. 
// In this case this would produce keys: "1","2","x"
Dictionary<string,string> settings3 = settingsLoader.CreateDictionary<string>("MyPrefix");
```
## Type support
Settings can be loaded into all simple value types that implement IConvertible interface.
Support for additional types can be added by providing a type converter for the specific type.

Miracle.Settings has built in support for these additional types:

| Type | Comment |
| --- | --- |
| DateTime | ISO8601 converted to local date/time |
| DirectoryInfo | check that directory exist |
| Enum | incl. flags enum |
| FileInfo | check that file exist |
| Guid|Any format that [Guid.Parse](https://msdn.microsoft.com/en-us/library/system.guid.parse.aspx) supports. |
| IPAddress | Any format that [IPAddress.Parse](https://msdn.microsoft.com/en-us/library/system.net.ipaddress.parse.aspx) supports. |
| TimeSpan | Any format that [TimeSpan.Parse](https://msdn.microsoft.com/en-us/library/system.timespan.parse.aspx) supports. |
| Type | checks that type exist |
| Uri | check that url is valid |

## Rules
When loading a strong typed object, the following rules apply:

1. A value MUST be provided for each public property with a setter, or an exception is thrown.
2. Values are attempted loaded from value providers in the order they are specified. DefaultValueAttribute is always considered last.
3. Values are converted to target type using custom "TypeConverters" with fallback to Convert.ChangeType. Add custom type converter using: AddTypeConverter.
4. The key of the setting being loaded is calcualted as: (Prefix + PropertySeparator) + Name. No PropertySeparator is applied if Prefix is null (root). Name is the name os the property-name, but this can be overwritten by applying a SettingAttribute with "Name" property.
5. Elements in Arrays and Lists are returned in the same order as they are returned by the value provider. The AppSettings value proveider returns values in the same order as they are specified. 


