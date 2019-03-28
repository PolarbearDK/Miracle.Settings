# Miracle.Settings

Load application settings into strong typed objects with two lines of code.

- No need to write code that load, convert and validate each configuration value.
- No "magic strings" and no defaults scattered around in source files.
- Loaded models are guaranteed to be valid.
- Get detailed error message on load error, describing exactly which setting has failed.
- Load settings at startup and application fails immediately upon bad configuration (Fail fast).
- Load complex structores like nested objects, arrays, lists and dictionaries.
- Use DI to inject strong typed setting models into application at startup.
- Works with Full .NET and .NET Core.

## Table of content

- [Miracle.Settings](#miraclesettings)
  - [Table of content](#table-of-content)
  - [Install](#install)
  - [Usage](#usage)
  - [Nested objects](#nested-objects)
  - [Arrays, Lists & Dictionaries](#arrays-lists--dictionaries)
- [Advanced topics](#advanced-topics)

## Install

Available as a NuGet package: [Miracle.Settings](https://www.nuget.org/packages/Miracle.Settings/)

To install Miracle.Settings, run the following command in the Package Manager Console

```Powershell|foo
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

```CSharp
// Setting model that appSettings are loaded into
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

This code loads settings of type of type FooBar into settings variable.

**Top Tip!** Load settings ONCE at startup, and expose settings to application using Dependency Injection.

## Nested objects

Nested objects are supported using property separator (configurable using: SettingLoader.PropertySeparator)

```XML
<configuration>
  <appSettings>
    <add key="MyPrefix:Foo" value="Foo string" />
    <add key="MyPrefix:Nested:Foo" value="Foo" />
    <add key="MyPrefix:Nested:Bar" value="42" />
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

Collections (Arrays, Lists & Dictionaries) can be loaded implicitly into setting model properties, or explicitly by calling CreateArray/CreateList/CreateDictionary .

Simple arrays and lists can be loaded from a single string value containing separated values. See [Controlling settings with annotations](Annotations.md)

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

Elements in Arrays and Lists are returned in the same order as they are returned by the value provider. The AppSettings value proveider returns values in the same order as they are specified.

# Advanced topics

- [Controlling settings with annotations](Annotations.md)
- [Type converters](TypeConverters.md)
- [Value providers](ValueProviders.md)
- [Validating settings](Validation.md)
